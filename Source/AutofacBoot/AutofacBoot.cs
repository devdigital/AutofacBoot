using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public class AutofacBoot
    {
        private readonly ITaskResolver taskResolver;

        public AutofacBoot()
        {
            this.taskResolver = new AssemblyTaskResolver(Assembly.GetExecutingAssembly());
        }

        public AutofacBoot(ITaskResolver taskResolver)
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
    }
}