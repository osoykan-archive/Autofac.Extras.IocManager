namespace Autofac.Extras.IocManager
{
    public static class AutofacRootResolverExtensions
    {
        public static IRootResolver UseIocManager(this IRootResolver resolver)
        {
            resolver.Container.UseIocManager();
            return resolver;
        }

        public static IRootResolver UseIocManager(this IRootResolver resolver, IocManager iocManager)
        {
            resolver.Container.UseIocManager(iocManager);
            return resolver;
        }
    }
}
