// <copyright file="OrderedTaskResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Ordered task resolver.
    /// </summary>
    /// <seealso cref="AutofacBoot.ITaskResolver" />
    internal class OrderedTaskResolver : ITaskResolver
    {
        private readonly ITaskResolver taskResolver;

        private readonly ITaskOrderer taskOrderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedTaskResolver"/> class.
        /// </summary>
        /// <param name="taskResolver">The task resolver.</param>
        /// <param name="taskOrderer">The task orderer.</param>
        public OrderedTaskResolver(ITaskResolver taskResolver, ITaskOrderer taskOrderer)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            this.taskOrderer = taskOrderer ?? throw new ArgumentNullException(nameof(taskOrderer));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var tasks = await this.taskResolver.GetConfigurationTasks();
            return await this.taskOrderer.Order(tasks);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            var tasks = await this.taskResolver.GetServiceTasks();
            return await this.taskOrderer.Order(tasks);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            var tasks = await this.taskResolver.GetContainerTasks();
            return await this.taskOrderer.Order(tasks);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            return await this.taskResolver.GetApplicationTaskTypes();
        }
    }
}