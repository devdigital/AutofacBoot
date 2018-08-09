// <copyright file="ServiceBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using AutofacBoot.Sample.Api.Controllers;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Service bootstrap task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IServiceBootstrapTask" />
    public class ServiceBootstrapTask : IServiceBootstrapTask
    {
        /// <inheritdoc />
        public Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(
                typeof(ValuesController).Assembly);

            return Task.CompletedTask;
        }
    }
}