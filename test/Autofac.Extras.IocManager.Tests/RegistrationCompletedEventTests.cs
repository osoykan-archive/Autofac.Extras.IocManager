using System;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class RegistrationCompletedEventTests : TestBaseWithIocBuilder
    {
        public RegistrationCompletedEventTests()
        {
            SampleClass sampleClassInstance = null;

            Building(builder =>
            {
                builder.RegisterServices(r => r.RegisterType<SampleClass>());

                builder.RegisterServices(r => r.RegistrationCompleted += (sender, args) =>
                {
                    var scopeResolver = args.Resolver.Resolve<IScopeResolver>();

                    using (IScopeResolver scope = scopeResolver.BeginScope())
                    {
                        sampleClassInstance = scope.Resolve<SampleClass>();
                    }
                });
            });

            sampleClassInstance.DisposeCount.Should().Be(1);
        }

        [Fact]
        public void RegistrationCompletedEvent_Should_Work()
        {
        }

        public class SampleClass : IDisposable
        {
            public int DisposeCount { get; set; }

            public void Dispose()
            {
                DisposeCount++;
            }
        }
    }
}
