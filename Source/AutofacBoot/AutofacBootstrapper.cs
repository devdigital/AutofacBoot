using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    public class AutofacBootstrapper
    {
        public IAutofacBootBuilder WithArguments(string[] arguments)
        {
            return new AutofacBootBuilder(arguments);
        }

        public IAutofacBootBuilder WithTasks(IAutofacBootTaskResolver taskResolver)
        {
            return new AutofacBootBuilder(taskResolver);
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