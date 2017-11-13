using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    public interface IAutofacBootBuilder
    {
        IAutofacBootBuilder WithArguments(string[] arguments);

        IAutofacBootBuilder WithTasks(ITaskResolver taskResolver);

        IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration);

        IAutofacBootBuilder WithExceptionHandler(Func<Exception, ILoggerFactory, bool> exceptionHandler);

        IWebHostBuilder Configure();

        IWebHostRunner Build();
    }
}