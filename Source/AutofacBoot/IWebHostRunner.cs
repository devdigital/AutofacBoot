using System.Threading.Tasks;

namespace AutofacBoot
{
    public interface IWebHostRunner
    {
        void Run();

        Task RunAsync();
    }
}