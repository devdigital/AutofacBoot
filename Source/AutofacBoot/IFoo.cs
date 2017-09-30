using System;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    public interface IFoo
    {
        IFoo WithArguments(string[] arguments);

        IFoo WithTasks(IAutofacBootTaskResolver taskResolver);

        IFoo WithContainer(Action<ContainerBuilder> configureContainer);

        IWebHostBuilder Configure();

        void Run();        
    }
}