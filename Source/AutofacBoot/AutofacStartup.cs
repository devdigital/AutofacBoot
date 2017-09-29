using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    public class AutofacStartup
    {
        private readonly IAutofacBootTaskResolver taskResolver;

        public AutofacStartup()
        {
            this.taskResolver = new AssemblyTaskResolver(Assembly.GetExecutingAssembly());
        }

        public AutofacStartup(IAutofacBootTaskResolver taskResolver)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
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
                await containerTask.Execute(builder);
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
                await bootstrapTask.Execute(app);
            }
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

            return configurationBuilder.Build();
        }
    }
}