namespace Autofac.Extras.IocManager.Tests.FluentTests.FakeRabbitMQ
{
    public static class FakeRabbitMQBuilderExtensions
    {
        public static IIocBuilder UseRabbitMQ(this IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.Register<IBus, RabbitMQBus>());
            return iocBuilder;
        }
    }
}
