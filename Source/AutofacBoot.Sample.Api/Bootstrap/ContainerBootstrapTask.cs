// <copyright file="ContainerBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using Autofac;
    using AutofacBoot.Sample.Data.InMemory;
    using AutofacBoot.Sample.Domain;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Container bootstrap task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IContainerBootstrapTask" />
    public class ContainerBootstrapTask : IContainerBootstrapTask
    {
        /// <inheritdoc />
        public Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryValuesRepository>().As<IValuesRepository>();
            return Task.CompletedTask;
        }
    }
}
