// <copyright file="IAutofacBootBuilder.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutofacBoot builder.
    /// </summary>
    public interface IAutofacBootBuilder
    {
        /// <summary>
        /// Adds the arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The builder.</returns>
        IAutofacBootBuilder WithArguments(string[] arguments);

        /// <summary>
        /// Adds the tasks.
        /// </summary>
        /// <param name="taskResolver">The task resolver.</param>
        /// <returns>The builder.</returns>
        IAutofacBootBuilder WithTasks(ITaskResolver taskResolver);

        /// <summary>
        /// Adds the order.
        /// </summary>
        /// <param name="taskOrderer">The task orderer.</param>
        /// <returns>The builder.</returns>
        IAutofacBootBuilder WithOrder(ITaskOrderer taskOrderer);

        /// <summary>
        /// Adds the container.
        /// </summary>
        /// <param name="containerConfiguration">The container configuration.</param>
        /// <returns>The builder.</returns>
        IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration);

        /// <summary>
        /// Adds the application builder configurations.
        /// </summary>
        /// <param name="appBuilderConfigurations">The application builder configurations.</param>
        /// <returns>The builder.</returns>
        IAutofacBootBuilder WithAppBuilderConfigurations(IDictionary<string, IAppBuilderConfiguration> appBuilderConfigurations);

        /// <summary>
        /// Adds the exception handler.
        /// </summary>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>The builder.</returns>
        IAutofacBootBuilder WithExceptionHandler(Func<Exception, ILoggerFactory, bool> exceptionHandler);

        /// <summary>
        /// Configures this instance.
        /// </summary>
        /// <returns>The web host builder.</returns>
        IWebHostBuilder Configure();

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>The web host runner.</returns>
        IWebHostRunner Build();
    }
}