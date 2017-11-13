using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    public class AutofacBootstrapper
    {
        public IAutofacBootBuilder WithArguments(string[] arguments)
        {
            return new AutofacBootBuilder(arguments);
        }

        public IAutofacBootBuilder WithTasks(IAutofacBootTaskResolver taskResolver)
        {
            return new AutofacBootBuilder(taskResolver);
        }

        public IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration)
        {
            return new AutofacBootBuilder(containerConfiguration);
        }

        public IAutofacBootBuilder WithExceptionHandler(Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            return new AutofacBootBuilder(exceptionHandler);
        }

        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                arguments: null,
                taskResolver: null,
                containerConfiguration: null,
                exceptionHandler: null);
        }

        public void Run()
        {
            this.RunAsync().GetAwaiter().GetResult();
        }

        public async Task RunAsync()
        {
            var hostBuilder = this.Configure();
            await hostBuilder.Build().RunAsync();
        }
    }
}