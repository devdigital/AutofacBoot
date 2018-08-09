// <copyright file="ApplicationBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Application bootstrap task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IApplicationBootstrapTask" />
    public class ApplicationBootstrapTask : IApplicationBootstrapTask
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly IConfigurationRoot configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBootstrapTask"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        public ApplicationBootstrapTask(ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public Task Execute(IApplicationBuilder app)
        {
            this.loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
            this.loggerFactory.AddDebug();

            app.UseMvc();

            return Task.CompletedTask;
        }
    }
}