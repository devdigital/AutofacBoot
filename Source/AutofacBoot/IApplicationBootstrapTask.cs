// <copyright file="IApplicationBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Application bootstrap task.
    /// </summary>
    public interface IApplicationBootstrapTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>The task.</returns>
        Task Execute(IApplicationBuilder app);
    }
}