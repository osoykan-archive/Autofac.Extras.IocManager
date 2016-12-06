using System.Reflection;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ConventionalRegistrationTests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ConventionalRegistrarShouldWork_WithDefaultInterfaces()
        {
            Building(builder =>
                     {
                         builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                     });

            var myTransientInstance = LocalIocManager.Resolve<IMyTransientClass>();
            myTransientInstance.ShouldNotBeNull();
            myTransientInstance = LocalIocManager.Resolve<MyTransientClass>();
            myTransientInstance.ShouldNotBeNull();

            var mySingletonInstance = LocalIocManager.Resolve<IMySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();
            mySingletonInstance = LocalIocManager.Resolve<MySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();

            var myLifeTimeScopeInstance = LocalIocManager.Resolve<IMyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
            myLifeTimeScopeInstance = LocalIocManager.Resolve<MyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
        }

        [Fact]
        public void ConventionalRegistrarShouldWork_GenericInterRegistrations()
        {
            Building(builder => { builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()); });

            var genericHumanInstance = LocalIocManager.Resolve<IMyGenericClass<MyTransientClass>>();
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
