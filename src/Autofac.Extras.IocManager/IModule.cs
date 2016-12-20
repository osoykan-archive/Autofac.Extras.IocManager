namespace Autofac.Extras.IocManager
{
    /// <summary>
    ///     Uses to register Modules which belong to IocBuilder convention.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        ///     Registers the specified ioc builder.
        /// </summary>
        /// <param name="iocBuilder">The ioc builder.</param>
        void Register(IIocBuilder iocBuilder);
    }
}
