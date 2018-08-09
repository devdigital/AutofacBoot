// <copyright file="WebHostBuilder.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Web host builder.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Hosting.IWebHostBuilder" />
    internal class WebHostBuilder : IWebHostBuilder
    {
        private readonly IWebHostBuilder webHostBuilder;

        private readonly Func<Exception, ILoggerFactory, bool> exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHostBuilder"/> class.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        public WebHostBuilder(IWebHostBuilder webHostBuilder, Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.webHostBuilder = webHostBuilder ?? throw new ArgumentNullException(nameof(webHostBuilder));
            this.exceptionHandler = exceptionHandler;
        }

        /// <inheritdoc />
        public IWebHost Build()
        {
            try
            {
                return this.webHostBuilder.Build();
            }
            catch (AutofacBootException exception)
            {
                if (this.exceptionHandler == null)
                {
                    throw exception.InnerException;
                }

                var handled = this.exceptionHandler(
                    exception.InnerException, exception.LoggerFactory);

                if (!handled)
                {
                    throw exception.InnerException;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return this.webHostBuilder.ConfigureAppConfiguration(configureDelegate);
        }

        /// <inheritdoc />
        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return this.webHostBuilder.ConfigureServices(configureServices);
        }

        /// <inheritdoc />
        public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            return this.webHostBuilder.ConfigureServices(configureServices);
        }

        /// <inheritdoc />
        public string GetSetting(string key)
        {
            return this.webHostBuilder.GetSetting(key);
        }

        /// <inheritdoc />
        public IWebHostBuilder UseSetting(string key, string value)
        {
            return this.webHostBuilder.UseSetting(key, value);
        }
    }
}