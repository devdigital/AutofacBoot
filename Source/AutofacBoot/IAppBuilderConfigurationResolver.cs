// <copyright file="IAppBuilderConfigurationResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    /// <summary>
    /// App builder configuration resolver.
    /// </summary>
    public interface IAppBuilderConfigurationResolver
    {
        /// <summary>
        /// Resolves the specified app builder configuration.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The app builder configuration.</returns>
        IAppBuilderConfiguration Resolve(string id);
    }
}