// <copyright file="TestApplicationTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Tasks
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Test application task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IApplicationBootstrapTask" />
    public class TestApplicationTask : IApplicationBootstrapTask
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly IConfigurationRoot configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestApplicationTask"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        public TestApplicationTask(
            ILoggerFactory loggerFactory,
            IConfigurationRoot configuration)
        {
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public Task Execute(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}