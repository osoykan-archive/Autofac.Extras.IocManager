using System;
using System.Reflection;

namespace Autofac.Extras.IocManager
{
	public class OnConventionalRegisteringEventArgs : EventArgs
	{
		public OnConventionalRegisteringEventArgs(ContainerBuilder containerBuilder, Assembly assembly)
		{
			ContainerBuilder = containerBuilder;
			Assembly = assembly;
		}

		public ContainerBuilder ContainerBuilder { get; set; }

		public Assembly Assembly { get; set; }
	}
}
