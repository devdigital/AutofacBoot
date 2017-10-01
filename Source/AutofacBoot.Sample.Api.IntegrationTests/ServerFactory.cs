using AutofacBoot.Sample.Api.Bootstrap;
using AutofacBoot.Test;

namespace AutofacBoot.Sample.Api.IntegrationTests
{
    public class ServerFactory : FooFactory<ServerFactory>
    {
        public ServerFactory() : base(
            new AssemblyTaskResolver(typeof(ServiceBootstrapTask).Assembly))
        {            
        }
    }
}