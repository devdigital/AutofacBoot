using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ServiceWontExecuteTask : IServiceBootstrapTask, IConditionalExecution
    {
        public Task<bool> CanExecute(IConfigurationRoot configurationRoot)
        {         
            return Task.FromResult(false);
        }

        public Task Execute(IServiceCollection services)
        {
            throw new System.NotImplementedException();
        }        
    }
}