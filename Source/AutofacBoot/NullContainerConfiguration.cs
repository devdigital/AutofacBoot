// <copyright file="NullContainerConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Null container configuration.
    /// </summary>
    /// <seealso cref="AutofacBoot.IContainerConfiguration" />
    public class NullContainerConfiguration : IContainerConfiguration
    {
        /// <inheritdoc />
        public Task Configure(IHostingEnvironment environment, ContainerBuilder builder)
        {
            return Task.CompletedTask;
        }
    }
}