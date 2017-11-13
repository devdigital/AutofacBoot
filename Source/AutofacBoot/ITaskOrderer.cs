using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public interface ITaskOrderer
    {
        Task<IEnumerable<TTask>> Order<TTask>(IEnumerable<TTask> tasks);        
    }
}