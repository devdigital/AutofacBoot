using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ServiceBootstrapTask : IServiceBootstrapTask
    {
        public Task Execute(IServiceCollection services)
        {
            services.AddMvc();
            return Task.FromResult(0);
        }
    }
}