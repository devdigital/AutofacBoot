using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    public interface IContainerBootstrapTask
    {
        Task Execute(
            IHostingEnvironment environment,
            IConfigurationRoot configuration,
            ContainerBuilder builder);
    }
}
