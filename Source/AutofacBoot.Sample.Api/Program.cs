using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacBoot.Sample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new AutofacBootstrapper()
                .WithExceptionHandler((exception, loggerFactory) =>
                {
                    var logger = loggerFactory.CreateLogger("Program");
                    logger.LogError(exception, "There was an error during bootstrapping.");
                    return false;
                })
                .Configure()
                .Build()
                .Run();
        }
    }
}
