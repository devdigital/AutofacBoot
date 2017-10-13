using System;
using System.Threading.Tasks;
using Autofac;

namespace AutofacBoot.UnitTests.Services
{
    public class ExceptionThrowingContainerConfiguration : IContainerConfiguration
    {
        public Task Configure(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}