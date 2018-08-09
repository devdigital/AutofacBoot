// <copyright file="IContainerBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Container bootstrap task.
    /// </summary>
    public interface IContainerBootstrapTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="builder">The builder.</param>
        /// <returns>The task.</returns>
        Task Execute(
            IHostingEnvironment environment,
            IConfigurationRoot configuration,
            ContainerBuilder builder);
    }
}
