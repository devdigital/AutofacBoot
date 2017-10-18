using System.Threading.Tasks;
using Autofac;
using AutofacBoot.Sample.Data.InMemory;
using AutofacBoot.Sample.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ContainerBootstrapTask : IContainerBootstrapTask
    {
        public Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryValuesRepository>().As<IValuesRepository>();
            return Task.FromResult(0);
        }
    }
}
