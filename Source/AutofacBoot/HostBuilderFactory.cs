﻿using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal class HostBuilderFactory
    {        
        public IWebHostBuilder Create(
            string[] arguments, 
            IAutofacBootTaskResolver taskResolver, 
            IContainerConfiguration containerConfiguration,
            Action<Exception, ILoggerFactory> exceptionHandler)
        {
            var hostBuilder = arguments == null
                ? Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                : Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(arguments);

            var webHostBuilder = hostBuilder.ConfigureServices(services =>
                {
                    services.AddSingleton(taskResolver ?? AssemblyTaskResolver.Default);
                    services.AddSingleton(containerConfiguration ?? new NullContainerConfiguration());
                    services.AddAutofac();
                })
                .UseStartup<AutofacBootStartup>();

            return new WebHostBuilder(webHostBuilder, exceptionHandler);
        }        
    }
}