using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.UnitTests.Tasks
{
    public class HighNumberedTask : IConfigurationBootstrapTask, IOrderedTask
    {
        public Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)
        {
            throw new NotImplementedException();
        }

        public int Order => 10;
    }
}