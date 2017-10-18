using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    public interface IConditionalExecution
    {
        Task<bool> CanExecute(
            IHostingEnvironment environment,
            IConfigurationRoot configurationRoot);
    }
}