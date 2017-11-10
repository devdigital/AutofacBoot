using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    public interface IAutofacBootBuilder
    {
        IAutofacBootBuilder WithArguments(string[] arguments);

        IAutofacBootBuilder WithTasks(IAutofacBootTaskResolver taskResolver);

        IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration);

        IAutofacBootBuilder WithExceptionHandler(Action<Exception, ILoggerFactory> exceptionHandler);

        IWebHostBuilder Configure();

        void Run();

        Task RunAsync();
    }
}