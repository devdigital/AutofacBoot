using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot
{
    internal class OrderedTaskResolver : ITaskResolver
    {
        private readonly ITaskResolver taskResolver;

        private readonly ITaskOrderer taskOrderer;

        public OrderedTaskResolver(ITaskResolver taskResolver, ITaskOrderer taskOrderer)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            this.taskOrderer = taskOrderer ?? throw new ArgumentNullException(nameof(taskOrderer));
        }

        public async Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var tasks = await this.taskResolver.GetConfigurationTasks();
            return await this.taskOrderer.Order(tasks);
        }

        public async Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            var tasks = await this.taskResolver.GetServiceTasks();
            return await this.taskOrderer.Order(tasks);
        }

        public async Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            var tasks = await this.taskResolver.GetContainerTasks();
            return await this.taskOrderer.Order(tasks);
        }

        public async Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            return await this.taskResolver.GetApplicationTaskTypes();
        }
    }
}