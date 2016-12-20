using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class AutofacServiceRegistrationTests : Test
    {
        private readonly AutofacServiceRegistration _sut;

        public AutofacServiceRegistrationTests()
        {
            _sut = new AutofacServiceRegistration();
        }

        [Fact]
        public void ServiceViaFactory()
        {
            _sut.Register<IMagicInterface>(r => new MagicClass());

            Assert_Service();
        }

        [Fact]
        public void ServiceViaGeneric()
        {
            _sut.Register<IMagicInterface, MagicClass>();

            Assert_Service();
        }

        [Fact]
        public void ServiceViaType()
        {
            _sut.Register(typeof(IMagicInterface), typeof(MagicClass));

            Assert_Service();
        }

        public void Assert_Service()
        {
            IRootResolver resolver = _sut.CreateResolver();
            var magicInterface = resolver.Resolve<IMagicInterface>();

            magicInterface.ShouldNotBeNull();
            magicInterface.ShouldBeAssignableTo<MagicClass>();
        }

        [Fact]
        public void DecoratorViaFactory()
        {
            _sut.Register<IMagicInterface>(r => new MagicClass());

            Assert_Decorator();
        }

        [Fact]
        public void DecoratorViaGeneric()
        {
            _sut.Register<IMagicInterface, MagicClass>();

            Assert_Decorator();
        }

        [Fact]
        public void DecoratorViaType()
        {
            _sut.Register(typeof(IMagicInterface), typeof(MagicClass));

            Assert_Decorator();
        }

        public void Assert_Decorator()
        {
            // The order should be like this (like unwrapping a present with the order of
            // wrapping paper applied)
            // 
            // Call      MagicClassDecorator2
            // Call      MagicClassDecorator1
            // Call      MagicClass
            // Return to MagicClassDecorator1
            // Return to MagicClassDecorator2

            _sut.Decorate<IMagicInterface>((r, inner) => new MagicClassDecorator1(inner));
            _sut.Decorate<IMagicInterface>((r, inner) => new MagicClassDecorator2(inner));

            IRootResolver resolver = _sut.CreateResolver();
            var magic = resolver.Resolve<IMagicInterface>();

            magic.ShouldBeAssignableTo<MagicClassDecorator2>();
            var magicClassDecorator2 = (MagicClassDecorator2)magic;
            magicClassDecorator2.Inner.ShouldBeAssignableTo<MagicClassDecorator1>();
            var magicClassDecorator1 = (MagicClassDecorator1)magicClassDecorator2.Inner;
            magicClassDecorator1.Inner.ShouldBeAssignableTo<MagicClass>();
        }

        private interface IMagicInterface {}

        private class MagicClass : IMagicInterface {}

        private class MagicClassDecorator1 : IMagicInterface
        {
            public MagicClassDecorator1(IMagicInterface magicInterface)
            {
                Inner = magicInterface;
            }

            public IMagicInterface Inner { get; }
        }

        private class MagicClassDecorator2 : IMagicInterface
        {
            public MagicClassDecorator2(IMagicInterface magicInterface)
            {
                Inner = magicInterface;
            }

            public IMagicInterface Inner { get; }
        }
    }
}
