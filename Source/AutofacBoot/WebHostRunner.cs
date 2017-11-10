using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    internal class WebHostRunner : IWebHostRunner
    {
        private readonly IWebHost host;

        public WebHostRunner(IWebHost host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public void Run()
        {
            this.host.Run();
        }

        public async Task RunAsync()
        {
            await this.host.RunAsync();
        }
    }
}