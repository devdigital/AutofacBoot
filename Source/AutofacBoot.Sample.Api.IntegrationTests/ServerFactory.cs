using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutofacBoot.Sample.Api.Bootstrap;
using AutofacBoot.Test;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    public class ServerFactory : TestServerFactory<ServerFactory>
    {
        public bool ConfigurationInvoked { get; private set; }

        protected override Task<ITaskResolver> GetTaskResolver()
        {
            ITaskResolver taskResolver = new AssemblyTaskResolver(
                typeof(ServiceBootstrapTask).Assembly);

            return Task.FromResult(taskResolver);
        }

        protected override IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder.UseKestrel(options =>
            {
                options.Listen(IPAddress.Any, 443, listenOptions =>
                {
                    listenOptions.UseHttps("server.pfx");
                });
            });
        }

        protected override Task<IDictionary<string, string>> GetConfiguration()
        {
            IDictionary<string, string> configuration = new Dictionary<string, string>
            {
                { "Test", "TestValue" }
            };

            this.ConfigurationInvoked = true;
            return Task.FromResult(configuration);
        }        
    }
}