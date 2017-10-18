using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ServiceWontExecuteTask : IServiceBootstrapTask, IConditionalExecution
    {
        public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {         
            return Task.FromResult(false);
        }

        public Task Execute(
            IHostingEnvironment environment, 
            IConfigurationRoot configuration, 
            IServiceCollection services)
        {
            throw new System.NotImplementedException();
        }        
    }
}