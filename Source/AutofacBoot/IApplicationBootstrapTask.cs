using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace AutofacBoot
{
    public interface IApplicationBootstrapTask
    {
        Task Execute(IApplicationBuilder app);
    }
}