using System;

using Moq;

using Xunit;
using Shouldly;

namespace Autofac.Extras.IocManager.Tests
{
    public class ModuleRegistrationTests : TestFor<ModuleRegistration>
    {
        [Fact]
        public void RegisterInvokesRegister()
        {
            // Arrange
            var moduleA = new Mock<IModuleA>();

            // Act
            Sut.Register(moduleA.Object);

            // Assert
            moduleA.Verify(m => m.Register(It.IsAny<IIocBuilder>()), Times.Once());
        }

        [Fact]
        public void CannotRegisterSameModuleTwice()
        {
            // Arrange
            var moduleA = new Mock<IModuleA>();
            var anotherModuleA = new Mock<IModuleA>();

            // Act
            Sut.Register(moduleA.Object);
            Assert.Throws<ArgumentException>(() => Sut.Register(anotherModuleA.Object));
        }

        [Fact]
        public void RegisterCanRegisterMultipleModules()
        {
            // Arrange
            var moduleA = new Mock<IModuleA>();
            var moduleB = new Mock<IModuleB>();

            // Act
            Sut.Register(moduleA.Object);
            Sut.Register(moduleB.Object);

            // Assert
            Sut.GetModule<IModuleA>().ShouldBe(moduleA.Object);
            Sut.GetModule<IModuleB>().ShouldBe(moduleB.Object);
        }

        [Fact]
        public void GetModuleThrowsExceptionForUnknownModule()
        {
            // Arrange
            var aCompletlyDifferentModule = new Mock<IModuleA>();
            Sut.Register(aCompletlyDifferentModule.Object);

            // Act
            Assert.Throws<ArgumentException>(() => Sut.GetModule<IModuleB>());
        }

        [Fact]
        public void TryGetModuleReturnsFalseForUnknownModule()
        {
            // Arrange
            var aCompletlyDifferentModule = new Mock<IModuleA>();
            Sut.Register(aCompletlyDifferentModule.Object);
            IModuleB moduleB;

            // Act
            Sut.TryGetModule(out moduleB).ShouldBeFalse();
        }

        [Fact]
        public void TryGetModuleReturnsModuleAndTrueForKnownModule()
        {
            // Arrange
            var moduleA = new Mock<IModuleA>();
            Sut.Register(moduleA.Object);
            IModuleA fetchedModuleA;

            // Act
            Sut.TryGetModule(out fetchedModuleA).ShouldBeTrue();

            // Assert
            fetchedModuleA.ShouldBe(moduleA.Object);
        }

        public interface IModuleA : IModule
        {
        }

        public interface IModuleB : IModule
        {
        }
    }
}
