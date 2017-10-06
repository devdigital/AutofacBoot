using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public interface IServiceBootstrapTask
    {
        Task Execute(
            IConfigurationRoot configuration,
            IServiceCollection services);
    }
}