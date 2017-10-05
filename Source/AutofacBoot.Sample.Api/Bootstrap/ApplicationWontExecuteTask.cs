using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ApplicationWontExecuteTask : IApplicationBootstrapTask, IConditionalExecution
    {
        public Task<bool> CanExecute(IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        public Task Execute(IApplicationBuilder app)
        {
            throw new System.NotImplementedException();
        }        
    }
}