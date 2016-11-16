Autofac.Extras.IocManager
=====================
[![Build status](https://ci.appveyor.com/api/projects/status/udvakwrxb3nhb25d?svg=true)](https://ci.appveyor.com/project/osoykan/autofac-extras-iocmanager) [![NuGet version](https://badge.fury.io/nu/Autofac.Extras.IocManager.svg)](https://badge.fury.io/nu/Autofac.Extras.IocManager)

Autofac.Extras.IocManager allows Autofac Container to be portable. It also provides all container resolve methods and conventional registration mechanism. IocManager is best alternative to Service Locator.

#Usage
```csharp
 var builder = new ContainerBuilder();
 builder.RegisterIocManager();
 IContainer container = builder.Build();
 container.UseIocManager();
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

class SimpleDependency3 : ISimpleDependency3, ILifeTimeScopeDependency {}

interface ISimpleDependency4 {}

class SimpleDependency4 : ISimpleDependency4, IPerRequestDependency {}
```
Registration extension:

```csharp
protected override void Load(ContainerBuilder builder)
{
    builder.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
}
```

After the container build it can use:

 ```csharp
 IocManager.Instance.Resolve<ISimpleDependency3>();
 ```


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
