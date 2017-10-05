using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AutofacBoot
{
    public interface IConditionalExecution
    {
        Task<bool> CanExecute(IConfigurationRoot configurationRoot);
    }
}