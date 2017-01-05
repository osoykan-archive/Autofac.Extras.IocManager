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

        private interface IMyClass : IMyClass2
        {
        }

        private interface IMyClass2 : IMyClass3<int>
        {
        }

        private interface IMyClass3<T>
        {
        }
    }
}
