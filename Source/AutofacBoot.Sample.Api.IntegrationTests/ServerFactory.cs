using System.Net;
using AutofacBoot.Sample.Api.Bootstrap;
using AutofacBoot.Test;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    public class ServerFactory : TestServerFactory<ServerFactory>
    {
        protected override IAutofacBootTaskResolver GetTaskResolver()
        {
            return new AssemblyTaskResolver(
                typeof(ServiceBootstrapTask).Assembly);
        }

        protected override IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder.UseKestrel(options =>
            {
                options.Listen(IPAddress.Any, 443, listenOptions =>
                {
                    listenOptions.UseHttps("server.pfx");
                });
            })
            .UseUrls("https://*:4430");
        }
    }
}