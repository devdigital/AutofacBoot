using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public class NumberedTaskOrderer : ITaskOrderer
    {        
        public Task<IEnumerable<TTask>> Order<TTask>(IEnumerable<TTask> tasks)
        {

            var orderedTasks = tasks.Select(t =>
            {
                var order = t is IOrderedTask ordered ? ordered.Order : 0;
                return new { Order = order, Task = t };
            }).OrderBy(t => t.Order);

            return Task.FromResult(orderedTasks.Select(t => t.Task));
        }
    }
}