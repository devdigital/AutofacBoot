// <copyright file="TestServerFactory.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutofacBoot.Test.Sources;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;

    /// <summary>
    /// Test server factory.
    /// </summary>
    /// <typeparam name="TServerFactory">The type of the server factory.</typeparam>
    public abstract class TestServerFactory<TServerFactory>
        where TServerFactory : TestServerFactory<TServerFactory>
    {
        private readonly DictionaryContainerConfiguration containerConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestServerFactory{TServerFactory}"/> class.
        /// </summary>
        protected TestServerFactory()
        {
            this.containerConfiguration = new DictionaryContainerConfiguration();
        }

        /// <summary>
        /// Adds type registration.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>The server factory.</returns>
        public TServerFactory With<TInterface, TImplementation>()
        {
            this.containerConfiguration.With<TInterface, TImplementation>();
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds instance registration.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory With<TInterface>(object instance)
        {
            this.containerConfiguration.With<TInterface>(instance);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfiguration(IServerFactoryConfiguration<TServerFactory> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configuration.Configure(this as TServerFactory);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds the configurations.
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithConfigurations(params IServerFactoryConfiguration<TServerFactory>[] configurations)
        {
            if (configurations == null)
            {
                throw new ArgumentNullException(nameof(configurations));
            }

            foreach (var configuration in configurations)
            {
                this.WithConfiguration(configuration);
            }

            return this as TServerFactory;
        }

        /// <summary>
        /// Creates the test server.
        /// </summary>
        /// <returns>The test server.</returns>
        public virtual async Task<TestServer> Create()
        {
            var taskResolver = await this.GetAdditionalConfigurationTaskResolver();

            var hostBuilder = new AutofacBootstrapper()
                .WithTasks(taskResolver)
                .WithContainer(this.containerConfiguration)
                .Configure();

            hostBuilder = this.Configure(hostBuilder);

            return new TestServer(hostBuilder);
        }

        /// <summary>
        /// Gets the task resolver.
        /// </summary>
        /// <returns>The task resolver.</returns>
        protected abstract Task<ITaskResolver> GetTaskResolver();

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>The configuration.</returns>
        protected virtual Task<IDictionary<string, string>> GetConfiguration()
        {
            return Task.FromResult<IDictionary<string, string>>(null);
        }

        /// <summary>
        /// Configures the web host builder.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <returns>The web host builder.</returns>
        protected virtual IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder;
        }

        private async Task<ITaskResolver> GetAdditionalConfigurationTaskResolver()
        {
            var taskResolver = await this.GetTaskResolver();
            var configuration = await this.GetConfiguration();
            return new AdditionalConfigurationTaskResolver(taskResolver, configuration);
        }
    }
}