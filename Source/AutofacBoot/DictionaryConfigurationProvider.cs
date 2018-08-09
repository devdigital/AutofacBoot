// <copyright file="DictionaryConfigurationProvider.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Dictionary configuration provider.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.ConfigurationProvider" />
    internal class DictionaryConfigurationProvider : ConfigurationProvider
    {
        private readonly IDictionary<string, string> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConfigurationProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DictionaryConfigurationProvider(IDictionary<string, string> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public override void Load()
        {
            foreach (var kvp in this.configuration)
            {
                this.Data[kvp.Key] = kvp.Value;
            }
        }
    }
}