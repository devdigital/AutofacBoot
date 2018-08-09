// <copyright file="HostBuilderFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Host builder factory.
    /// </summary>
    internal class HostBuilderFactory
    {
        /// <summary>
        /// Creates the web host builder.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="taskResolver">The task resolver.</param>
        /// <param name="taskOrderer">The task orderer.</param>
        /// <param name="containerConfiguration">The container configuration.</param>
        /// <param name="appBuilderConfigurations">The application builder configurations.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>The web host builder.</returns>
        public IWebHostBuilder Create(
            string[] arguments,
            ITaskResolver taskResolver,
            ITaskOrderer taskOrderer,
            IContainerConfiguration containerConfiguration,
            IDictionary<string, IAppBuilderConfiguration> appBuilderConfigurations,
            Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            var hostBuilder = arguments == null
                ? Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                : Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(arguments);

            var webHostBuilder = hostBuilder.ConfigureServices(services =>
                {
                    var appBuilderConfigurationResolver = appBuilderConfigurations == null
                        ? (IAppBuilderConfigurationResolver)new NullAppBuilderConfigurationResolver()
                        : new DefaultAppBuilderConfigurationResolver(appBuilderConfigurations);

                    services.AddSingleton(taskResolver ?? AssemblyTaskResolver.Default);
                    services.AddSingleton(taskOrderer ?? new NumberedTaskOrderer());
                    services.AddSingleton(containerConfiguration ?? new NullContainerConfiguration());
                    services.AddSingleton(appBuilderConfigurationResolver);
                    services.AddAutofac();
                })
                .UseStartup<AutofacBootStartup>();

            return new WebHostBuilder(webHostBuilder, exceptionHandler);
        }
    }
}