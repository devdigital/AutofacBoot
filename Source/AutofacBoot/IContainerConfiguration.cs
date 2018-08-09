// <copyright file="IContainerConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Container configuration.
    /// </summary>
    public interface IContainerConfiguration
    {
        /// <summary>
        /// Configures the container builder.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="builder">The builder.</param>
        /// <returns>The task.</returns>
        Task Configure(
            IHostingEnvironment environment,
            ContainerBuilder builder);
    }
}