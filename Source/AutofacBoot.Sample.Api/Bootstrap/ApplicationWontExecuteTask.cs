// <copyright file="ApplicationWontExecuteTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Application won't execute task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IApplicationBootstrapTask" />
    /// <seealso cref="AutofacBoot.IConditionalExecution" />
    public class ApplicationWontExecuteTask : IApplicationBootstrapTask, IConditionalExecution
    {
        /// <inheritdoc />
        public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public Task Execute(IApplicationBuilder app)
        {
            throw new System.NotImplementedException();
        }
    }
}