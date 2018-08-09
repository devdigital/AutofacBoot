// <copyright file="AutofacBootBuilder.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutofacBoot builder.
    /// </summary>
    /// <seealso cref="AutofacBoot.IAutofacBootBuilder" />
    internal class AutofacBootBuilder : IAutofacBootBuilder
    {
        private string[] currentArguments;

        private ITaskResolver currentTaskResolver;

        private ITaskOrderer currentTaskOrderer;

        private IContainerConfiguration currentContainerConfiguration;

        private Func<Exception, ILoggerFactory, bool> currentExceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootBuilder"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public AutofacBootBuilder(string[] arguments)
        {
            this.WithArguments(arguments);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootBuilder"/> class.
        /// </summary>
        /// <param name="taskResolver">The task resolver.</param>
        public AutofacBootBuilder(ITaskResolver taskResolver)
        {
            this.WithTasks(taskResolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootBuilder"/> class.
        /// </summary>
        /// <param name="taskOrderer">The task orderer.</param>
        public AutofacBootBuilder(ITaskOrderer taskOrderer)
        {
            this.WithOrder(taskOrderer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootBuilder"/> class.
        /// </summary>
        /// <param name="containerConfiguration">The container configuration.</param>
        public AutofacBootBuilder(IContainerConfiguration containerConfiguration)
        {
            this.WithContainer(containerConfiguration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootBuilder"/> class.
        /// </summary>
        /// <param name="exceptionHandler">The exception handler.</param>
        public AutofacBootBuilder(Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.WithExceptionHandler(exceptionHandler);
        }

        /// <inheritdoc />
        public IAutofacBootBuilder WithArguments(string[] arguments)
        {
            this.currentArguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            return this;
        }

        /// <inheritdoc />
        public IAutofacBootBuilder WithTasks(ITaskResolver taskResolver)
        {
            this.currentTaskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            return this;
        }

        /// <inheritdoc />
        public IAutofacBootBuilder WithOrder(ITaskOrderer taskOrderer)
        {
            this.currentTaskOrderer = taskOrderer ?? throw new ArgumentNullException(nameof(taskOrderer));
            return this;
        }

        /// <inheritdoc />
        public IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration)
        {
            this.currentContainerConfiguration = containerConfiguration ?? throw new ArgumentNullException(nameof(containerConfiguration));
            return this;
        }

        /// <inheritdoc />
        public IAutofacBootBuilder WithExceptionHandler(Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.currentExceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
            return this;
        }

        /// <inheritdoc />
        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                this.currentArguments,
                this.currentTaskResolver,
                this.currentTaskOrderer,
                this.currentContainerConfiguration,
                this.currentExceptionHandler);
        }

        /// <inheritdoc />
        public IWebHostRunner Build()
        {
            var host = this.Configure().Build();
            return host == null ? null : new WebHostRunner(host);
        }
    }
}