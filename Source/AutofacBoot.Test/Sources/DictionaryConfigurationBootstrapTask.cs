using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Test.Sources
{
    internal class DictionaryConfigurationBootstrapTask : IConfigurationBootstrapTask
    {
        private readonly IDictionary<string, string> configuration;

        public DictionaryConfigurationBootstrapTask(IDictionary<string, string> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new DictionaryConfigurationSource(this.configuration));
            return Task.FromResult(0);
        }
    }
}