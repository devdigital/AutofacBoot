// <copyright file="AutofacBootStartup.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Linq;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutofacBoot startup.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Hosting.IStartup" />
    public class AutofacBootStartup : IStartup
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly IHostingEnvironment hostingEnvironment;

        private readonly AutofacBootStartupHelper startupHelper;

        private readonly IContainerConfiguration containerConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootStartup"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="taskResolver">The task resolver.</param>
        /// <param name="taskOrderer">The task orderer.</param>
        /// <param name="containerConfiguration">The container configuration.</param>
        public AutofacBootStartup(
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment,
            ITaskResolver taskResolver,
            ITaskOrderer taskOrderer,
            IContainerConfiguration containerConfiguration)
        {
            if (taskResolver == null)
            {
                throw new ArgumentNullException(nameof(taskResolver));
            }

            if (taskOrderer == null)
            {
                throw new ArgumentNullException(nameof(taskOrderer));
            }

            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.hostingEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.startupHelper = new AutofacBootStartupHelper(taskResolver, taskOrderer);
            this.Configuration = this.startupHelper.GetConfiguration(environment);
            this.containerConfiguration = containerConfiguration ?? throw new ArgumentNullException(nameof(containerConfiguration));
        }

        /// <summary>
        /// Gets the application container.
        /// </summary>
        /// <value>
        /// The application container.
        /// </value>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfigurationRoot Configuration { get; }

        /// <inheritdoc />
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                this.startupHelper.ConfigureServices(services);

                var builder = new ContainerBuilder();
                builder.Populate(services);

                this.startupHelper.ConfigureContainer(builder, this.Configuration);

                this.containerConfiguration.Configure(this.hostingEnvironment, builder);

                this.ApplicationContainer = builder.Build();
                return new AutofacServiceProvider(this.ApplicationContainer);
            }
            catch (Exception exception)
            {
                throw new AutofacBootException(this.loggerFactory, exception);
            }
        }

        /// <inheritdoc />
        public void Configure(IApplicationBuilder app)
        {
            try
            {
                var applicationTasks = app.ApplicationServices
                    .GetServices(typeof(IApplicationBootstrapTask))
                    .Cast<IApplicationBootstrapTask>();

                this.startupHelper.Configure(app, applicationTasks);

                var appLifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;
                appLifetime?.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
            }
            catch (Exception exception)
            {
                throw new AutofacBootException(this.loggerFactory, exception);
            }
        }
    }
}