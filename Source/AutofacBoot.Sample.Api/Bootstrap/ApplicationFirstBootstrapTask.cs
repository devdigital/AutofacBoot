// <copyright file="ApplicationFirstBootstrapTask.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Sample.Api.Bootstrap
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Application first bootstrap task.
    /// </summary>
    /// <seealso cref="AutofacBoot.IApplicationBootstrapTask" />
    /// <seealso cref="AutofacBoot.IOrderedTask" />
    public class ApplicationFirstBootstrapTask : IApplicationBootstrapTask, IOrderedTask
    {
        /// <inheritdoc />
        public int Order => -10;

        /// <inheritdoc />
        public Task Execute(IApplicationBuilder app)
        {
            return Task.CompletedTask;
        }
    }
}