using System;
using System.Data.Common;

using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Autofac.Extras.IocManager.MsDependencyInjection.Tests
{
	public class ServiceProvider_Tests : TestBaseWithIocBuilder
	{
		public ServiceProvider_Tests()
		{
			var services = new ServiceCollection();

			Building(builder =>
			{
				builder.RegisterServices(r =>
				{
					r.UseBuilder(cb =>
					{
						cb.RegisterInstance(new StoveDbContextConfigurerAction<ILifetimeScope>(configuration => { configuration.ConnectionString = "asd"; }))
						  .As<IStoveDbContextConfigurer<ILifetimeScope>>()
						  .AsImplementedInterfaces()
						  .SingleInstance();

						cb.RegisterInstance(new StoveDbContextConfigurerAction<ContextBoundObject>(configuration => { configuration.ConnectionString = "b"; }))
						  .As<IStoveDbContextConfigurer<ContextBoundObject>>()
						  .AsImplementedInterfaces()
						  .SingleInstance();
					});

					r.BeforeRegistrationCompleted += (sender, args) => { args.ContainerBuilder.Populate(services); };
				});
			});
		}

		[Fact]
		public void serviceprovider_should_resolve_lambda()
		{
			var serviceProvider = new AutofacServiceProvider(LocalIocManager.Resolver.Container);

			using (ILifetimeScope beginLifetimeScope = serviceProvider.GetService<ILifetimeScope>().BeginLifetimeScope(
				"message",
				ctx =>
				{
					ctx.RegisterType<UserCreatedConsumer>().AsSelf().InstancePerDependency();
				}))
			{
				beginLifetimeScope.Resolve<UserCreatedConsumer>().Consume();
			}
		}
	}

	public class UserCreatedConsumer
	{
		private readonly IScopeResolver _scope;

		public UserCreatedConsumer(IScopeResolver scope)
		{
			_scope = scope;
		}

		public void Consume()
		{
			var conf = new StoveDbContextConfiguration<ContextBoundObject>("A", null);
			_scope.Resolve<IStoveDbContextConfigurer<ContextBoundObject>>().Configure(conf);
			conf.ConnectionString.Should().Be("b");
		}
	}

	public class StoveDbContextConfigurerAction<TDbContext> : IStoveDbContextConfigurer<TDbContext>
	{
		public StoveDbContextConfigurerAction(Action<StoveDbContextConfiguration<TDbContext>> action)
		{
			Action = action;
		}

		public Action<StoveDbContextConfiguration<TDbContext>> Action { get; set; }

		public void Configure(StoveDbContextConfiguration<TDbContext> configuration)
		{
			Action(configuration);
		}
	}

	public interface IStoveDbContextConfigurer<TDbContext>
	{
		void Configure(StoveDbContextConfiguration<TDbContext> configuration);
	}

	public class StoveDbContextConfiguration<TDbContext>
	{
		public StoveDbContextConfiguration(string connectionString, DbConnection existingConnection)
		{
			ConnectionString = connectionString;
			ExistingConnection = existingConnection;
		}

		public string ConnectionString { get; internal set; }

		public DbConnection ExistingConnection { get; internal set; }
	}
}
