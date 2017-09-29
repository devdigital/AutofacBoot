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
        private readonly ITaskResolver taskResolver;

        public AutofacStartup()
        {
            this.taskResolver = new AssemblyTaskResolver(Assembly.GetExecutingAssembly());
        }

        public AutofacStartup(ITaskResolver taskResolver)
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

            var containerTaskTypes = await this.taskResolver.GetContainerTasks();
            var containerTasks = containerTaskTypes
                .Where(t => typeof(IContainerBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IContainerBootstrapTask)Activator.CreateInstance(t));

            foreach (var containerTask in containerTasks)
            {
                await containerTask.Execute(builder);
            }

            var bootstrapTaskTypes = await this.taskResolver.GetBootstrapTasks();
            foreach (var bootstrapTaskType in bootstrapTaskTypes)
            {
                builder.RegisterType(bootstrapTaskType).As<IBootstrapTask>();
            }
        }

        public void Configure(IApplicationBuilder app, IEnumerable<IBootstrapTask> bootstrapTasks)
        {
            this.ConfigureAsync(app, bootstrapTasks).GetAwaiter().GetResult();
        }

        public async Task ConfigureAsync(IApplicationBuilder app, IEnumerable<IBootstrapTask> bootstrapTasks)
        {
            foreach (var bootstrapTask in bootstrapTasks)
            {
                await bootstrapTask.Execute(app);
            }
        }

        public IConfigurationRoot ConfigureFoo(IHostingEnvironment environment)
        {
            return this.ConfigureFooAsync(environment)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IConfigurationRoot> ConfigureFooAsync(IHostingEnvironment environment)
        {
            var configurationTaskTypes = await this.taskResolver.GetConfigurationTasks();
            var configurationTasks = configurationTaskTypes
                .Where(t => typeof(IConfigurationBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IConfigurationBootstrapTask)Activator.CreateInstance(t));

            var configurationBuilder = new ConfigurationBuilder();
            foreach (var configurationTask in configurationTasks)
            {
                await configurationTask.Execute(configurationBuilder, environment);
            }

            return configurationBuilder.Build();
        }
    }
}