// <copyright file="NullAppBuilderConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Null application builder configuration.
    /// </summary>
    /// <seealso cref="AutofacBoot.IAppBuilderConfiguration" />
    public class NullAppBuilderConfiguration : IAppBuilderConfiguration
    {
        /// <inheritdoc />
        public Task Configure(IApplicationBuilder builder)
        {
            return Task.CompletedTask;
        }
    }
}