using Autofac;

namespace AutofacBoot
{
    public interface IFoo
    {
        ContainerBuilder GetContainerBuilder();

        IContainer Build();
    }
}