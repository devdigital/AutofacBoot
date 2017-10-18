using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    public interface IContainerConfiguration
    {
        Task Configure(
            IHostingEnvironment environment,
            ContainerBuilder builder);
    }
}