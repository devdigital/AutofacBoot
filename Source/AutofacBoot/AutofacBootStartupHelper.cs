// <copyright file="AutofacBootStartupHelper.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// AutofacBoot startup helper.
    /// </summary>
    internal class AutofacBootStartupHelper
    {
        private readonly ITaskResolver currentTaskResolver;

        private readonly ITaskOrderer currentTaskOrderer;

        private IHostingEnvironment currentHostingEnvironment;

        private IConfigurationRoot currentConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootStartupHelper"/> class.
        /// </summary>
        /// <param name="taskResolver">The task resolver.</param>
        /// <param name="taskOrderer">The task orderer.</param>
        public AutofacBootStartupHelper(ITaskResolver taskResolver, ITaskOrderer taskOrderer)
        {
            this.currentTaskResolver = new OrderedTaskResolver(taskResolver, taskOrderer);
            this.currentTaskOrderer = taskOrderer ?? throw new ArgumentNullException(nameof(taskOrderer));
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The configuration.</returns>
        public IConfigurationRoot GetConfiguration(IHostingEnvironment environment)
        {
            return this.GetConfigurationAsync(environment)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Gets the configuration asynchronously.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The configuration.</returns>
        public async Task<IConfigurationRoot> GetConfigurationAsync(IHostingEnvironment environment)
        {
            this.currentHostingEnvironment = environment;
            var configurationTasks = await this.currentTaskResolver.GetConfigurationTasks();

            var configurationBuilder = new ConfigurationBuilder();
            foreach (var configurationTask in configurationTasks)
            {
                await configurationTask.Execute(
                    this.currentHostingEnvironment,
                    configurationBuilder);
            }

            this.currentConfiguration = configurationBuilder.Build();
            return this.currentConfiguration;
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureServicesAsync(services).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Configures the services asynchronously.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The task.</returns>
        public async Task ConfigureServicesAsync(IServiceCollection services)
        {
            var serviceTasks = await this.currentTaskResolver.GetServiceTasks();

            foreach (var serviceTask in serviceTasks)
            {
                if (serviceTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(
                        this.currentHostingEnvironment,
                        this.currentConfiguration);

                    if (canExecute)
                    {
                        await serviceTask.Execute(
                            this.currentHostingEnvironment,
                            this.currentConfiguration,
                            services);
                    }

                    continue;
                }

                await serviceTask.Execute(
                    this.currentHostingEnvironment,
                    this.currentConfiguration,
                    services);
            }
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        public void ConfigureContainer(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            this.ConfigureContainerAsync(builder, configuration).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Configures the container asynchronously.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The task.</returns>
        public async Task ConfigureContainerAsync(ContainerBuilder builder, IConfigurationRoot configuration)
        {
            builder.RegisterInstance(configuration);

            var containerTasks = await this.currentTaskResolver.GetContainerTasks();

            foreach (var containerTask in containerTasks)
            {
                if (containerTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(
                        this.currentHostingEnvironment,
                        this.currentConfiguration);

                    if (canExecute)
                    {
                        await containerTask.Execute(
                            this.currentHostingEnvironment,
                            this.currentConfiguration,
                            builder);
                    }

                    continue;
                }

                await containerTask.Execute(
                    this.currentHostingEnvironment,
                    this.currentConfiguration,
                    builder);
            }

            var bootstrapTaskTypes = await this.currentTaskResolver.GetApplicationTaskTypes();
            foreach (var bootstrapTaskType in bootstrapTaskTypes)
            {
                builder.RegisterType(bootstrapTaskType).As<IApplicationBootstrapTask>();
            }
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="bootstrapTasks">The bootstrap tasks.</param>
        public void Configure(IApplicationBuilder app, IEnumerable<IApplicationBootstrapTask> bootstrapTasks)
        {
            this.ConfigureAsync(app, bootstrapTasks).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Configures the specified application asynchronously.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="applicationTasks">The application tasks.</param>
        /// <returns>The task.</returns>
        public async Task ConfigureAsync(IApplicationBuilder app, IEnumerable<IApplicationBootstrapTask> applicationTasks)
        {
            var orderedTasks = await this.currentTaskOrderer.Order(applicationTasks);

            foreach (var applicationTask in orderedTasks)
            {
                if (applicationTask is IConditionalExecution conditional)
                {
                    var canExecute = await conditional.CanExecute(
                        this.currentHostingEnvironment,
                        this.currentConfiguration);

                    if (canExecute)
                    {
                        await applicationTask.Execute(app);
                    }

                    continue;
                }

                await applicationTask.Execute(app);
            }
        }
    }
}