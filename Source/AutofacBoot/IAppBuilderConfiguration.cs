// <copyright file="IAppBuilderConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// App builder configuration.
    /// </summary>
    public interface IAppBuilderConfiguration
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The task.</returns>
        Task Configure(IApplicationBuilder app);
    }
}