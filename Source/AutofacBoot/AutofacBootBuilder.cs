using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal class AutofacBootBuilder : IAutofacBootBuilder
    {
        private string[] arguments;

        private IAutofacBootTaskResolver taskResolver;

        private IContainerConfiguration containerConfiguration;

        private Action<Exception, ILoggerFactory> exceptionHandler;

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

        public AutofacBootBuilder(Action<Exception, ILoggerFactory> exceptionHandler)
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

        public IAutofacBootBuilder WithExceptionHandler(Action<Exception, ILoggerFactory> exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
            return this;
        }

        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                this.arguments,
                this.taskResolver,
                this.containerConfiguration);
        }

        public void Run()
        {
            this.RunAsync().GetAwaiter().GetResult();
        }

        public async Task RunAsync()
        {
            try
            {
                var host = this.Configure().Build();
                await host.RunAsync();
            }
            catch (AutofacBootException exception)
            {
                if (this.exceptionHandler == null)
                {
                    throw;
                }

                this.exceptionHandler(
                    exception.InnerException, exception.LoggerFactory);
            }         
        }
    }
}