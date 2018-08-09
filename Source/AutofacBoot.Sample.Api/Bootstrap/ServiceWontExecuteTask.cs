// <copyright file="ServiceWontExecuteTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Service won't execute task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IServiceBootstrapTask" />
    /// <seealso cref="AutofacBoot.IConditionalExecution" />
    public class ServiceWontExecuteTask : IServiceBootstrapTask, IConditionalExecution
    {
        /// <inheritdoc />
        public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public Task Execute(
            IHostingEnvironment environment,
            IConfigurationRoot configuration,
            IServiceCollection services)
        {
            throw new System.NotImplementedException();
        }
    }
}