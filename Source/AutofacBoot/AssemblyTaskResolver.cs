using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public class AssemblyTaskResolver : IAutofacBootTaskResolver
    {
        private readonly IEnumerable<Assembly> assemblies;

        public AssemblyTaskResolver(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            this.assemblies = new List<Assembly> { assembly };
        }

        public AssemblyTaskResolver(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        public static AssemblyTaskResolver Default => new AssemblyTaskResolver(
            Assembly.GetExecutingAssembly());    

        public Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var configurationTaskTypes = this.ScanAssembliesForTypesImplementing<IConfigurationBootstrapTask>();
            var configurationTasks = configurationTaskTypes
                .Where(t => typeof(IConfigurationBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IConfigurationBootstrapTask)Activator.CreateInstance(t));

            return Task.FromResult(configurationTasks);
        }

        public Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            var serviceTaskTypes = this.ScanAssembliesForTypesImplementing<IServiceBootstrapTask>();
            var serviceTasks = serviceTaskTypes
                .Where(t => typeof(IServiceBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IServiceBootstrapTask)Activator.CreateInstance(t));

            return Task.FromResult(serviceTasks);
        }

        public Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            var containerTaskTypes = this.ScanAssembliesForTypesImplementing<IContainerBootstrapTask>();
            var containerTasks = containerTaskTypes
                .Where(t => typeof(IContainerBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IContainerBootstrapTask)Activator.CreateInstance(t));

            return Task.FromResult(containerTasks);
        }

        public Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            return Task.FromResult(this.ScanAssembliesForTypesImplementing<IApplicationBootstrapTask>());
        }

        private IEnumerable<Type> ScanAssembliesForTypesImplementing<T>()
        {
            return this.assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => !t.IsInterface && typeof(T).IsAssignableFrom(t));
        }
    }
}