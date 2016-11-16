using System.Reflection;

using Autofac.Extras.IocManager.Extensions;

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
            Builder.Build().UseIocManager();

            var myTransientInstance = IocManager.Instance.Resolve<IMyTransientClass>();
            myTransientInstance.ShouldNotBeNull();
            myTransientInstance = IocManager.Instance.Resolve<MyTransientClass>();
            myTransientInstance.ShouldNotBeNull();

            var mySingletonInstance = IocManager.Instance.Resolve<IMySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();
            mySingletonInstance = IocManager.Instance.Resolve<MySingletonClass>();
            mySingletonInstance.ShouldNotBeNull();

            var myLifeTimeScopeInstance = IocManager.Instance.Resolve<IMyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
            myLifeTimeScopeInstance = IocManager.Instance.Resolve<MyLifeTimeScopeClass>();
            myLifeTimeScopeInstance.ShouldNotBeNull();
        }

        [Fact]
        public void ConventionalRegistrarShouldWork_GenericInterRegistrations()
        {
            Builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Builder.Build().UseIocManager();

            var genericHumanInstance = IocManager.Instance.Resolve<IMyGenericClass<MyTransientClass>>();
            genericHumanInstance.Object.ShouldBeAssignableTo(typeof(MyTransientClass));
        }

        internal class MyTransientClass : IMyTransientClass, ITransientDependency {}

        internal interface IMyTransientClass {}

        internal class MySingletonClass : IMySingletonClass, ISingletonDependency {}

        internal interface IMySingletonClass {}

        internal class MyLifeTimeScopeClass : IMyLifeTimeScopeClass, ILifeTimeScopeDependency {}

        internal interface IMyLifeTimeScopeClass {}

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
