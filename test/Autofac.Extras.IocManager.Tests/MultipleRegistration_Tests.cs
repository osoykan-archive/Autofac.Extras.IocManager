using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class MultipleRegistration_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void RegisterAssemblyByConvention_should_select_last_registration_as_default_implementation()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.Load(new AssemblyName("Autofac.Extras.IocManager.Tests")))); });

            var messageHandler = The<IMessageHandler>();
            messageHandler.Should().BeAssignableTo<ThirdMessageHandler>();

            var messageHandlers = The<IEnumerable<IMessageHandler>>();
            messageHandlers.Count().Should().Be(3);
        }

        [Fact]
        public void should_select_last_registration_as_default_implementation()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.Register<IMessageHandler, FirstMessageHandler>();
                    r.Register<IMessageHandler, SecondMessageHandler>();
                    r.Register<IMessageHandler, ThirdMessageHandler>();
                });
            });

            var messageHandler = The<IMessageHandler>();
            messageHandler.Should().BeAssignableTo<ThirdMessageHandler>();

            var messageHandlers = The<IEnumerable<IMessageHandler>>();
            messageHandlers.Count().Should().Be(3);
        }

        public interface IMessageHandler : ITransientDependency
        {
        }

        public class FirstMessageHandler : IMessageHandler
        {
        }

        public class SecondMessageHandler : IMessageHandler
        {
        }

        public class ThirdMessageHandler : IMessageHandler
        {
        }
    }
}
