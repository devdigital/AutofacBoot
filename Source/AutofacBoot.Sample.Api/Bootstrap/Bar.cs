using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class Bar : IBootstrapTask
    {        
        private readonly ILoggerFactory loggerFactory;

        private readonly IConfigurationRoot configuration;

        public Bar(ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        public Task Execute(IApplicationBuilder app)
        {
            this.loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
            this.loggerFactory.AddDebug();

            app.UseMvc();

            return Task.FromResult(0);
        }
    }
}