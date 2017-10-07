using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public class NumberedOrderedTaskResolver : IOrderedTaskResolver
    {
        public Task<IEnumerable<IConfigurationBootstrapTask>> GetOrderedConfigurationTasks(IEnumerable<IConfigurationBootstrapTask> configurationTasks)
        {
            return Task.FromResult(GetOrderedTasks(configurationTasks));
        }
        
        public Task<IEnumerable<IServiceBootstrapTask>> GetOrderedServiceTasks(IEnumerable<IServiceBootstrapTask> serviceTasks)
        {
            return Task.FromResult(GetOrderedTasks(serviceTasks));
        }

        public Task<IEnumerable<IContainerBootstrapTask>> GetOrderedContainerTasks(IEnumerable<IContainerBootstrapTask> containerTasks)
        {
            return Task.FromResult(GetOrderedTasks(containerTasks));
        }

        public Task<IEnumerable<Type>> GetOrderedApplicationTasks(IEnumerable<Type> applicationTasks)
        {
            return Task.FromResult(GetOrderedTasks(applicationTasks));
        }

        private static IEnumerable<T> GetOrderedTasks<T>(IEnumerable<T> tasks)
        {
            var orderedTasks = tasks.Select(t =>
            {
                var order = t is IOrderedTask ordered ? ordered.Order : 0;
                return new { Order = order, Task = t };
            }).OrderBy(t => t.Order);

            return orderedTasks.Select(t => t.Task);
        }
    }
}