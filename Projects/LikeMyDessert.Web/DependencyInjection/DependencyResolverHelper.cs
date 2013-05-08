using System;
using Ninject;

namespace LikeMyDessert.Web.DependencyInjection
{
    public static class DependencyResolverHelper
    {
        private static IKernel _kernel;

		/// <summary>
		/// Initiates dependency injection engine.
		/// </summary>
        static DependencyResolverHelper()
        {
            _kernel = new StandardKernel(new LikeMyDessertModule());
        }

		/// <summary>
		/// Uses dependency injection engine to return instantiated concrete object directly bound to passed in object Type.  
		/// Gets real concrete object.
		/// </summary>
		/// <typeparam name="T">Type of concrete object to return</typeparam>
		/// <returns></returns>
        public static T GetDependency<T>()
        {
            var obj = _kernel.Get<T>();

            return obj;
        }
    }
}
