using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Ninject;
using Ninject.Modules;

namespace LikeMyDessert.Web.DependencyInjection
{
	public class NinjectResolver : IDependencyResolver
	{
		private static IKernel _kernel;

		public NinjectResolver()
		{
			_kernel = new StandardKernel(new INinjectModule[] { new LikeMyDessertModule() });
			RegisterServices(_kernel);
		}

		public static void RegisterServices(IKernel kernel)
		{
			//Extra bindings (outside of LikeMyDessertModule) go here:
		}

		public object GetService(Type serviceType)
		{
			return _kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return _kernel.GetAll(serviceType);
		}
	}
}