// <copyright file="NullAppBuilderConfigurationResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    /// <summary>
    /// Null application builder configuration resolver.
    /// </summary>
    /// <seealso cref="AutofacBoot.IAppBuilderConfigurationResolver" />
    public class NullAppBuilderConfigurationResolver : IAppBuilderConfigurationResolver
    {
        /// <inheritdoc />
        public IAppBuilderConfiguration Resolve(string id)
        {
            return new NullAppBuilderConfiguration();
        }
    }
}