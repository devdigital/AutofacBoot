using System.Threading.Tasks;
using Autofac;

namespace AutofacBoot
{
    public interface IContainerBootstrapTask
    {
        Task Execute(ContainerBuilder builder);
    }
}
