using System.Threading.Tasks;
using Autofac;

namespace AutofacBoot
{
    public interface IContainerConfiguration
    {
        Task Configure(ContainerBuilder builder);
    }
}