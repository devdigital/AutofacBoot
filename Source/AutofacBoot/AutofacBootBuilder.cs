using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    internal class AutofacBootBuilder : IAutofacBootBuilder
    {
        private string[] arguments;

        private IAutofacBootTaskResolver taskResolver;

        private IContainerConfiguration containerConfiguration;

        public AutofacBootBuilder(string[] arguments)
        {
            this.WithArguments(arguments);
        }

        public AutofacBootBuilder(IAutofacBootTaskResolver taskResolver)
        {
            this.WithTasks(taskResolver);
        }

        public AutofacBootBuilder(IContainerConfiguration containerConfiguration)
        {
            this.WithContainer(containerConfiguration);
        }

        public IAutofacBootBuilder WithArguments(string[] arguments)
        {
            this.arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            return this;
        }

        public IAutofacBootBuilder WithTasks(IAutofacBootTaskResolver taskResolver)
        {
            this.taskResolver = taskResolver ?? throw new ArgumentNullException(nameof(taskResolver));
            return this;
        }

        public IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration)
        {
            this.containerConfiguration = containerConfiguration ?? throw new ArgumentNullException(nameof(containerConfiguration));
            return this;
        }

        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                this.arguments,
                this.taskResolver,
                this.containerConfiguration);
        }

        public void Run()
        {
            var host = this.Configure().Build();
            host.Run();
        }
    }
}