using System;

using Shouldly;

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

            _manager.ShutdownCount.ShouldBe(1);
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

                                                           throw new ApplicationException();
                                                       };
                                                   })
                                                   .CreateResolver();

            Action act = () => rootResolver.Dispose();
            act.ShouldThrow<ApplicationException>();

            Action containerDisposedAction = () => rootResolver.Container.Resolve<ISomeManager>();
            containerDisposedAction.ShouldThrow<ObjectDisposedException>();

            Action rootResolverDisposedAction = () => rootResolver.Resolve<ISomeManager>();
            rootResolverDisposedAction.ShouldThrow<ObjectDisposedException>();

            manager.ShutdownCount.ShouldBe(1);
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
