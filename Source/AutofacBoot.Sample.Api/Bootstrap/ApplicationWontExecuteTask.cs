using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ApplicationWontExecuteTask : IApplicationBootstrapTask, IConditionalExecution
    {
        public Task<bool> CanExecute(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {
            return Task.FromResult(false);
        }

        public Task Execute(IApplicationBuilder app)
        {
            throw new System.NotImplementedException();
        }        
    }
}