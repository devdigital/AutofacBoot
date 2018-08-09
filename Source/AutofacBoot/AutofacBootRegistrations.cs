// <copyright file="AutofacBootRegistrations.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// AutofacBoot registrations.
    /// </summary>
    public class AutofacBootRegistrations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootRegistrations"/> class.
        /// </summary>
        /// <param name="typeRegistrations">The type registrations.</param>
        /// <param name="instanceRegistrations">The instance registrations.</param>
        public AutofacBootRegistrations(IDictionary<Type, Type> typeRegistrations, IDictionary<Type, object> instanceRegistrations)
        {
            this.TypeRegistrations = typeRegistrations ?? new Dictionary<Type, Type>();
            this.InstanceRegistrations = instanceRegistrations ?? new Dictionary<Type, object>();
        }

        /// <summary>
        /// Gets the type registrations.
        /// </summary>
        /// <value>
        /// The type registrations.
        /// </value>
        public IDictionary<Type, Type> TypeRegistrations { get; }

        /// <summary>
        /// Gets the instance registrations.
        /// </summary>
        /// <value>
        /// The instance registrations.
        /// </value>
        public IDictionary<Type, object> InstanceRegistrations { get; }
    }
}