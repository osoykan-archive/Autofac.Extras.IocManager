using System;
using System.Reflection;

using Autofac.Core;
using Autofac.Extras.IocManager.TestBase;

using Castle.DynamicProxy;

using Xunit;

namespace Autofac.Extras.IocManager.DynamicProxy.Tests
{
    public class GenericRepository_Interception_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void ShouldWork()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.UseBuilder(cb => cb.RegisterCallback(registry => registry.Registered += RegistryOnRegistered)));
                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
                builder.RegisterServices(r => r.RegisterGeneric(typeof(IMyModuleRepository<,>), typeof(MyModuleRepositoryBase<,>)));
                builder.RegisterServices(r => r.RegisterGeneric(typeof(IMyModuleRepository<>), typeof(MyModuleRepositoryBase<>)));
                builder.RegisterServices(r => r.RegisterGeneric(typeof(IRepository<,>), typeof(EfRepositoryBase<,>)));
            });

            var myGenericResoulition = The<IRepository<MyClass, int>>();
        }

        private void RegistryOnRegistered(object sender, ComponentRegisteredEventArgs args)
        {
            Type implType = args.ComponentRegistration.Activator.LimitType;

            if (typeof(IRepository).IsAssignableFrom(implType))
            {
                args.ComponentRegistration.InterceptedBy<UnitOfWorkInterceptor>();
            }
        }

        public interface IRepository : ITransientDependency
        {
        }

        public interface IRepository<TEntity, TPrimaryKey> : IRepository
            where TEntity : class
        {
        }

        public interface IRepository<TEntity> : IRepository<TEntity, int>
            where TEntity : class
        {
        }

        public abstract class StoveRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
            where TEntity : class
        {
        }

        public class EfRepositoryBase<TEntity, TPrimaryKey> : StoveRepositoryBase<TEntity, TPrimaryKey>
            where TEntity : class
        {
        }

        public class EfRepositoryBase<TEntity> : StoveRepositoryBase<TEntity, int>
            where TEntity : class
        {
        }

        public interface IMyModuleRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
            where TEntity : class
        {
        }

        public interface IMyModuleRepository<TEntity> : IMyModuleRepository<TEntity, int>
            where TEntity : class
        {
        }

        public class MyModuleRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<TEntity, TPrimaryKey>, IMyModuleRepository<TEntity, TPrimaryKey>
            where TEntity : class
        {
        }

        public class MyModuleRepositoryBase<TEntity> : MyModuleRepositoryBase<TEntity, int>, IMyModuleRepository<TEntity>
            where TEntity : class
        {
        }

        public class MyClass
        {
        }

        public class MyContext
        {
        }

        public class UnitOfWorkInterceptor : IInterceptor, ITransientDependency
        {
            public void Intercept(IInvocation invocation)
            {
                MethodInfo method = invocation.Method;
            }
        }
    }
}
