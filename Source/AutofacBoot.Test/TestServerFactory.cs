using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutofacBoot.Test.Sources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace AutofacBoot.Test
{
    public abstract class TestServerFactory<TServerFactory>
        where TServerFactory : TestServerFactory<TServerFactory>
    {
        private Dictionary<Type, Type> TypeRegistrations { get; }

        private Dictionary<Type, object> InstanceRegistrations { get; }
        
        protected TestServerFactory()
        {
            this.TypeRegistrations = new Dictionary<Type, Type>();
            this.InstanceRegistrations = new Dictionary<Type, object>();
        }

        public TServerFactory With<TInterface, TImplementation>()
        {
            if (this.TypeRegistrations.ContainsKey(typeof(TInterface)))
            {
                throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
            }

            this.TypeRegistrations[typeof(TInterface)] = typeof(TImplementation);
            return this as TServerFactory;
        }

        public TServerFactory With<TInterface>(object instance)
        {
            if (this.InstanceRegistrations.ContainsKey(typeof(TInterface)))
            {
                throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
            }

            this.InstanceRegistrations[typeof(TInterface)] = instance ?? throw new ArgumentNullException(nameof(instance));
            return this as TServerFactory;
        }

        public virtual async Task<TestServer> Create()
        {
            var taskResolver = await this.GetAdditionalConfigurationTaskResolver();

            var hostBuilder = new AutofacBootstrapper()
                .WithTasks(taskResolver)
                .WithContainer(new TestContainerConfiguration(
                    this.TypeRegistrations,
                    this.InstanceRegistrations))
                .Configure();

            hostBuilder = this.Configure(hostBuilder);

            return new TestServer(hostBuilder);
        }

        protected abstract Task<ITaskResolver> GetTaskResolver();

        protected virtual Task<IDictionary<string, string>> GetConfiguration()
        {
            return Task.FromResult<IDictionary<string, string>>(null);
        }

        protected virtual IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder;
        }

        private async Task<ITaskResolver> GetAdditionalConfigurationTaskResolver()
        {
            var taskResolver = await this.GetTaskResolver();
            var configuration = await this.GetConfiguration();
            return new AdditionalConfigurationTaskResolver(taskResolver, configuration);
        }
    }
}