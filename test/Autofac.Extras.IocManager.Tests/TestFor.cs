using System;

using Ploeh.AutoFixture;

namespace Autofac.Extras.IocManager.Tests
{
    public abstract class TestFor<TSut> : Test
    {
        private readonly Lazy<TSut> _lazySut;

        protected TestFor()
        {
            _lazySut = new Lazy<TSut>(CreateSut);
        }

        protected TSut Sut => _lazySut.Value;

        protected virtual TSut CreateSut()
        {
            return Fixture.Create<TSut>();
        }
    }
}
