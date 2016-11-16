using System.Reflection;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class ConventionalRegistrationTests
    {
        [Fact]
        public void ConventionalRegistrarShouldWork_WithDefaultInterfaces()
        {
            var localIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(localIocManager);
            builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            builder.Build().UseIocManager(localIocManager);

            var myTransientInstance = localIocManager.Resolve<IMyTransientClass>();
            myTransientInstance.ShouldNotBeNull();
            myTransientInstance = localIocManager.Resolve<MyTransientClass>();
            myTransientInstance.ShouldNotBeNull();

            var mySingletonInstance = localIocManager.Resolve<IMySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();
            mySingletonInstance = localIocManager.Resolve<MySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();

            var myLifeTimeScopeInstance = localIocManager.Resolve<IMyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
            myLifeTimeScopeInstance = localIocManager.Resolve<MyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
        }

        [Fact]
        public void ConventionalRegistrarShouldWork_GenericInterRegistrations()
        {
            var localIocManager = new IocManager();
            ContainerBuilder builder = new ContainerBuilder().RegisterIocManager(localIocManager); ;
            builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            builder.Build().UseIocManager(localIocManager);

            var genericHumanInstance = localIocManager.Resolve<IMyGenericClass<MyTransientClass>>();
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
