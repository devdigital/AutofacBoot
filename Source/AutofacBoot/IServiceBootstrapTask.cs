using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public interface IServiceBootstrapTask
    {
        Task Execute(
            IHostingEnvironment environment,
            IConfigurationRoot configuration,
            IServiceCollection services);
    }
}