using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    internal class HostBuilderFactory
    {        
        public IWebHostBuilder Create(
            string[] arguments, 
            IAutofacBootTaskResolver taskResolver, 
            IContainerConfiguration containerConfiguration)
        {
            var hostBuilder = arguments == null
                ? WebHost.CreateDefaultBuilder()
                : WebHost.CreateDefaultBuilder(arguments);

            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(taskResolver ?? AssemblyTaskResolver.Default);
                services.AddSingleton(containerConfiguration ?? new NullContainerConfiguration());
                services.AddAutofac();
            })
            .UseStartup<AutofacBootStartup>();
        }        
    }
}