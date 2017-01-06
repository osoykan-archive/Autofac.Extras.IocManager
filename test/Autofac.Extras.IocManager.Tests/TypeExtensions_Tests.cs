using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class TypeExtensions_Tests
    {
        [Fact]
        public void GetDefaultInterfacesWithSelf_should_work()
        {
            List<Type> interfaces = typeof(MyClass).GetDefaultInterfacesWithSelf().ToList();

            interfaces.ShouldNotBeNull();
            interfaces.Count.ShouldBe(4);
        }

        private class MyClass : IMyClass
        {
        }

        private interface IMyClass : IMyClass<int, string>
        {
        }

        private interface IMyClass<T, T1> : IMyClass<int>
        {
        }

        private interface IMyClass<T>
        {
        }
    }
}
