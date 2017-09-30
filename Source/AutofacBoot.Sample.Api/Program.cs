using AutofacBoot.Sample.Api.Bootstrap;

namespace AutofacBoot.Sample.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new AutofacBoot()
                .WithArguments(args)
                .WithTasks(new AssemblyTaskResolver(typeof(ApplicationBootstrapTask).Assembly))
                //.WithContainer(container => { })
                .Run();
        }
    }
}
