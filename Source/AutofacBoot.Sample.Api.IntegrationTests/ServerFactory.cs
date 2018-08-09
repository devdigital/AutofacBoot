// <copyright file="ServerFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using AutofacBoot.Sample.Api.Bootstrap;
    using AutofacBoot.Test;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Server factory.
    /// </summary>
    public class ServerFactory : TestServerFactory<ServerFactory>
    {
        /// <summary>
        /// Gets a value indicating whether the configuration has been invoked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the configuration has been invoked; otherwise, <c>false</c>.
        /// </value>
        public bool ConfigurationInvoked { get; private set; }

        /// <inheritdoc />
        protected override Task<ITaskResolver> GetTaskResolver()
        {
            ITaskResolver taskResolver = new AssemblyTaskResolver(
                typeof(ServiceBootstrapTask).Assembly);

            return Task.FromResult(taskResolver);
        }

        /// <inheritdoc />
        protected override IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder.UseKestrel(options =>
            {
                options.Listen(IPAddress.Any, 443, listenOptions =>
                {
                    listenOptions.UseHttps("server.pfx");
                });
            });
        }

        /// <inheritdoc />
        protected override Task<IDictionary<string, string>> GetConfiguration()
        {
            IDictionary<string, string> configuration = new Dictionary<string, string>
            {
                { "Test", "TestValue" },
            };

            this.ConfigurationInvoked = true;
            return Task.FromResult(configuration);
        }
    }
}