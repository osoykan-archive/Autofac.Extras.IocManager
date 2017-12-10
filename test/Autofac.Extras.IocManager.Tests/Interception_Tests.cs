using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Castle.DynamicProxy;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class Interception_Tests : TestBaseWithIocBuilder
    {
        public Interception_Tests()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.AddInterceptionCallback(type => typeof(IShouldBeIntercepted).IsAssignableFrom(type), typeof(UnitOfWorkInterceptor));
                    r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                });
            });
        }

        [Fact]
        public void shoudld_work()
        {
            The<IShouldBeIntercepted>().InterceptMe();
        }
    }

    public class ShouldBeIntercepted : IShouldBeIntercepted, ITransientDependency
    {
        public void InterceptMe()
        {
        }
    }

    public class UnitOfWorkInterceptor : IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Should().NotBeNull();
            invocation.Proceed();
        }
    }
}
