using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal class AutofacBootBuilder : IAutofacBootBuilder
    {
        private string[] arguments;

        private ITaskResolver taskResolver;

        private ITaskOrderer taskOrderer;

        private IContainerConfiguration containerConfiguration;

        private Func<Exception, ILoggerFactory, bool> exceptionHandler;
        
        public AutofacBootBuilder(string[] arguments)
        {
            this.WithArguments(arguments);
        }

        public AutofacBootBuilder(ITaskResolver taskResolver)
        {
            this.WithTasks(taskResolver);
        }

        public AutofacBootBuilder(ITaskOrderer taskOrderer)
        {
            this.WithOrder(taskOrderer);
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

        public IAutofacBootBuilder WithTasks(ITaskResolver taskResolver)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            return this;
        }

        public IAutofacBootBuilder WithOrder(ITaskOrderer taskOrderer)
        {
            this.taskOrderer = taskOrderer ?? throw new ArgumentNullException(nameof(taskOrderer));
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
                this.taskOrderer,
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