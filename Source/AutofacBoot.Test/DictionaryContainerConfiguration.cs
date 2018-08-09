// <copyright file="DictionaryContainerConfiguration.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Dictionary container configuration.
    /// </summary>
    /// <seealso cref="AutofacBoot.IContainerConfiguration" />
    public class DictionaryContainerConfiguration : IContainerConfiguration
    {
        private readonly IDictionary<Type, Type> typeRegistrations;

        private readonly IDictionary<Type, object> instanceRegistrations;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryContainerConfiguration"/> class.
        /// </summary>
        public DictionaryContainerConfiguration()
        {
            this.typeRegistrations = new Dictionary<Type, Type>();
            this.instanceRegistrations = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Add a type registration.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>The configuration.</returns>
        public DictionaryContainerConfiguration With<TInterface, TImplementation>()
        {
            if (this.typeRegistrations.ContainsKey(typeof(TInterface)))
            {
                throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
            }

            this.typeRegistrations[typeof(TInterface)] = typeof(TImplementation);
            return this;
        }

        /// <summary>
        /// Adds an instance registration.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>The configuration.</returns>
        public DictionaryContainerConfiguration With<TInterface>(object instance)
        {
            if (this.instanceRegistrations.ContainsKey(typeof(TInterface)))
            {
                throw new InvalidOperationException($"The type {typeof(TInterface).Name} has already been registered");
            }

            this.instanceRegistrations[typeof(TInterface)] = instance ?? throw new ArgumentNullException(nameof(instance));
            return this;
        }

        /// <inheritdoc />
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