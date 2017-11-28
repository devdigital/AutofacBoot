using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    internal class DictionaryConfigurationProvider : ConfigurationProvider
    {
        private readonly IDictionary<string, string> configuration;

        public DictionaryConfigurationProvider(IDictionary<string, string> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public override void Load()
        {
            foreach (var kvp in this.configuration)
            {
                this.Data[kvp.Key] = kvp.Value;
            }
        }
    }
}