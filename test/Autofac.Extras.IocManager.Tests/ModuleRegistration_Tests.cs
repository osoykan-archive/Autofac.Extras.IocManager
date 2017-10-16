using System;

using FakeItEasy;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ModuleRegistration_Tests
    {
        public ModuleRegistration Sut;

        public ModuleRegistration_Tests()
        {
            Sut = new ModuleRegistration(A.Fake<IIocBuilder>());
        }

        [Fact]
        public void RegisterInvokesRegister()
        {
            // Arrange
            var moduleA = Substitute.For<IModule>();

            // Act
            Sut.Register(moduleA);

            // Assert
            moduleA.Received(1).Register(Arg.Any<IIocBuilder>());
        }

        [Fact]
        public void CannotRegisterSameModuleTwice()
        {
            // Arrange
            var moduleA = Substitute.For<IModuleA>();
            var anotherModuleA = Substitute.For<IModuleA>();

            // Act
            Sut.Register(moduleA);
            Assert.Throws<ArgumentException>(() => Sut.Register(anotherModuleA));
        }

        [Fact]
        public void RegisterCanRegisterMultipleModules()
        {
            // Arrange
            var moduleA = Substitute.For<IModuleA>();
            var moduleB = Substitute.For<IModuleB>();

            // Act
            Sut.Register(moduleA);
            Sut.Register(moduleB);

            // Assert
            Sut.GetModule<IModuleA>().Should().Be(moduleA);
            Sut.GetModule<IModuleB>().Should().Be(moduleB);
        }

        [Fact]
        public void GetModuleThrowsExceptionForUnknownModule()
        {
            // Arrange
            var aCompletlyDifferentModule = Substitute.For<IModuleA>();
            Sut.Register(aCompletlyDifferentModule);

            // Act
            Assert.Throws<ArgumentException>(() => Sut.GetModule<IModuleB>());
        }

        [Fact]
        public void TryGetModuleReturnsFalseForUnknownModule()
        {
            // Arrange
            var aCompletlyDifferentModule = Substitute.For<IModuleA>();
            Sut.Register(aCompletlyDifferentModule);
            IModuleB moduleB;

            // Act
            Sut.TryGetModule(out moduleB).Should().BeFalse();
        }

        [Fact]
        public void TryGetModuleReturnsModuleAndTrueForKnownModule()
        {
            // Arrange
            var moduleA = Substitute.For<IModuleA>();
            Sut.Register(moduleA);
            IModuleA fetchedModuleA;

            // Act
            Sut.TryGetModule(out fetchedModuleA).Should().BeTrue();

            // Assert
            fetchedModuleA.Should().Be(moduleA);
        }

        public interface IModuleA : IModule
        {
        }

        public interface IModuleB : IModule
        {
        }
    }
}
