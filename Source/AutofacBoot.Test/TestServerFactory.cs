using System;
using System.Collections.Generic;
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

        public virtual TestServer Create()
        {
            var hostBuilder = new AutofacBootstrapper()
                .WithTasks(this.GetTaskResolver())
                .WithContainer(new TestContainerConfiguration(
                    this.TypeRegistrations,
                    this.InstanceRegistrations))
                .Configure();

            hostBuilder = this.Configure(hostBuilder);

            return new TestServer(hostBuilder);
        }

        protected abstract IAutofacBootTaskResolver GetTaskResolver();

        protected virtual IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder;
        }
    }
}