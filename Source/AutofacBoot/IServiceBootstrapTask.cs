// <copyright file="IServiceBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Service bootstrap task.
    /// </summary>
    public interface IServiceBootstrapTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="services">The services.</param>
        /// <returns>The task.</returns>
        Task Execute(
            IHostingEnvironment environment,
            IConfigurationRoot configuration,
            IServiceCollection services);
    }
}