using System.Collections.Generic;
using Autofac;
using AutofacBoot.Sample.Api.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot.Sample.Api
{
    public class Startup
    {
        private readonly AutofacStartup bootstrapper;

        public Startup(IHostingEnvironment env)
        {
            this.bootstrapper = new AutofacStartup(
                new AssemblyTaskResolver(typeof(Foo).Assembly));

            this.Configuration = this.bootstrapper.ConfigureFoo(env);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services            
            services.AddMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            this.bootstrapper.ConfigureContainer(builder, this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IEnumerable<IBootstrapTask> bootstrapTasks)
        {
            this.bootstrapper.Configure(app, bootstrapTasks);
        }
    }
}
