using System.Reflection;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class GenericTests : TestBaseWithIocBuilder
    {
        [Fact]
        public void GenericTests_Should_Work()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())); });

            var myGenericResoulition = LocalIocManager.Resolve<IRepository<MyClass, int>>();
        }

        public interface IRepository : ITransientDependency
        {
        }

        public interface IRepository<TEntity, TPrimaryKey> : IRepository
            where TEntity : class
        {
            TPrimaryKey Key { get; set; }

            TEntity Entity { get; set; }
        }

        public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
        {
            public TPrimaryKey Key { get; set; }

            public TEntity Entity { get; set; }
        }

        public class MyClass
        {
        }
    }
}
