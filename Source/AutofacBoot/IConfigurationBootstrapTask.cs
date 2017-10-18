using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    public interface IConfigurationBootstrapTask
    {
        Task Execute(
            IHostingEnvironment environment,
            ConfigurationBuilder configurationBuilder);
    }
}