using System;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
    public class DecoratorPropagate_Tests : TestBaseWithIocBuilder
    {
        [Fact]
        public void parameters_should_propagate()
        {
            Building(builder =>
            {
                builder.RegisterServices(r => r.Register<IServiceA, ServiceAImpl>());
                builder.RegisterServices(r => r.Register<IServiceB, ServiceBImpl>());
                builder.RegisterServices(r => r.Register<IServiceC, ServiceCImpl>());
                builder.RegisterServices(r => r.Decorate<IServiceC>((context, c) => new ServiceCDecorator(c)));
            });

            var serviceCDecorator = The<Func<IServiceA, IServiceB, IServiceC>>();
            IServiceC serviceC = serviceCDecorator(new ServiceAImpl(), new ServiceBImpl());

            serviceC.ShouldBeOfType(typeof(ServiceCDecorator));
        }

        internal interface IServiceA
        {
        }

        internal class ServiceAImpl : IServiceA
        {
            public ServiceAImpl()
            {
                Id = Guid.NewGuid();
            }

            public Guid Id { get; }
        }

        internal interface IServiceB
        {
        }

        internal class ServiceBImpl : IServiceB
        {
            public ServiceBImpl()
            {
                Id = Guid.NewGuid();
            }

            public Guid Id { get; }
        }

        internal interface IServiceC
        {
        }

        internal class ServiceCImpl : IServiceC
        {
            private readonly IServiceA _serviceA;
            private readonly IServiceB _serviceB;

            public ServiceCImpl(IServiceA serviceA, IServiceB serviceB)
            {
                if (serviceA == null)
                {
                    throw new ArgumentNullException(nameof(serviceA));
                }
                if (serviceB == null)
                {
                    throw new ArgumentNullException(nameof(serviceB));
                }

                _serviceA = serviceA;
                _serviceB = serviceB;
            }
        }

        internal class ServiceCDecorator : IServiceC
        {
            private readonly IServiceC _inner;

            public ServiceCDecorator(IServiceC inner)
            {
                if (inner == null)
                {
                    throw new ArgumentNullException(nameof(inner));
                }

                _inner = inner;
            }
        }
    }
}
