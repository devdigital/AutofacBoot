using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class Foo : IContainerBootstrapTask
    {
        public Task Execute(ContainerBuilder builder)
        {
            return Task.FromResult(0);
        }
    }
}
