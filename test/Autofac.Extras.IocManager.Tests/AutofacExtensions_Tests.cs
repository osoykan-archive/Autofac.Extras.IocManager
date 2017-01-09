using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

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
            typedParameters.ShouldNotBeNull();
            typedParameters.ShouldBeOfType<List<TypedParameter>>();
            typedParameters.FirstOrDefault(x => x.Type == typeof(string)).ShouldNotBeNull();
            typedParameters.FirstOrDefault(x => x.Type == typeof(Type)).ShouldNotBeNull();
            typedParameters.FirstOrDefault(x => x.Type == typeof(string)).Value.ShouldBe("someString"); ;
            typedParameters.FirstOrDefault(x => x.Type == typeof(Type)).Value.ShouldBe(typeof(MyDbContext)); ;
        }

        private class MyDbContext
        {
        }
    }
}
