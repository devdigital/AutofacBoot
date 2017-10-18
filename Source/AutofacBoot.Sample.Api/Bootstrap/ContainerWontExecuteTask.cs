using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ContainerWontExecuteTask : IContainerBootstrapTask, IConditionalExecution
    {
        public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        public Task Execute(IHostingEnvironment environment, IConfigurationRoot configuration, ContainerBuilder builder)
        {
            throw new System.NotImplementedException();
        }        
    }
}