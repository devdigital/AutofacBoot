// <copyright file="AssemblyTaskResolver.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Assembly task resolver.
    /// </summary>
    /// <seealso cref="AutofacBoot.ITaskResolver" />
    public class AssemblyTaskResolver : ITaskResolver
    {
        private readonly IEnumerable<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyTaskResolver"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public AssemblyTaskResolver(Assembly assembly)
            : this(new List<Assembly> { assembly })
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyTaskResolver"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public AssemblyTaskResolver(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static AssemblyTaskResolver Default => new AssemblyTaskResolver(
            Assembly.GetEntryAssembly());

        /// <inheritdoc />
        public Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var configurationTaskTypes = this.ScanAssembliesForTypesImplementing<IConfigurationBootstrapTask>();
            var instances = configurationTaskTypes
                .Where(t => typeof(IConfigurationBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IConfigurationBootstrapTask)Activator.CreateInstance(t))
                .ToList();

            return Task.FromResult(instances.AsEnumerable());
        }

        /// <inheritdoc />
        public Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            var serviceTaskTypes = this.ScanAssembliesForTypesImplementing<IServiceBootstrapTask>();
            var instances = serviceTaskTypes
                .Where(t => typeof(IServiceBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IServiceBootstrapTask)Activator.CreateInstance(t))
                .ToList();

            return Task.FromResult(instances.AsEnumerable());
        }

        /// <inheritdoc />
        public Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            var containerTaskTypes = this.ScanAssembliesForTypesImplementing<IContainerBootstrapTask>();
            var instances = containerTaskTypes
                .Where(t => typeof(IContainerBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IContainerBootstrapTask)Activator.CreateInstance(t))
                .ToList();

            return Task.FromResult(instances.AsEnumerable());
        }

        /// <inheritdoc />
        public Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            var types = this.ScanAssembliesForTypesImplementing<IApplicationBootstrapTask>();
            return Task.FromResult(types);
        }

        private IEnumerable<Type> ScanAssembliesForTypesImplementing<T>()
        {
            return this.assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => !t.IsInterface && typeof(T).IsAssignableFrom(t));
        }
    }
}