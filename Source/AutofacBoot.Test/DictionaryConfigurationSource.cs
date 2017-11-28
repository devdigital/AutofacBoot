using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Test
{
    internal class DictionaryConfigurationSource : IConfigurationSource
    {
        private readonly IDictionary<string, string> configuration;

        public DictionaryConfigurationSource(IDictionary<string, string> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DictionaryConfigurationProvider(this.configuration);
        }
    }
}