using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace AutofacBoot.Sample.Api.Bootstrap
{
    public class ApplicationFirstBootstrapTask : IApplicationBootstrapTask, IOrderedTask
    {
        public Task Execute(IApplicationBuilder app)
        {
            return Task.CompletedTask;
        }

        public int Order => -10;
    }
}