// <copyright file="WebHostRunner.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutofacBoot
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// Web host runner.
    /// </summary>
    /// <seealso cref="AutofacBoot.IWebHostRunner" />
    internal class WebHostRunner : IWebHostRunner
    {
        private readonly IWebHost host;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHostRunner"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public WebHostRunner(IWebHost host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        /// <inheritdoc />
        public void Run()
        {
            this.host.Run();
        }

        /// <inheritdoc />
        public async Task RunAsync()
        {
            await this.host.RunAsync();
        }
    }
}