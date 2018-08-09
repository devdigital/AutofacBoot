// <copyright file="AutofacBootException.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AutofacBoot exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class AutofacBootException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacBootException"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="innerException">The inner exception.</param>
        public AutofacBootException(ILoggerFactory loggerFactory, Exception innerException)
            : base("There was an error during bootstrapping.", innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }

            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        /// <value>
        /// The logger factory.
        /// </value>
        public ILoggerFactory LoggerFactory { get; }
    }
}