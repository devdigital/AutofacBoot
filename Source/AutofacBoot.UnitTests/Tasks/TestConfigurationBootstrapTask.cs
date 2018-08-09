// <copyright file="TestConfigurationBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Tasks
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Test configuration bootstrap task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IConfigurationBootstrapTask" />
    public class TestConfigurationBootstrapTask : IConfigurationBootstrapTask
    {
        /// <inheritdoc />
        public Task Execute(
            IHostingEnvironment environment,
            IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            return Task.FromResult(0);
        }
    }
}