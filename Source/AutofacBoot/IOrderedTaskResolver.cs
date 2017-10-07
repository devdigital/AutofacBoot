using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public interface IOrderedTaskResolver
    {
        Task<IEnumerable<IConfigurationBootstrapTask>> GetOrderedConfigurationTasks(
            IEnumerable<IConfigurationBootstrapTask> configurationTasks);

        Task<IEnumerable<IServiceBootstrapTask>> GetOrderedServiceTasks(
            IEnumerable<IServiceBootstrapTask> serviceTasks);

        Task<IEnumerable<IContainerBootstrapTask>> GetOrderedContainerTasks(
            IEnumerable<IContainerBootstrapTask> containerTasks);

        Task<IEnumerable<Type>> GetOrderedApplicationTasks(
            IEnumerable<Type> applicationTasks);
    }
}