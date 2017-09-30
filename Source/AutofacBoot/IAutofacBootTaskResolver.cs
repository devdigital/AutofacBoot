using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public interface IAutofacBootTaskResolver
    {
        Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks();

        Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks();

        Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks();

        Task<IEnumerable<Type>> GetApplicationTaskTypes();        
    }
}