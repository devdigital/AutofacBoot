// <copyright file="ContainerWontExecuteTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Container won't execute task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IContainerBootstrapTask" />
    /// <seealso cref="AutofacBoot.IConditionalExecution" />
    public class ContainerWontExecuteTask : IContainerBootstrapTask, IConditionalExecution
    {
        /// <inheritdoc />
        public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, ContainerBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}