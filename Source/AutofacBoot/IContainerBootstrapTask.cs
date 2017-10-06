using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    public interface IContainerBootstrapTask
    {
        Task Execute(
            IConfigurationRoot configuration,
            ContainerBuilder builder);
    }
}
