using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace AutofacBoot
{
    internal class WebHost : IWebHost
    {
        private readonly IWebHost build;

        private readonly Func<Exception, ILoggerFactory, bool> exceptionHandler;

        public WebHost(IWebHost build, Func<Exception, ILoggerFactory, bool> exceptionHandler)
        {
            this.build = build ?? throw new ArgumentNullException(nameof(build));
            this.exceptionHandler = exceptionHandler;
        }

        public void Dispose()
        {
            this.build.Dispose();
        }

        public void Start()
        {
            this.build.Start();
        }

        public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return this.build.StartAsync(cancellationToken);
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

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return this.build.StopAsync(cancellationToken);
        }

        public IFeatureCollection ServerFeatures => this.build.ServerFeatures;

        public IServiceProvider Services => this.build.Services;
    }
}