using Autofac;

namespace AutofacBoot.UnitTests
{
    public class Foo : IFoo
    {
        private readonly ContainerBuilder containerBuilder;

        public Foo()
        {
            this.containerBuilder = new ContainerBuilder();
        }

        public ContainerBuilder GetContainerBuilder()
        {
            return this.containerBuilder;
        }

        public IContainer Build()
        {
            return this.containerBuilder.Build();
        }
    }
}