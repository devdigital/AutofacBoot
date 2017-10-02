using System.Threading.Tasks;
using Autofac;

namespace AutofacBoot
{
    public class NullContainerConfiguration : IContainerConfiguration
    {
        public Task Configure(ContainerBuilder builder)
        {
            return Task.FromResult(0);
        }
    }
}