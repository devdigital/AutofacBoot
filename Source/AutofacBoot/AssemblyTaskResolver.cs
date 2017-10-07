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

        private readonly IOrderedTaskResolver orderedTaskResolver;

        public AssemblyTaskResolver(Assembly assembly) 
            : this(assembly, new NumberedOrderedTaskResolver())
        {            
        }

        public AssemblyTaskResolver(Assembly assembly, IOrderedTaskResolver orderedTaskResolver)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            this.assemblies = new List<Assembly> { assembly };
            this.orderedTaskResolver = orderedTaskResolver ?? throw new ArgumentNullException(nameof(orderedTaskResolver));
        }

        public AssemblyTaskResolver(IEnumerable<Assembly> assemblies) 
            : this(assemblies, new NumberedOrderedTaskResolver())
        {            
        }

        public AssemblyTaskResolver(IEnumerable<Assembly> assemblies, IOrderedTaskResolver orderedTaskResolver)
        {
            this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
            this.orderedTaskResolver = orderedTaskResolver ?? throw new ArgumentNullException(nameof(orderedTaskResolver));
        }

        public static AssemblyTaskResolver Default => new AssemblyTaskResolver(
            Assembly.GetEntryAssembly());    

        public async Task<IEnumerable<IConfigurationBootstrapTask>> GetConfigurationTasks()
        {
            var configurationTaskTypes = this.ScanAssembliesForTypesImplementing<IConfigurationBootstrapTask>();
            var configurationTasks = configurationTaskTypes
                .Where(t => typeof(IConfigurationBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IConfigurationBootstrapTask) Activator.CreateInstance(t))
                .ToList();

            return await this.orderedTaskResolver
                .GetOrderedConfigurationTasks(configurationTasks);
        }

        public async Task<IEnumerable<IServiceBootstrapTask>> GetServiceTasks()
        {
            var serviceTaskTypes = this.ScanAssembliesForTypesImplementing<IServiceBootstrapTask>();
            var serviceTasks = serviceTaskTypes
                .Where(t => typeof(IServiceBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IServiceBootstrapTask) Activator.CreateInstance(t))
                .ToList();

            return await this.orderedTaskResolver
                .GetOrderedServiceTasks(serviceTasks);
        }

        public async Task<IEnumerable<IContainerBootstrapTask>> GetContainerTasks()
        {
            var containerTaskTypes = this.ScanAssembliesForTypesImplementing<IContainerBootstrapTask>();
            var containerTasks = containerTaskTypes
                .Where(t => typeof(IContainerBootstrapTask).IsAssignableFrom(t))
                .Select(t => (IContainerBootstrapTask) Activator.CreateInstance(t))
                .ToList();

            return await this.orderedTaskResolver
                .GetOrderedContainerTasks(containerTasks);            
        }

        public async Task<IEnumerable<Type>> GetApplicationTaskTypes()
        {
            var applicationTasks = 
                this.ScanAssembliesForTypesImplementing<IApplicationBootstrapTask>();

            return await this.orderedTaskResolver
                .GetOrderedApplicationTasks(applicationTasks);            
        }

        private IEnumerable<Type> ScanAssembliesForTypesImplementing<T>()
        {
            return this.assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => !t.IsInterface && typeof(T).IsAssignableFrom(t));
        }
    }
}