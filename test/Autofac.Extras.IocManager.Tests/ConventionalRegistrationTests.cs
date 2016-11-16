using System.Reflection;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ConventionalRegistrationTests : TestBase
    {
        [Fact]
        public void ConventionalRegistrarShouldWork_WithDefaultInterfaces()
        {
            Builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Builder.Build().UseIocManager(LocalIocManager);

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
            var LocalIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(LocalIocManager);
            ;
            builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            builder.Build().UseIocManager(LocalIocManager);

            var genericHumanInstance = LocalIocManager.Resolve<IMyGenericClass<MyTransientClass>>();
            genericHumanInstance.Object.ShouldBeAssignableTo(typeof(MyTransientClass));
        }

        internal class MyTransientClass : IMyTransientClass, ILifeTimeScopeDependency {}

        internal interface IMyTransientClass {}

        internal class MySingletonClass : IMySingletonClass, ILifeTimeScopeDependency {}

        internal interface IMySingletonClass {}

        internal class MyLifeTimeScopeClass : IMyLifeTimeScopeClass, ILifeTimeScopeDependency {}

        internal interface IMyLifeTimeScopeClass {}

        internal class MyGenericClass<T> : IMyGenericClass<T>, ILifeTimeScopeDependency
        {
            public T Object { get; set; }
        }

        internal interface IMyGenericClass<T>
        {
            T Object { get; set; }
        }
    }
}
