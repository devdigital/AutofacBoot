// <copyright file="IConfigurationBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configuration bootstrap task.
    /// </summary>
    public interface IConfigurationBootstrapTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="configurationBuilder">The configuration builder.</param>
        /// <returns>The task.</returns>
        Task Execute(
            IHostingEnvironment environment,
            IConfigurationBuilder configurationBuilder);
    }
}