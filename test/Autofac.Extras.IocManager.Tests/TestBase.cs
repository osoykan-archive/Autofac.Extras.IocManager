namespace Autofac.Extras.IocManager.Tests
{
    public abstract class TestBase
    {
        protected IocManager LocalIocManager;
        protected ContainerBuilder Builder;

        protected TestBase()
        {
            LocalIocManager = new IocManager();
            Builder = new ContainerBuilder().RegisterIocManager(LocalIocManager);
        }
    }
}
