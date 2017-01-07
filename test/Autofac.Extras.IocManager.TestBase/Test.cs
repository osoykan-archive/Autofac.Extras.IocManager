using System.Collections.Generic;
using System.Linq;

using Moq;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace Autofac.Extras.IocManager.TestBase
{
    public abstract class Test
    {
        protected Test()
        {
            Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        protected IFixture Fixture { get; }

        protected T A<T>()
        {
            return Fixture.Create<T>();
        }

        protected List<T> Many<T>(int count = 3)
        {
            return Fixture.CreateMany<T>(count).ToList();
        }

        protected T Mock<T>() where T : class
        {
            return new Mock<T>().Object;
        }

        protected T Inject<T>(T instance) where T : class
        {
            Fixture.Inject(instance);
            return instance;
        }

        protected Mock<T> InjectMock<T>() where T : class
        {
            var mock = new Mock<T>();
            Fixture.Inject(mock.Object);
            return mock;
        }
    }
}
