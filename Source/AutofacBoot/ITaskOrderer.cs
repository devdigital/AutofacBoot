// <copyright file="ITaskOrderer.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Task orderer.
    /// </summary>
    public interface ITaskOrderer
    {
        /// <summary>
        /// Orders the specified tasks.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <param name="tasks">The tasks.</param>
        /// <returns>The ordered tasks.</returns>
        Task<IEnumerable<TTask>> Order<TTask>(IEnumerable<TTask> tasks);
    }
}