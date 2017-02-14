using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class MultipleRegistration_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void RegisterAssemblyByConvention_should_select_last_registration_as_default_implementation()
        {
            Building(builder => { builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly())); });

            var messageHandler = LocalIocManager.Resolve<IMessageHandler>();
            messageHandler.ShouldBeAssignableTo<ThirdMessageHandler>();

            var messageHandlers = LocalIocManager.Resolve<IEnumerable<IMessageHandler>>();
            messageHandlers.Count().ShouldBe(3);
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

            var messageHandler = LocalIocManager.Resolve<IMessageHandler>();
            messageHandler.ShouldBeAssignableTo<ThirdMessageHandler>();

            var messageHandlers = LocalIocManager.Resolve<IEnumerable<IMessageHandler>>();
            messageHandlers.Count().ShouldBe(3);
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
