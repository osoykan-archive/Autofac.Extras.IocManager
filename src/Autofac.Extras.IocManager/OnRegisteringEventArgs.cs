using System;

namespace Autofac.Extras.IocManager
{
	public class OnRegisteringEventArgs
	{
		public OnRegisteringEventArgs(
			ContainerBuilder containerBuilder,
			Type implementationType,
			Type[] serviceTypes,
			Lifetime lifetime)
		{
			ContainerBuilder = containerBuilder;
			ImplementationType = implementationType;
			ServiceTypes = serviceTypes;
			Lifetime = lifetime;
		}

		public ContainerBuilder ContainerBuilder { get; set; }

		public Type ImplementationType { get; set; }

		public Type[] ServiceTypes { get; set; }

		public Lifetime Lifetime { get; set; }
	}
}
