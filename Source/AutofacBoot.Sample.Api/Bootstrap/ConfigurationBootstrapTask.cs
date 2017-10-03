using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ConfigurationBootstrapTask : IConfigurationBootstrapTask
    {
        public Task Execute(ConfigurationBuilder configurationBuilder, IHostingEnvironment environment)
        {
            throw new NotImplementedException();
            configurationBuilder
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            return Task.FromResult(0);
        }
    }
}