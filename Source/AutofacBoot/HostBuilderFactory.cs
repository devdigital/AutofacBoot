using System;
using System.Reflection;
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
            Action<ContainerBuilder> configureContainer)
        {
            var hostBuilder = arguments == null
                ? WebHost.CreateDefaultBuilder()
                : WebHost.CreateDefaultBuilder(arguments);

            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartup>(serviceProvider =>
                {
                    var hostingEnvironment = serviceProvider
                        .GetRequiredService<IHostingEnvironment>();

                    return new AutofacBootStartup(
                        hostingEnvironment,
                        taskResolver ?? AssemblyTaskResolver.Default);
                });

                services.AddAutofac();
            })
            .UseSetting(WebHostDefaults.ApplicationKey, typeof(AutofacBootStartup).GetTypeInfo().Assembly.FullName);
        }        
    }
}