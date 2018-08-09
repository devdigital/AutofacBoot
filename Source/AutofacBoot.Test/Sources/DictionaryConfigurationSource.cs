// <copyright file="DictionaryConfigurationSource.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Test.Sources
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Dictionary configuration source.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.IConfigurationSource" />
    internal class DictionaryConfigurationSource : IConfigurationSource
    {
        private readonly IDictionary<string, string> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConfigurationSource"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DictionaryConfigurationSource(IDictionary<string, string> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DictionaryConfigurationProvider(this.configuration);
        }
    }
}