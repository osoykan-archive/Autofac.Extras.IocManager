using System;

namespace Autofac.Extras.IocManager.Tests
{
    public class TestBaseWithIocBuilder
    {
        protected IIocBuilder IocBuilder;
        protected IocManager LocalIocManager;

        protected TestBaseWithIocBuilder()
        {
            LocalIocManager = new IocManager();
            IocBuilder = Extras.IocManager.IocBuilder.New
                               .UseAutofacContainerBuilder()
                               .RegisterIocManager(LocalIocManager);
        }

        protected IResolver Building(Action<IIocBuilder> builderAction)
        {
            builderAction(IocBuilder);
            return IocBuilder.CreateResolver().UseIocManager(LocalIocManager);
        }
    }
}
