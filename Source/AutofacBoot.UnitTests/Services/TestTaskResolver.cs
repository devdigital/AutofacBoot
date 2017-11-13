using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacBoot.UnitTests.Tasks;

namespace AutofacBoot.UnitTests.Services
{
    public class TestTaskResolver : ITaskResolver
    {
        public Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var tasks = new List<IConfigurationBootstrapTask>
            {
                new TestConfigurationBootstrapTask()
            };

            return Task.FromResult(tasks.AsEnumerable());
        }

        public Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            return Task.FromResult(Enumerable.Empty<IServiceBootstrapTask>());
        }

        public Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            return Task.FromResult(Enumerable.Empty<IContainerBootstrapTask>());
        }

        public Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            var tasks = new List<Type>
            {
                typeof(TestApplicationTask)
            };

            return Task.FromResult(tasks.AsEnumerable());
        }
    }
}