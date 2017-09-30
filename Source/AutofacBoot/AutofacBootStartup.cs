using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public class AutofacBootStartup : IStartup
    {
        private readonly AutofacStartup bootstrapper;
       
        public AutofacBootStartup(IHostingEnvironment env, IAutofacBootTaskResolver taskResolver)
        {
            this.bootstrapper = new AutofacStartup(taskResolver);
            this.Configuration = this.bootstrapper.Configuration(env);
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.bootstrapper.ConfigureServices(services);            
            return services.BuildServiceProvider(); 
        }        

        public void ConfigureContainer(ContainerBuilder builder)
        {
            this.bootstrapper.ConfigureContainer(builder, this.Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            var bootstrapTasks = app.ApplicationServices
                .GetServices(typeof(IApplicationBootstrapTask))
                .Cast<IApplicationBootstrapTask>();

            this.bootstrapper.Configure(app, bootstrapTasks);
        }
    }
}