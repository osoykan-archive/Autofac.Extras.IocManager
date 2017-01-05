Autofac.Extras.IocManager
=====================
[![Build status](https://ci.appveyor.com/api/projects/status/udvakwrxb3nhb25d?svg=true)](https://ci.appveyor.com/project/osoykan/autofac-extras-iocmanager) [![NuGet version](https://badge.fury.io/nu/Autofac.Extras.IocManager.svg)](https://badge.fury.io/nu/Autofac.Extras.IocManager)

Autofac.Extras.IocManager allows Autofac Container to be portable. It also provides entire resolve methods which belong to Autofac Container and also provides conventional registration mechanism. IocManager is the best alternative to [common Service Locator anti-pattern](http://blog.ploeh.dk/2010/02/03/ServiceLocatorisanAnti-Pattern/).

#Sample Usage
```csharp
IRootResolver resolver = IocBuilder.New
                                   .UseAutofacContainerBuilder()
                                   .UseStove()
                                   .UseStoveEntityFramework()
                                   .UseDefaultEventBus()
                                   .UseDbContextEfTransactionStrategy()
                                   .UseTypedConnectionStringResolver()
                                   .UseNLog()
                                   .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                   .RegisterIocManager()
                                   .CreateResolver()
                                   .UseIocManager();
                                   
var someDomainService = resolver.Resolve<SomeDomainService>();
someDomainService.DoSomeStuff();
 ```
 

##Extension Oriented Registrations
 
 Extension sample:
 
 ```csharp
public static class StoveRegistrationExtensions
{
    public static IIocBuilder UseStove(this IIocBuilder builder)
    {
        RegisterDefaults(builder);
        return builder;
    }

    private static void RegisterDefaults(IIocBuilder builder)
    {
        builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
        builder.RegisterServices(r => r.Register<IGuidGenerator>(context => SequentialGuidGenerator.Instance));
        builder.RegisterServices(r => r.Register<IStoveStartupConfiguration, StoveStartupConfiguration>(Lifetime.Singleton));
    }

    public static IIocBuilder UseDefaultConnectionStringResolver(this IIocBuilder builder)
    {
        builder.RegisterServices(r => r.Register<IConnectionStringResolver, DefaultConnectionStringResolver>());
        return builder;
    }

    public static IIocBuilder UseDefaultEventBus(this IIocBuilder builder)
    {
        builder.RegisterServices(r => r.Register<IEventBus>(context => EventBus.Default));
        return builder;
    }

    public static IIocBuilder UseEventBus(this IIocBuilder builder)
    {
        builder.RegisterServices(r => r.Register<IEventBus, EventBus>());
        return builder;
    }
}
 ```
 
##Registrations
###Conventional assembly registrations

Interface and classes:
```csharp
interface ISimpleDependency1 {}

class SimpleDependency1 : ISimpleDependency1, ITransientDependency {}

interface ISimpleDependency2 {}

class SimpleDependency2 : ISimpleDependency2, ISingletonDependency {}

interface ISimpleDependency3 {}

class SimpleDependency3 : ISimpleDependency3, ILifetimeScopeDependency {}
```

##IocManager Using

After the container build which means `CreateResolver()` it can use:

 ```csharp
 IocManager.Instance.Resolve<ISimpleDependency3>();
 ```
 With this feature, you no longer don't have to define your registrations in `Builder`'s `Load` method explicitly. `RegisterAssemblyByConvention` does this with [interface marking pattern](https://en.wikipedia.org/wiki/Marker_interface_pattern).

##Resolvings
 Resolve instance:
 ```csharp
 IocManager.Instance.Resolve<IMyTransientClass>();
 ```
 
Disposable resolve to avoid memory leaks:
 ```csharp
SimpleDisposableDependency simpleDisposableDependency;
using (var simpleDependencyWrapper = IocManager.Instance.ResolveAsDisposable<SimpleDisposableDependency>())
{
    simpleDisposableDependency = simpleDependencyWrapper.Object;
}

simpleDisposableDependency.DisposeCount.ShouldBe(1);
```

Scoped resolver to avoid memory leaks:
```csharp
SimpleDisposableDependency simpleDisposableDependency;
using (IIocScopedResolver iocScopedResolver = IocManager.Instance.CreateScope())
{
    simpleDisposableDependency = iocScopedResolver.Resolve<SimpleDisposableDependency>();
}

simpleDisposableDependency.DisposeCount.ShouldBe(1);
```

##Property Injection
Autofac.Extras.IocManager also provides property injection on public and private properties with `InjectPropertiesAsAutowired()` extension.

```csharp
builder.RegisterType<ISimpleDependency>().AsSelf().AsImplementedInterfaces().InjectPropertiesAsAutowired();
```

Also you may not want to inject all properties for a dependency, it can be done with putting single attribute to target property.
`DoNotInjectAttribute`

```csharp
 class MySimpleClass : IMySimpleClass, ILifeTimeScopeDependency
 {
     [DoNotInject]
     public IHuman Human { get; set; }
 }
```

##Injectable IocManager
IocManager also self-injectable in any dependencies. For example:

```csharp
class SimpleDependencyWithIocManager
{
    private readonly IIocManager _iocManager;

    public SimpleDependencyWithIocManager(IIocManager iocManager)
    {
        _iocManager = iocManager;
    }

    public IIocManager GetIocManager()
    {
        return _iocManager;
    }

    public void DoSomeStuff()
    {
        _iocManager.Resolve<SomeType>();

        // It would be disposed automatically.
        using (var someType = _iocManager.ResolveAsDisposable<SomeType>())
        {
            someType.Object.DoStuff();
        }

        // All instances would be disposed automatically after the using statement.
        using (IIocScopedResolver iocScopedResolver = _iocManager.CreateScope())
        {
            iocScopedResolver.Resolve<SomeKindOfType>();
            iocScopedResolver.Resolve<HumanClass>();
            iocScopedResolver.Resolve<BirdClass>();
        }
    }
}
```
feel free to use `IIocManager` for resolving operations in any dependency.


#Example

Extension:
```csharp
public static class SomeRegistrationExtensions
{
    public static IIocBuilder UseSomeFeature(this IIocBuilder builder)
    {
        builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
        //do some registrations which belong to the feature
        //...
        return builder;
    }
}
```
Composition Root or Program.cs

```csharp
internal class Program
{
    private static void Main(string[] args)
    {
        IocBuilder.New
                  .UseAutofacContainerBuilder()
                  .UseSomeFeature();
    }
}
```



