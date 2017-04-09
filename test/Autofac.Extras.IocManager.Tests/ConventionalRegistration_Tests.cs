using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

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
            myTransientInstance.ShouldNotBeNull();
            myTransientInstance = The<MyTransientClass>();
            myTransientInstance.ShouldNotBeNull();

            var mySingletonInstance = The<IMySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();
            mySingletonInstance = The<MySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();

            var myLifeTimeScopeInstance = The<IMyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
            myLifeTimeScopeInstance = The<MyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
        }

        [Fact]
        public void ConventionalRegistrarShouldWork_GenericInterfaceRegistrations()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))); });

            var genericHumanInstance = The<IMyGenericClass<MyTransientClass>>();
            genericHumanInstance.Object.ShouldBeAssignableTo(typeof(MyTransientClass));
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
