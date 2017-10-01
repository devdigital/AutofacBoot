using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.AspNetCore.TestHost;

namespace AutofacBoot.Test
{
    public class FooFactory<TServerFactory>
        where TServerFactory : FooFactory<TServerFactory>
    {
        private Dictionary<Type, Type> TypeRegistrations { get; }

        private Dictionary<Type, object> InstanceRegistrations { get; }

        private readonly IAutofacBootTaskResolver taskResolver;

        public FooFactory() : this(AssemblyTaskResolver.Default)
        {        
        }

        public FooFactory(IAutofacBootTaskResolver taskResolver)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
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
                .WithTasks(this.taskResolver)
                .WithContainer(builder =>
                {
                    foreach (var typeRegistration in this.TypeRegistrations)
                    {
                        builder.RegisterType(typeRegistration.Value).As(typeRegistration.Key);
                    }

                    foreach (var instanceRegistration in this.InstanceRegistrations)
                    {
                        builder.RegisterInstance(instanceRegistration.Value).As(instanceRegistration.Key);
                    }
                })
                .Configure();

            return new TestServer(hostBuilder);
        }
    }
}