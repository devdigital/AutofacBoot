// <copyright file="AutofacBootstrapper.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutofacBootstrapper.
    /// </summary>
    public class AutofacBootstrapper
    {
        /// <summary>
        /// Adds the arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The builder.</returns>
        public IAutofacBootBuilder WithArguments(string[] arguments)
        {
            return new AutofacBootBuilder(arguments);
        }

        /// <summary>
        /// Adds the tasks.
        /// </summary>
        /// <param name="taskResolver">The task resolver.</param>
        /// <returns>The builder.</returns>
        public IAutofacBootBuilder WithTasks(ITaskResolver taskResolver)
        {
            return new AutofacBootBuilder(taskResolver);
        }

        /// <summary>
        /// Adds the order.
        /// </summary>
        /// <param name="taskOrderer">The task orderer.</param>
        /// <returns>The builder.</returns>
        public IAutofacBootBuilder WithOrder(ITaskOrderer taskOrderer)
        {
            return new AutofacBootBuilder(taskOrderer);
        }

        /// <summary>
        /// Adds the container.
        /// </summary>
        /// <param name="containerConfiguration">The container configuration.</param>
        /// <returns>The builder.</returns>
        public IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration)
        {
            return new AutofacBootBuilder(containerConfiguration);
        }

        /// <summary>
        /// Adds the exception handler.
        /// </summary>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns>The builder.</returns>
        public IAutofacBootBuilder WithExceptionHandler(Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            return new AutofacBootBuilder(exceptionHandler);
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        /// <returns>The web host builder.</returns>
        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                arguments: null,
                taskResolver: null,
                taskOrderer: null,
                containerConfiguration: null,
                exceptionHandler: null);
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            this.RunAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs this instance asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        public async Task RunAsync()
        {
            var hostBuilder = this.Configure();
            await hostBuilder.Build().RunAsync();
        }
    }
}