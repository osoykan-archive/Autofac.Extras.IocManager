namespace Autofac.Extras.IocManager.Tests.FluentTests
{
    public static class FakeNLogBuilderExtensions
    {
        public static IIocBuilder UseNLog(this IIocBuilder iocBuilder)
        {
            iocBuilder.RegisterServices(r => r.Register<ILogger, NLogLogger>());
            return iocBuilder;
        }
    }
}
