using System;

using Autofac.Extras.IocManager.TestBase;

using FluentAssertions;

using Xunit;

namespace Autofac.Extras.IocManager.Tests
{
	public class Lamda_Tests : TestBaseWithIocBuilder
	{
		public Lamda_Tests()
		{
			Building(builder => { builder.RegisterServices(r => { r.Register<Func<string, string>>(context => (s => s.ToLower())); }); });
		}

		[Fact]
		public void funcs_are_registered()
		{
			The<Func<string, string>>().Should().NotBeNull();

			using (ILifetimeScope scope = The<ILifetimeScope>().BeginLifetimeScope("message", x => x.Register<Func<int, int>>(ctx => (i =>
			{
				i = i * i;
				return i;
			}))))
			{
				scope.Resolve<Func<int, int>>().Invoke(4).Should().Be(16);

				using (ILifetimeScope beginLifetimeScope = scope.BeginLifetimeScope())
				{
					beginLifetimeScope.Resolve<Func<string, string>>().Invoke("A").Should().Be("a");
				}
			}
		}
	}
}
