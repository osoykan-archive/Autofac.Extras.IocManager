namespace Autofac.Extras.IocManager
{
    public interface IModule
    {
        void Register(IIocBuilder iocBuilder);
    }
}