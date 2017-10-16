using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ConventionalRegistration_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ConventionalRegistrarShouldWork_WithDefaultInterfaces()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))); });

            var myTransientInstance = The<IMyTransientClass>();
            myTransientInstance.Should().NotBeNull();
            myTransientInstance = The<MyTransientClass>();
            myTransientInstance.Should().NotBeNull();

            var mySingletonInstance = The<IMySingletonClass>();
            mySingletonInstance.Should().NotBeNull();
            mySingletonInstance = The<MySingletonClass>();
            mySingletonInstance.Should().NotBeNull();

            var myLifeTimeScopeInstance = The<IMyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.Should().NotBeNull();
            myLifeTimeScopeInstance = The<MyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.Should().NotBeNull();
        }

        [Fact]
        public void ConventionalRegistrarShouldWork_GenericInterfaceRegistrations()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))); });

            var genericHumanInstance = The<IMyGenericClass<MyTransientClass>>();
            genericHumanInstance.Object.Should().BeAssignableTo(typeof(MyTransientClass));
        }

        internal class MyTransientClass : IMyTransientClass, ITransientDependency
        {
        }

        internal interface IMyTransientClass
        {
        }

        internal class MySingletonClass : IMySingletonClass, ISingletonDependency
        {
        }

        internal interface IMySingletonClass
        {
        }

        internal class MyLifeTimeScopeClass : IMyLifeTimeScopeClass, ILifetimeScopeDependency
        {
        }

        internal interface IMyLifeTimeScopeClass
        {
        }

        internal class MyGenericClass<T> : IMyGenericClass<T>, ITransientDependency
        {
            public T Object { get; set; }
        }

        internal interface IMyGenericClass<T>
        {
            T Object { get; set; }
        }
    }
}
