using System;

namespace Autofac.Extras.IocManager.TestBase
{
    public class TestBaseWithIocBuilder
    {
        protected IIocBuilder IocBuilder;
        protected IIocManager LocalIocManager;

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

        protected T The<T>()
        {
            return LocalIocManager.Resolve<T>();
        }
    }
}
