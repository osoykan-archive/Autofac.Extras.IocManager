using System;

namespace Autofac.Extras.IocManager.Tests
{
    public abstract class TestBase
    {
        protected ContainerBuilder Builder;
        protected IocManager LocalIocManager;

        protected TestBase()
        {
            LocalIocManager = new IocManager();
            Builder = new ContainerBuilder().RegisterIocManager(LocalIocManager);
        }

        protected IContainer Building(Action<ContainerBuilder> builderAction)
        {
            builderAction(Builder);
            return Builder.Build().UseIocManager(LocalIocManager);
        }
    }
}
