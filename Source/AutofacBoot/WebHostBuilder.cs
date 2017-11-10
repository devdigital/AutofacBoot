using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal class WebHostBuilder : IWebHostBuilder
    {
        private readonly IWebHostBuilder webHostBuilder;

        private readonly Action<Exception, ILoggerFactory> exceptionHandler;

        public WebHostBuilder(IWebHostBuilder webHostBuilder, Action<Exception, ILoggerFactory> exceptionHandler)
        {
            this.webHostBuilder = webHostBuilder ?? throw new ArgumentNullException(nameof(webHostBuilder));
            this.exceptionHandler = exceptionHandler;
        }

        public IWebHost Build()
        {
            try
            {
                this.webHostBuilder.Build();
            }
            catch (AutofacBootException exception)
            {
                if (this.exceptionHandler == null)
                {
                    throw;
                }

                this.exceptionHandler(
                    exception.InnerException, exception.LoggerFactory);
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