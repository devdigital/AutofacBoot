using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot.Test
{
    public class DictionaryContainerConfiguration : IContainerConfiguration
    {
        private readonly IDictionary<Type, Type> typeRegistrations;

        private readonly IDictionary<Type, object> instanceRegistrations;

        public DictionaryContainerConfiguration()
        {
            this.typeRegistrations = new Dictionary<Type, Type>();
            this.instanceRegistrations = new Dictionary<Type, object>();
        }

        public DictionaryContainerConfiguration With<TInterface, TImplementation>()
        {
            if (this.typeRegistrations.ContainsKey(typeof(TInterface)))
            {
                throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
            }

            this.typeRegistrations[typeof(TInterface)] = typeof(TImplementation);
            return this;
        }

        public DictionaryContainerConfiguration With<TInterface>(object instance)
        {
            if (this.instanceRegistrations.ContainsKey(typeof(TInterface)))
            {
                throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
            }

            this.instanceRegistrations[typeof(TInterface)] = instance ?? throw new ArgumentNullException(nameof(instance));
            return this;
        }

        public Task Configure(IHostingEnvironment environment, ContainerBuilder builder)
        {
            foreach (var typeRegistration in this.typeRegistrations)
            {
                builder.RegisterType(typeRegistration.Value).As(typeRegistration.Key);
            }

            foreach (var instanceRegistration in this.instanceRegistrations)
            {
                builder.RegisterInstance(instanceRegistration.Value).As(instanceRegistration.Key);
            }

            return Task.CompletedTask;
        }
    }
}