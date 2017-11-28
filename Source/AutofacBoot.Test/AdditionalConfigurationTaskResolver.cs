using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutofacBoot.Test
{
    internal class AdditionalConfigurationTaskResolver : ITaskResolver
    {
        private readonly ITaskResolver taskResolver;

        private readonly IDictionary<string, string> configuration;

        public AdditionalConfigurationTaskResolver(ITaskResolver taskResolver, IDictionary<string, string> configuration)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            this.configuration = configuration;
        }

        public async Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var tasks = await this.taskResolver.GetConfigurationTasks();
            return this.configuration == null
                ? tasks
                : tasks.Concat(new [] { new DictionaryConfigurationBootstrapTask(this.configuration) });
        }

        public async Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            return await this.taskResolver.GetServiceTasks();
        }

        public async Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            return await this.taskResolver.GetContainerTasks();
        }

        public async Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            return await this.taskResolver.GetApplicationTaskTypes();
        }
    }
}