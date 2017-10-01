using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public class AutofacBootStartup : IStartup
    {
        private readonly AutofacStartupHelper startupHelper;

        public AutofacBootStartup(
            IHostingEnvironment env, 
            IAutofacBootTaskResolver taskResolver)
        {
            this.startupHelper = new AutofacStartupHelper(taskResolver);
            this.Configuration = this.startupHelper.Configuration(env);
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.startupHelper.ConfigureServices(services);

            var builder = new ContainerBuilder();            
            builder.Populate(services);

            this.startupHelper.ConfigureContainer(builder, this.Configuration);

            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void Configure(
            IApplicationBuilder app)
        {
            var bootstrapTasks = app.ApplicationServices
                .GetServices(typeof(IApplicationBootstrapTask))
                .Cast<IApplicationBootstrapTask>();

            this.startupHelper.Configure(app, bootstrapTasks);

            var appLifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;
            appLifetime?.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}