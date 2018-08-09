// <copyright file="DefaultAppBuilderConfigurationResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Default app builder configuration resolver.
    /// </summary>
    /// <seealso cref="AutofacBoot.IAppBuilderConfigurationResolver" />
    internal class DefaultAppBuilderConfigurationResolver : IAppBuilderConfigurationResolver
    {
        private readonly IDictionary<string, IAppBuilderConfiguration> appBuilderConfigurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAppBuilderConfigurationResolver"/> class.
        /// </summary>
        /// <param name="appBuilderConfigurations">The application builder configurations.</param>
        public DefaultAppBuilderConfigurationResolver(IDictionary<string, IAppBuilderConfiguration> appBuilderConfigurations)
        {
            this.appBuilderConfigurations = appBuilderConfigurations ??
                throw new ArgumentNullException(nameof(appBuilderConfigurations));
        }

        /// <inheritdoc />
        public IAppBuilderConfiguration Resolve(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!this.appBuilderConfigurations.ContainsKey(id))
            {
                throw new InvalidOperationException(
                    $"Application builder configuration with identifier '{id}' not registered.");
            }

            return this.appBuilderConfigurations[id];
        }
    }
}