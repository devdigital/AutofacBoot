// <copyright file="AdditionalConfigurationTaskResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Additional configuration task resolver.
    /// </summary>
    /// <seealso cref="AutofacBoot.ITaskResolver" />
    internal class AdditionalConfigurationTaskResolver : ITaskResolver
    {
        private readonly ITaskResolver taskResolver;

        private readonly IDictionary<string, string> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalConfigurationTaskResolver"/> class.
        /// </summary>
        /// <param name="taskResolver">The task resolver.</param>
        /// <param name="configuration">The configuration.</param>
        public AdditionalConfigurationTaskResolver(ITaskResolver taskResolver, IDictionary<string, string> configuration)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            this.configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var tasks = await this.taskResolver.GetConfigurationTasks();
            return this.configuration == null
                ? tasks
                : tasks.Concat(new[] { new DictionaryConfigurationBootstrapTask(this.configuration) });
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            return await this.taskResolver.GetServiceTasks();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            return await this.taskResolver.GetContainerTasks();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            return await this.taskResolver.GetApplicationTaskTypes();
        }
    }
}