using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class AutofacExtensions_Tests
    {
        [Fact]
        public void GetTypedResolvingParameters_Test()
        {
            var obj = new { connectionString = "someString", dbContext = typeof(MyDbContext) };

            IEnumerable<TypedParameter> typedParameters = obj.GetTypedResolvingParameters().ToList();
            typedParameters.Should().NotBeNull();
            typedParameters.Should().BeOfType<List<TypedParameter>>();
            typedParameters.FirstOrDefault(x => x.Type == typeof(string)).Should().NotBeNull();
            typedParameters.FirstOrDefault(x => x.Type == typeof(Type)).Should().NotBeNull();
            typedParameters.FirstOrDefault(x => x.Type == typeof(string)).Value.Should().Be("someString");
            typedParameters.FirstOrDefault(x => x.Type == typeof(Type)).Value.Should().Be(typeof(MyDbContext));
        }

        private class MyDbContext
        {
        }
    }
}
