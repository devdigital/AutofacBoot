using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

namespace AutofacBoot.Test
{
    internal class TestContainerConfiguration : IContainerConfiguration
    {
        private readonly IDictionary<Type, Type> typeRegistrations;

        private readonly IDictionary<Type, object> instanceRegistrations;

        public TestContainerConfiguration(
            IDictionary<Type, Type> typeRegistrations, 
            IDictionary<Type, object> instanceRegistrations)
        {
            this.typeRegistrations = typeRegistrations ?? throw new ArgumentNullException(nameof(typeRegistrations));
            this.instanceRegistrations = instanceRegistrations ?? throw new ArgumentNullException(nameof(instanceRegistrations));
        }

        public Task Configure(ContainerBuilder builder)
        {
            foreach (var typeRegistration in this.typeRegistrations)
            {
                builder.RegisterType(typeRegistration.Value).As(typeRegistration.Key);
            }

            foreach (var instanceRegistration in this.instanceRegistrations)
            {
                builder.RegisterInstance(instanceRegistration.Value).As(instanceRegistration.Key);
            }

            return Task.FromResult(0);
        }
    }
}