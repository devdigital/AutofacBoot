using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public class AutofacStartupHelper
    {
        private readonly IAutofacBootTaskResolver taskResolver;

        private IConfigurationRoot configuration;

        public AutofacStartupHelper()
        {
            this.taskResolver = new AssemblyTaskResolver(Assembly.GetExecutingAssembly());
        }

        public AutofacStartupHelper(IAutofacBootTaskResolver taskResolver)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
        }

        public IConfigurationRoot Configuration(IHostingEnvironment environment)
        {
            return this.ConfigurationAsync(environment)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IConfigurationRoot> ConfigurationAsync(IHostingEnvironment environment)
        {
            var configurationTasks = await this.taskResolver.GetConfigurationTasks();

            var configurationBuilder = new ConfigurationBuilder();
            foreach (var configurationTask in configurationTasks)
            {
                await configurationTask.Execute(configurationBuilder, environment);
            }

            this.configuration = configurationBuilder.Build();
            return this.configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureServicesAsync(services).GetAwaiter().GetResult();
        }

        private async Task ConfigureServicesAsync(IServiceCollection services)
        {
            var serviceTasks = await this.taskResolver.GetServiceTasks();

            foreach (var serviceTask in serviceTasks)
            {
                if (serviceTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(this.configuration);
                    if (canExecute)
                    {
                        await serviceTask.Execute(this.configuration, services);
                    }

                    continue;
                }

                await serviceTask.Execute(this.configuration, services);
            }
        }

        public void ConfigureContainer(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            this.ConfigureContainerAsync(builder, configuration).GetAwaiter().GetResult();
        }

        public async Task ConfigureContainerAsync(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            builder.RegisterInstance(configuration);

            var containerTasks = await this.taskResolver.GetContainerTasks();
       
            foreach (var containerTask in containerTasks)
            {
                if (containerTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(this.configuration);
                    if (canExecute)
                    {
                        await containerTask.Execute(this.configuration, builder);
                    }

                    continue;
                }

                await containerTask.Execute(this.configuration, builder);
            }

            var bootstrapTaskTypes = await this.taskResolver.GetApplicationTaskTypes();
            foreach (var bootstrapTaskType in bootstrapTaskTypes)
            {
                builder.RegisterType(bootstrapTaskType).As<IApplicationBootstrapTask>();
            }
        }

        public void Configure(IApplicationBuilder app, IEnumerable<IApplicationBootstrapTask> bootstrapTasks)
        {
            this.ConfigureAsync(app, bootstrapTasks).GetAwaiter().GetResult();
        }

        public async Task ConfigureAsync(IApplicationBuilder app, IEnumerable<IApplicationBootstrapTask> bootstrapTasks)
        {
            foreach (var bootstrapTask in bootstrapTasks)
            {
                if (bootstrapTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(this.configuration);
                    if (canExecute)
                    {
                        await bootstrapTask.Execute(app);
                    }

                    continue;
                }

                await bootstrapTask.Execute(app);
            }
        }      
    }
}