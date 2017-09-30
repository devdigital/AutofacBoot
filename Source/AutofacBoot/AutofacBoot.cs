using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    public class AutofacBoot
    {
        public IFoo WithArguments(string[] arguments)
        {
            return new Foo(arguments);
        }

        public IFoo WithTasks(IAutofacBootTaskResolver taskResolver)
        {
            return new Foo(taskResolver);
        }

        public void Run()
        {
            var hostBuilder = new HostBuilderFactory().Create(
                arguments: null,
                taskResolver: null,
                configureContainer: null);

            hostBuilder.Build().Run();
        }
    }
}