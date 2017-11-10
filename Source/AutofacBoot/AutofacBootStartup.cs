using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    public class AutofacBootStartup : IStartup
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly IHostingEnvironment hostingEnvironment;

        private readonly AutofacStartupHelper startupHelper;

        private readonly IContainerConfiguration containerConfiguration;

        public AutofacBootStartup(
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment, 
            IAutofacBootTaskResolver taskResolver,
            IContainerConfiguration containerConfiguration)
        {
            if (taskResolver == null)
            {
                throw new ArgumentNullException(nameof(taskResolver));
            }

            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.hostingEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.startupHelper = new AutofacStartupHelper(taskResolver);
            this.Configuration = this.startupHelper.Configuration(environment);
            this.containerConfiguration = containerConfiguration ?? throw new ArgumentNullException(nameof(containerConfiguration));
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                this.startupHelper.ConfigureServices(services);

                var builder = new ContainerBuilder();
                builder.Populate(services);

                this.startupHelper.ConfigureContainer(builder, this.Configuration);

                containerConfiguration.Configure(this.hostingEnvironment, builder);

                this.ApplicationContainer = builder.Build();
                return new AutofacServiceProvider(this.ApplicationContainer);
            }
            catch (Exception exception)
            {
                throw new AutofacBootException(this.loggerFactory, exception);
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            try
            {
                var bootstrapTasks = app.ApplicationServices
                    .GetServices(typeof(IApplicationBootstrapTask))
                    .Cast<IApplicationBootstrapTask>();

                this.startupHelper.Configure(app, bootstrapTasks);

                var appLifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;
                appLifetime?.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
            }
            catch (Exception exception)
            {
                throw new AutofacBootException(this.loggerFactory, exception);
            }       
        }
    }
}