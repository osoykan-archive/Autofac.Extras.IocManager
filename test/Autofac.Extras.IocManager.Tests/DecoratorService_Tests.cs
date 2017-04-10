using NSubstitute;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class DecoratorService_Tests
    {
        private readonly IResolverContext _resolverContextMock;
        public DecoratorService Sut;

        public DecoratorService_Tests()
        {
            _resolverContextMock = Substitute.For<IResolverContext>();
            Sut = new DecoratorService();
        }

        [Fact]
        public void NoDecoratorReturnsSame()
        {
            var instance = Sut.Decorate<IMagicInterface>(new MagicClass(), _resolverContextMock);

            instance.ShouldNotBeNull();
            instance.ShouldBeAssignableTo<MagicClass>();
        }

        [Fact]
        public void WithTwoDecoratorsWithGeneric()
        {
            Sut.AddDecorator<IMagicInterface>((r, s) => new MagicClassDecorator1(s));
            Sut.AddDecorator<IMagicInterface>((r, s) => new MagicClassDecorator2(s));

            var instance = Sut.Decorate<IMagicInterface>(new MagicClass(), _resolverContextMock);

            instance.ShouldBeAssignableTo<MagicClassDecorator2>();
            var magicClassDecorator2 = (MagicClassDecorator2)instance;
            magicClassDecorator2.Inner.ShouldBeAssignableTo<MagicClassDecorator1>();
            var magicClassDecorator1 = (MagicClassDecorator1)magicClassDecorator2.Inner;
            magicClassDecorator1.Inner.ShouldBeAssignableTo<MagicClass>();
        }

        [Fact]
        public void WithTwoDecoratorsWithTyped()
        {
            Sut.AddDecorator<IMagicInterface>((r, s) => new MagicClassDecorator1(s));
            Sut.AddDecorator<IMagicInterface>((r, s) => new MagicClassDecorator2(s));

            object instance = Sut.Decorate(typeof(IMagicInterface), new MagicClass(), _resolverContextMock);

            instance.ShouldBeAssignableTo<MagicClassDecorator2>();
            var magicClassDecorator2 = (MagicClassDecorator2)instance;
            magicClassDecorator2.Inner.ShouldBeAssignableTo<MagicClassDecorator1>();
            var magicClassDecorator1 = (MagicClassDecorator1)magicClassDecorator2.Inner;
            magicClassDecorator1.Inner.ShouldBeAssignableTo<MagicClass>();
        }

        private interface IMagicInterface
        {
        }

        private class MagicClass : IMagicInterface
        {
        }

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
