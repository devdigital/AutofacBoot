// <copyright file="HighNumberedTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Tasks
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// High numbered task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IConfigurationBootstrapTask" />
    /// <seealso cref="AutofacBoot.IOrderedTask" />
    public class HighNumberedTask : IConfigurationBootstrapTask, IOrderedTask
    {
        /// <inheritdoc />
        public int Order => 10;

        /// <inheritdoc />
        public Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)
        {
            throw new NotImplementedException();
        }
    }
}