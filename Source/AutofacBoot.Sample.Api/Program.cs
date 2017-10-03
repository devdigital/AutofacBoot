using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot.Sample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new AutofacBootstrapper()
                .Configure()
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 443, listenOptions =>
                    {
                        listenOptions.UseHttps("server.pfx");
                    });
                })
                .UseUrls("https://*:4430")
                .Build()
                .Run();
        }
    }
}
