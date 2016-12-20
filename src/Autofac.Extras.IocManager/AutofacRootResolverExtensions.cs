namespace Autofac.Extras.IocManager
{
    public static class AutofacRootResolverExtensions
    {
        public static IRootResolver UseIocManager(this IRootResolver resolver)
        {
            IocManager.Instance.Resolver = resolver;
            return resolver;
        }

        public static IRootResolver UseIocManager(this IRootResolver resolver, IocManager iocManager)
        {
            iocManager.Resolver = resolver;
            return resolver;
        }
    }
}
