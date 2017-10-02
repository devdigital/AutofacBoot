using System;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace AutofacBoot
{
    public interface IAutofacBootBuilder
    {
        IAutofacBootBuilder WithArguments(string[] arguments);

        IAutofacBootBuilder WithTasks(IAutofacBootTaskResolver taskResolver);

        IAutofacBootBuilder WithContainer(IContainerConfiguration containerConfiguration);

        IWebHostBuilder Configure();

        void Run();        
    }
}