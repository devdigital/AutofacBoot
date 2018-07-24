using System.Collections.Generic;
using System.Threading.Tasks;
using AutofacBoot.Test.Sources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace AutofacBoot.Test
{
    public abstract class TestServerFactory<TServerFactory>
        where TServerFactory : TestServerFactory<TServerFactory>
    {
        private readonly DictionaryContainerConfiguration containerConfiguration;
        
        protected TestServerFactory()
        {
            this.containerConfiguration = new DictionaryContainerConfiguration();
        }

        public TServerFactory With<TInterface, TImplementation>()
        {
            this.containerConfiguration.With<TInterface, TImplementation>();
            return this as TServerFactory;
        }

        public TServerFactory With<TInterface>(object instance)
        {
            this.containerConfiguration.With<TInterface>(instance);
            return this as TServerFactory;
        }

        public virtual async Task<TestServer> Create()
        {
            var taskResolver = await this.GetAdditionalConfigurationTaskResolver();

            var hostBuilder = new AutofacBootstrapper()
                .WithTasks(taskResolver)
                .WithContainer(this.containerConfiguration)
                .Configure();

            hostBuilder = this.Configure(hostBuilder);

            return new TestServer(hostBuilder);
        }

        protected abstract Task<ITaskResolver> GetTaskResolver();

        protected virtual Task<IDictionary<string, string>> GetConfiguration()
        {
            return Task.FromResult<IDictionary<string, string>>(null);
        }

        protected virtual IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            return hostBuilder;
        }

        private async Task<ITaskResolver> GetAdditionalConfigurationTaskResolver()
        {
            var taskResolver = await this.GetTaskResolver();
            var configuration = await this.GetConfiguration();
            return new AdditionalConfigurationTaskResolver(taskResolver, configuration);
        }
    }
}