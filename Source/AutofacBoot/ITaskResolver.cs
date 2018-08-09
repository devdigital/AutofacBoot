// <copyright file="ITaskResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Task resolver.
    /// </summary>
    public interface ITaskResolver
    {
        /// <summary>
        /// Gets the configuration tasks.
        /// </summary>
        /// <returns>The configuration tasks.</returns>
        Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks();

        /// <summary>
        /// Gets the service tasks.
        /// </summary>
        /// <returns>The service tasks.</returns>
        Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks();

        /// <summary>
        /// Gets the container tasks.
        /// </summary>
        /// <returns>The container tasks.</returns>
        Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks();

        /// <summary>
        /// Gets the application task types.
        /// </summary>
        /// <returns>The application task types.</returns>
        Task<IEnumerable<Type>> GetApplicationTaskTypes();
    }
}