using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public interface ITaskResolver
    {
        Task<IEnumerable<Type>> GetConfigurationTasks();

        Task<IEnumerable<Type>> GetContainerTasks();

        Task<IEnumerable<Type>> GetBootstrapTasks();
    }
}