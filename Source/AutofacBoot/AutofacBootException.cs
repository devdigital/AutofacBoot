using System;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    public class AutofacBootException : Exception
    {
        public AutofacBootException(ILoggerFactory loggerFactory, Exception innerException) : base(
            "There was an error during bootstrapping.", innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException(nameof(innerException));
            }

            this.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public ILoggerFactory LoggerFactory { get; }
    }
}