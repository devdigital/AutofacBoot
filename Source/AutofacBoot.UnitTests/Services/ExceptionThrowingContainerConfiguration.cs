using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot.UnitTests.Services
{
    public class ExceptionThrowingContainerConfiguration : IContainerConfiguration
    {
        public Task Configure(IHostingEnvironment environment, ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}