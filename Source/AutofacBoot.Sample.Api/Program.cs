using System.Net;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot.Sample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new AutofacBootstrapper()
                .Run();
        }
    }
}
