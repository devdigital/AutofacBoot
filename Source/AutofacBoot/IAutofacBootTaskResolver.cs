using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public interface IAutofacBootTaskResolver
    {
        Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks();

        Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks();

        Task<IEnumerable<Type>> GetApplicationTaskTypes();
    }
}