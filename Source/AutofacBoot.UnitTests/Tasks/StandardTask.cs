using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.UnitTests.Tasks
{
    public class StandardTask : IConfigurationBootstrapTask
    {
        public Task Execute(ConfigurationBuilder configurationBuilder, IHostingEnvironment environment)
        {
            throw new NotImplementedException();
        }
    }
}