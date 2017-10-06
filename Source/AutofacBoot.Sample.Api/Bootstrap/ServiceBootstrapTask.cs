using System.Threading.Tasks;
using AutofacBoot.Sample.Api.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ServiceBootstrapTask : IServiceBootstrapTask
    {
        public Task Execute(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(
                typeof(ValuesController).Assembly);

            return Task.FromResult(0);
        }
    }
}