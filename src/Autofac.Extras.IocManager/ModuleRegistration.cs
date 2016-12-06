using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

public class ModuleRegistration : IModuleRegistration
{
    private readonly Dictionary<Type, IModule> _modules = new Dictionary<Type, IModule>();
    private readonly IocBuilder iocBuilder;

    public ModuleRegistration(IocBuilder iocBuilder)
    {
        this.iocBuilder = iocBuilder;
    }

    public void Register<TModule>()
        where TModule : IModule, new()
    {
        var module = new TModule();
        Register(module);
    }

    public void Register<TModule>(TModule module)
        where TModule : IModule
    {
        Type moduleType = typeof(TModule);
        if (_modules.ContainsKey(moduleType))
        {
            throw new ArgumentException($"Module '{moduleType.PrettyPrint()}' has already been registered");
        }

        module.Register(iocBuilder);
        _modules.Add(moduleType, module);
    }

    public TModule GetModule<TModule>()
        where TModule : IModule
    {
        TModule module;
        if (!TryGetModule(out module))
        {
            throw new ArgumentException($"Module '{typeof(TModule).PrettyPrint()}' is not registered");
        }

        return module;
    }

    public bool TryGetModule<TModule>(out TModule module)
        where TModule : IModule
    {
        Type moduleType = typeof(TModule);
        IModule iModule;
        if (!_modules.TryGetValue(moduleType, out iModule))
        {
            module = default(TModule);
            return false;
        }

        module = (TModule)iModule;
        return true;
    }
}
