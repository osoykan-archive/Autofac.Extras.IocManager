using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

public class ModuleRegistration : IModuleRegistration
{
    /// <summary>
    ///     The modules
    /// </summary>
    private readonly Dictionary<Type, IModule> _modules = new Dictionary<Type, IModule>();

    /// <summary>
    ///     The ioc builder
    /// </summary>
    private readonly IIocBuilder iocBuilder;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ModuleRegistration" /> class.
    /// </summary>
    /// <param name="iocBuilder">The ioc builder.</param>
    public ModuleRegistration(IocBuilder iocBuilder)
    {
        this.iocBuilder = iocBuilder;
    }

    /// <summary>
    ///     Registers this instance.
    /// </summary>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    public void Register<TModule>()
        where TModule : IModule, new()
    {
        var module = new TModule();
        Register(module);
    }

    /// <summary>
    ///     Registers the specified module.
    /// </summary>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    /// <param name="module">The module.</param>
    /// <exception cref="System.ArgumentException"></exception>
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

    /// <summary>
    ///     Gets the module.
    /// </summary>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
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

    /// <summary>
    ///     Tries the get module.
    /// </summary>
    /// <typeparam name="TModule">The type of the module.</typeparam>
    /// <param name="module">The module.</param>
    /// <returns></returns>
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
