using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal class AutofacBootBuilder : IAutofacBootBuilder
    {
        private string[] arguments;

        private IAutofacBootTaskResolver taskResolver;

        private IContainerConfiguration containerConfiguration;

        private Func<Exception, ILoggerFactory, bool> exceptionHandler;

        public AutofacBootBuilder(string[] arguments)
        {
            this.WithArguments(arguments);
        }

        public AutofacBootBuilder(IAutofacBootTaskResolver taskResolver)
        {
            this.WithTasks(taskResolver);
        }

        public AutofacBootBuilder(IContainerConfiguration containerConfiguration)
        {
            this.WithContainer(containerConfiguration);
        }

        public AutofacBootBuilder(Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.WithExceptionHandler(exceptionHandler);
        }

        public IAutofacBootBuilder WithArguments(string[] arguments)
        {
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            return this;
        }

        public IAutofacBootBuilder WithTasks(IAutofacBootTaskResolver taskResolver)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            return this;
        }

        public IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration)
        {
            this.containerConfiguration = containerConfiguration ?? throw new ArgumentNullException(nameof(containerConfiguration));
            return this;
        }

        public IAutofacBootBuilder WithExceptionHandler(Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
            return this;
        }

        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                this.arguments,
                this.taskResolver,
                this.containerConfiguration,
                this.exceptionHandler);
        }

        public IWebHostRunner Build()
        {
            var host = this.Configure().Build();
            return host == null ? null : new WebHostRunner(host);
        }
    }
}