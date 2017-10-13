using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutofacBoot.UnitTests.Tasks
{
    public class TestApplicationTask : IApplicationBootstrapTask
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly IConfigurationRoot configuration;

        public TestApplicationTask(
            ILoggerFactory loggerFactory,
            IConfigurationRoot configuration)
        {
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        public Task Execute(IApplicationBuilder app)
        {
            throw new NotImplementedException();            
        }
    }
}