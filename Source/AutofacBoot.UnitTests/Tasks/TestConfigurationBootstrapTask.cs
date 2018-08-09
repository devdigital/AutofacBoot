using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.UnitTests.Tasks
{
    public class TestConfigurationBootstrapTask : IConfigurationBootstrapTask
    {
        public Task Execute(
            IHostingEnvironment environment, 
            IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            return Task.FromResult(0);
        }
    }
}