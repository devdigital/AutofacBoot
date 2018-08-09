// <copyright file="DictionaryConfigurationBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Test.Sources
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Dictionary configuration bootstrap task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IConfigurationBootstrapTask" />
    internal class DictionaryConfigurationBootstrapTask : IConfigurationBootstrapTask
    {
        private readonly IDictionary<string, string> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryConfigurationBootstrapTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public DictionaryConfigurationBootstrapTask(IDictionary<string, string> configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public Task Execute(IHostingEnvironment environment, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new DictionaryConfigurationSource(this.configuration));
            return Task.FromResult(0);
        }
    }
}