using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class TypeExtensions_Tests
    {
        [Fact]
        public void GetDefaultInterfacesWithSelf_should_work()
        {
            List<Type> interfaces = typeof(MyClass).GetDefaultInterfacesWithSelf().ToList();

            interfaces.Should().NotBeNull();
            interfaces.Count.Should().Be(2);
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
