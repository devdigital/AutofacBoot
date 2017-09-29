using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace AutofacBoot
{
    public interface IBootstrapTask
    {
        Task Execute(IApplicationBuilder app);
    }
}