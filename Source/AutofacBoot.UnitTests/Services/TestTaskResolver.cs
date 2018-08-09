// <copyright file="TestTaskResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutofacBoot.UnitTests.Tasks;

    /// <summary>
    /// Test task resolver.
    /// </summary>
    /// <seealso cref="AutofacBoot.ITaskResolver" />
    public class TestTaskResolver : ITaskResolver
    {
        /// <inheritdoc />
        public Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var tasks = new List<IConfigurationBootstrapTask>
            {
                new TestConfigurationBootstrapTask(),
            };

            return Task.FromResult(tasks.AsEnumerable());
        }

        /// <inheritdoc />
        public Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            return Task.FromResult(Enumerable.Empty<IServiceBootstrapTask>());
        }

        /// <inheritdoc />
        public Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            return Task.FromResult(Enumerable.Empty<IContainerBootstrapTask>());
        }

        /// <inheritdoc />
        public Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            var tasks = new List<Type>
            {
                typeof(TestApplicationTask),
            };

            return Task.FromResult(tasks.AsEnumerable());
        }
    }
}