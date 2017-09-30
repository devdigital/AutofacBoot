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

        private Action<ContainerBuilder> configureContainer;

        public AutofacBootBuilder(string[] arguments)
        {
            this.WithArguments(arguments);
        }

        public AutofacBootBuilder(IAutofacBootTaskResolver taskResolver)
        {
            this.WithTasks(taskResolver);
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

        public IAutofacBootBuilder WithContainer(Action<ContainerBuilder> configureContainer)
        {
            this.configureContainer = configureContainer ?? throw new ArgumentNullException(nameof(configureContainer));
            return this;
        }

        public IWebHostBuilder Configure()
        {
            return new HostBuilderFactory().Create(
                this.arguments,
                this.taskResolver,
                this.configureContainer);
        }

        public void Run()
        {
            var host = this.Configure().Build();
            host.Run();
        }
    }
}