// <copyright file="NumberedTaskOrderer.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Numbered task orderer.
    /// </summary>
    /// <seealso cref="AutofacBoot.ITaskOrderer" />
    public class NumberedTaskOrderer : ITaskOrderer
    {
        /// <inheritdoc/>
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