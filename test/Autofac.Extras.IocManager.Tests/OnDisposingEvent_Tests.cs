using System;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class OnDisposingEvent_Tests
    {
        private readonly IRootResolver _rootResolver;
        private ISomeManager _manager;

        public OnDisposingEvent_Tests()
        {
            _rootResolver = IocBuilder.New
                                      .UseAutofacContainerBuilder()
                                      .RegisterServices(r =>
                                      {
                                          r.Register<ISomeManager, SomeManager>();
                                          r.OnDisposing += (sender, args) =>
                                          {
                                              _manager = args.Context.Resolver.Resolve<ISomeManager>();
                                              _manager.Shutdown();
                                          };
                                      })
                                      .CreateResolver();
        }

        [Fact]
        public void dispose_event_should_fire_when_rootresolver_is_disposed()
        {
            _rootResolver.Dispose();

            _manager.ShutdownCount.Should().Be(1);
        }

        [Fact]
        public void if_any_exception_appears_on_disposing_events_root_container_should_be_dispose_finally()
        {
            ISomeManager manager = null;
            IRootResolver rootResolver = IocBuilder.New
                                                   .UseAutofacContainerBuilder()
                                                   .RegisterServices(r =>
                                                   {
                                                       r.Register<ISomeManager, SomeManager>();
                                                       r.OnDisposing += (sender, args) =>
                                                       {
                                                           manager = args.Context.Resolver.Resolve<ISomeManager>();
                                                           manager.Shutdown();

                                                           throw new HandlerException();
                                                       };
                                                   })
                                                   .CreateResolver();

            Action act = () => rootResolver.Dispose();
            act.Should().Throw<HandlerException>();

            Action containerDisposedAction = () => rootResolver.Container.Resolve<ISomeManager>();
            containerDisposedAction.Should().Throw<ObjectDisposedException>();

            Action rootResolverDisposedAction = () => rootResolver.Resolve<ISomeManager>();
            rootResolverDisposedAction.Should().Throw<ObjectDisposedException>();

            manager.ShutdownCount.Should().Be(1);
        }

        public class HandlerException : Exception
        {
        }

        public interface ISomeManager
        {
            int ShutdownCount { get; set; }

            void Shutdown();
        }

        public class SomeManager : ISomeManager
        {
            public int ShutdownCount { get; set; }

            public void Shutdown()
            {
                ShutdownCount++;
            }
        }
    }
}
