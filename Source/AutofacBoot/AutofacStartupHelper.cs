using System;
using System.Collections.Generic;
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
        private readonly ITaskResolver taskResolver;

        private readonly ITaskOrderer taskOrderer;

        private IHostingEnvironment hostingEnvironment;

        private IConfigurationRoot configuration;

        public AutofacStartupHelper(ITaskResolver taskResolver, ITaskOrderer taskOrderer)
        {
            this.taskResolver = new OrderedTaskResolver(taskResolver, taskOrderer);
            this.taskOrderer = taskOrderer ?? throw new ArgumentNullException(nameof(taskOrderer));
        }

        public IConfigurationRoot Configuration(IHostingEnvironment environment)
        {
            return this.ConfigurationAsync(environment)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IConfigurationRoot> ConfigurationAsync(IHostingEnvironment environment)
        {
            this.hostingEnvironment = environment;
            var configurationTasks = await this.taskResolver.GetConfigurationTasks();

            var configurationBuilder = new ConfigurationBuilder();
            foreach (var configurationTask in configurationTasks)
            {
                await configurationTask.Execute(
                    this.hostingEnvironment, 
                    configurationBuilder);
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
                    var canExecute = await conditional.CanExecute(
                        this.hostingEnvironment,
                        this.configuration);

                    if (canExecute)
                    {
                        await serviceTask.Execute(
                            this.hostingEnvironment,
                            this.configuration, 
                            services);
                    }

                    continue;
                }

                await serviceTask.Execute(
                    this.hostingEnvironment,
                    this.configuration, 
                    services);
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
                    var canExecute = await conditional.CanExecute(
                        this.hostingEnvironment,
                        this.configuration);

                    if (canExecute)
                    {
                        await containerTask.Execute(
                            this.hostingEnvironment,
                            this.configuration, 
                            builder);
                    }

                    continue;
                }

                await containerTask.Execute(
                    this.hostingEnvironment,
                    this.configuration, 
                    builder);
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

        public async Task ConfigureAsync(IApplicationBuilder app, IEnumerable<IApplicationBootstrapTask> applicationTasks)
        {
            var orderedTasks = await this.taskOrderer.Order(applicationTasks);

            foreach (var applicationTask in orderedTasks)
            {
                if (applicationTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(
                        this.hostingEnvironment,
                        this.configuration);

                    if (canExecute)
                    {
                        await applicationTask.Execute(app);
                    }

                    continue;
                }

                await applicationTask.Execute(app);
            }
        }      
    }
}