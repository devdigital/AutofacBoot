// <copyright file="ExceptionThrowingContainerConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Services
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Exception throwing container configuration.
    /// </summary>
    /// <seealso cref="AutofacBoot.IContainerConfiguration" />
    public class ExceptionThrowingContainerConfiguration : IContainerConfiguration
    {
        /// <inheritdoc />
        public Task Configure(IHostingEnvironment environment, ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}