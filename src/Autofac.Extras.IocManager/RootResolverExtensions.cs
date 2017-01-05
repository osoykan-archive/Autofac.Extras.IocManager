namespace Autofac.Extras.IocManager
{
    public static class RootResolverExtensions
    {
        /// <summary>
        ///     Enables and uses the Ioc Manager.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static IRootResolver UseIocManager(this IRootResolver resolver)
        {
            IocManager.Instance.Resolver = resolver;
            return resolver;
        }

        /// <summary>
        ///     Enables and uses the Ioc Manager with provided <see cref="IocManager" />
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="iocManager">The ioc manager.</param>
        /// <returns></returns>
        public static IRootResolver UseIocManager(this IRootResolver resolver, IIocManager iocManager)
        {
            iocManager.Resolver = resolver;
            return resolver;
        }
    }
}
