using Autofac.Extras.IocManager.Extensions;

namespace Autofac.Extras.IocManager.Tests
{
    public abstract class TestBase
    {
        internal ContainerBuilder Builder;

        protected TestBase()
        {
            Builder = new ContainerBuilder().RegisterIocManager();
        }
    }
}
