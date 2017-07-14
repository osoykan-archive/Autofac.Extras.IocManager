using System;

namespace Autofac.Extras.IocManager
{
	public class BeforeRegistrationCompletedEventArgs : EventArgs
	{
		public BeforeRegistrationCompletedEventArgs(ContainerBuilder containerBuilder)
		{
			this.ContainerBuilder = containerBuilder;
		}

		public ContainerBuilder ContainerBuilder { get; }
	}
}
