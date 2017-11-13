using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AutofacBoot
{
    internal class WebHostBuilder : IWebHostBuilder
    {
        private readonly IWebHostBuilder webHostBuilder;

        private readonly Func<Exception, ILoggerFactory, bool> exceptionHandler;

        public WebHostBuilder(IWebHostBuilder webHostBuilder, Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.webHostBuilder = webHostBuilder ?? throw new ArgumentNullException(nameof(webHostBuilder));
            this.exceptionHandler = exceptionHandler;
        }

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

        public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return this.webHostBuilder.ConfigureAppConfiguration(configureDelegate);
        }

        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return this.webHostBuilder.ConfigureServices(configureServices);
        }

        public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            return this.webHostBuilder.ConfigureServices(configureServices);
        }

        public string GetSetting(string key)
        {
            return this.webHostBuilder.GetSetting(key);
        }

        public IWebHostBuilder UseSetting(string key, string value)
        {
            return this.webHostBuilder.UseSetting(key, value);
        }
    }
}