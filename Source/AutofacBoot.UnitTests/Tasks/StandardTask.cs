// <copyright file="StandardTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Tasks
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Standard task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IConfigurationBootstrapTask" />
    public class StandardTask : IConfigurationBootstrapTask
    {
        /// <inheritdoc />
        public Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)
        {
            throw new NotImplementedException();
        }
    }
}