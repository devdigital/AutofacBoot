using System;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal sealed class ApplicationLogging
    {
        private static readonly Lazy<ApplicationLogging> lazy =
            new Lazy<ApplicationLogging>(() => new ApplicationLogging());

        private ILoggerFactory loggerFactory;

        public static ApplicationLogging Instance => lazy.Value;

        private ApplicationLogging()
        {
        }

        public void ConfigureLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public ILoggerFactory GetLoggerFactory()
        {
            return this.loggerFactory;
        }
    }
}