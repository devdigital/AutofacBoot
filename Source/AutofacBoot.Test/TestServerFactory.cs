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
        private readonly DictionaryContainerConfiguration dictionaryConfiguration;

        private readonly IDictionary<string, IAppBuilderConfiguration> appBuilderConfigurations;

        private IContainerConfiguration currentContainerConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestServerFactory{TServerFactory}"/> class.
        /// </summary>
        protected TestServerFactory()
        {
            this.dictionaryConfiguration = new DictionaryContainerConfiguration();
            this.appBuilderConfigurations = new Dictionary<string, IAppBuilderConfiguration>();
        }

        /// <summary>
        /// Adds type registration.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>The server factory.</returns>
        public TServerFactory With<TInterface, TImplementation>()
        {
            this.dictionaryConfiguration.With<TInterface, TImplementation>();
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
            this.dictionaryConfiguration.With<TInterface>(instance);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds container configuration.
        /// </summary>
        /// <param name="containerConfiguration">The container configuration.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithContainerConfiguration(IContainerConfiguration containerConfiguration)
        {
            this.currentContainerConfiguration = containerConfiguration ??
                throw new ArgumentNullException(nameof(containerConfiguration));

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
        /// Adds the application builder configuration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="appBuilderConfiguration">The application builder configuration.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithAppBuilderConfiguration(string id, IAppBuilderConfiguration appBuilderConfiguration)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (appBuilderConfiguration == null)
            {
                throw new ArgumentNullException(nameof(appBuilderConfiguration));
            }

            if (this.appBuilderConfigurations.ContainsKey(id))
            {
                throw new InvalidOperationException($"Application builder with identifier '{id}' already added.");
            }

            this.appBuilderConfigurations.Add(id, appBuilderConfiguration);
            return this as TServerFactory;
        }

        /// <summary>
        /// Adds the application builder configuration.
        /// </summary>
        /// <typeparam name="TAppBuilderConfiguration">The type of the application builder configuration.</typeparam>
        /// <param name="appBuilderConfiguration">The application builder configuration.</param>
        /// <returns>The server factory.</returns>
        public TServerFactory WithAppBuilderConfiguration<TAppBuilderConfiguration>(TAppBuilderConfiguration appBuilderConfiguration)
            where TAppBuilderConfiguration : IAppBuilderConfiguration
        {
            return this.WithAppBuilderConfiguration(typeof(TAppBuilderConfiguration).FullName, appBuilderConfiguration);
        }

        /// <summary>
        /// Creates the test server.
        /// </summary>
        /// <returns>The test server.</returns>
        public virtual async Task<TestServer> Create()
        {
            var hostBuilder = await this.GetWebHostBuilder();
            return new TestServer(hostBuilder);
        }

        /// <summary>
        /// Gets the web host builder.
        /// </summary>
        /// <returns>The web host builder.</returns>
        public async Task<IWebHostBuilder> GetWebHostBuilder()
        {
            var taskResolver = await this.GetAdditionalConfigurationTaskResolver();

            var hostBuilder = new AutofacBootstrapper()
                .WithTasks(taskResolver)
                .WithContainer(new CompositeContainerConfiguration(this.currentContainerConfiguration, this.dictionaryConfiguration))
                .WithAppBuilderConfigurations(this.appBuilderConfigurations)
                .Configure();

            return this.Configure(hostBuilder);
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