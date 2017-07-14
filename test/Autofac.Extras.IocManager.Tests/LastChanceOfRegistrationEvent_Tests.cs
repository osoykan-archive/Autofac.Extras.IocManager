using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using Shouldly;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
	public class LastChanceOfRegistrationEvent_Tests : TestBaseWithIocBuilder
	{
		[Fact]
		public void test()
		{
			var dbContexts = new List<Type>();

			Building(builder =>
			{
				builder.RegisterServices(r =>
				{
					r.OnConventionalRegistering += (sender, args) => { };

					r.OnRegistering += (sender, args) =>
					{
						args.ContainerBuilder.RegisterType<CStoveDbContext>().As<IStoveDbContext>().AsSelf();
					};

					r.RegisterAssemblyByConvention(typeof(LastChanceOfRegistrationEvent_Tests).GetTypeInfo().Assembly);
				});
			});

			LocalIocManager.Resolve<CStoveDbContext>().ShouldNotBeNull();
		}
	}

	public interface IStoveDbContext
	{
	}

	public class AStoveDbContext : IStoveDbContext, ITransientDependency
	{
	}

	public class BStoveDbContext : IStoveDbContext, ITransientDependency
	{
	}

	public class CStoveDbContext : IStoveDbContext
	{
	}

	public interface ILastChance
	{
	}

	public class ALastChance : ILastChance
	{
	}

	public class BLastChance : ILastChance
	{
	}
}
