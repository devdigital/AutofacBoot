// <copyright file="CompositeContainerConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Test
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Composite container configuration.
    /// </summary>
    /// <seealso cref="AutofacBoot.IContainerConfiguration" />
    public class CompositeContainerConfiguration : IContainerConfiguration
    {
        private readonly IContainerConfiguration[] containerConfigurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeContainerConfiguration"/> class.
        /// </summary>
        /// <param name="containerConfigurations">The container configurations.</param>
        public CompositeContainerConfiguration(params IContainerConfiguration[] containerConfigurations)
        {
            this.containerConfigurations = containerConfigurations ??
                throw new ArgumentNullException(nameof(containerConfigurations));
        }

        /// <inheritdoc />
        public async Task Configure(IHostingEnvironment environment, ContainerBuilder builder)
        {
            foreach (var configuration in this.containerConfigurations)
            {
                if (configuration == null)
                {
                    continue;
                }

                await configuration.Configure(environment, builder);
            }
        }
    }
}