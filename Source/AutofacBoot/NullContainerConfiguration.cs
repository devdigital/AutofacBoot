using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    public class NullContainerConfiguration : IContainerConfiguration
    {
        public Task Configure(IHostingEnvironment environment, ContainerBuilder builder)
        {
            return Task.CompletedTask;
        }
    }
}