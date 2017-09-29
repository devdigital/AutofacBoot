using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutofacBoot
{
    public class AssemblyTaskResolver : ITaskResolver
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

        public Task<IEnumerable<Type>> GetConfigurationTasks()
        {
            return Task.FromResult(this.ScanAssembliesForTypesImplementing<IConfigurationBootstrapTask>());
        }

        public Task<IEnumerable<Type>> GetContainerTasks()
        {
            return Task.FromResult(this.ScanAssembliesForTypesImplementing<IContainerBootstrapTask>());
        }

        public Task<IEnumerable<Type>> GetBootstrapTasks()
        {
            return Task.FromResult(this.ScanAssembliesForTypesImplementing<IBootstrapTask>());
        }

        private IEnumerable<Type> ScanAssembliesForTypesImplementing<T>()
        {
            return this.assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => !t.IsInterface && typeof(T).IsAssignableFrom(t));
        }
    }
}