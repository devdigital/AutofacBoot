using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacBoot
{
    public interface IServiceBootstrapTask
    {
        Task Execute(IServiceCollection services);
    }
}