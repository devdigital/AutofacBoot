// <copyright file="IConditionalExecution.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Conditional execution.
    /// </summary>
    public interface IConditionalExecution
    {
        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="configurationRoot">The configuration root.</param>
        /// <returns>True if the task can execute; false otherwise.</returns>
        Task<bool> CanExecute(
            IHostingEnvironment environment,
            IConfigurationRoot configurationRoot);
    }
}