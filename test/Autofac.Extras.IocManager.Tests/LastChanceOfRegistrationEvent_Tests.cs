using System.Reflection;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
	public class LastChanceOfRegistrationEvent_Tests : TestBaseWithIocBuilder
	{
		[Fact]
		public void test()
		{
			Building(builder =>
			{
				builder.RegisterServices(r =>
				{
					r.OnConventionalRegistering += (sender, args) => { };

					r.OnRegistering += (sender, args) => { args.ContainerBuilder.RegisterType<CStoveDbContext>().As<IStoveDbContext>().AsSelf(); };

					r.RegisterAssemblyByConvention(typeof(LastChanceOfRegistrationEvent_Tests).GetTypeInfo().Assembly);
				});
			});

			LocalIocManager.Resolve<CStoveDbContext>().Should().NotBeNull();
		}

		[Fact]
		public void before_registration_completed_should_register_last_chance()
		{
			Building(builder =>
			{
				builder.RegisterServices(r =>
				{
					r.BeforeRegistrationCompleted += (sender, args) =>
					{
						args.ContainerBuilder.RegisterType<CStoveDbContext>().As<IStoveDbContext>().AsSelf();
					};
				});
			});

			LocalIocManager.Resolve<CStoveDbContext>().Should().NotBeNull();
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
