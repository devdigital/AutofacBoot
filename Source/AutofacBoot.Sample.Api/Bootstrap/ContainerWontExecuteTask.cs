using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ContainerWontExecuteTask : IContainerBootstrapTask, IConditionalExecution
    {
        public Task<bool> CanExecute(IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        public Task Execute(ContainerBuilder builder)
        {
            throw new System.NotImplementedException();
        }        
    }
}