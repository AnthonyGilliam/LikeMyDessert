using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;

namespace LikeMyDessert.Tests.DependencyInjection
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
        /// Gets the dependency bound to given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDependency<T>()
        {
            return _kernel.Get<T>();
        }

		/// <summary>
		/// Uses dependency injection engine to return instantiated concrete object directly bound to passed in object Type.  
		/// Gets real concrete object.
		/// </summary>
		/// <typeparam name="T">Type of concrete object to return</typeparam>
		/// <returns></returns>
        public static T GetRealDependency<T>()
        {
            try
            {
                T obj = _kernel.Get<T>
                    (
                        metadata => metadata.Has(LikeMyDessertModule.LIVE_DATA_METADATA_KEY)
                        && metadata.Get<bool>(LikeMyDessertModule.LIVE_DATA_METADATA_KEY)
                    );

                return obj;
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    throw new InvalidOperationException(string.Format("No Live Data Dependency for '{0}' could be found."
                                                        + " Check the dependecy injection definitions"
                                                        + " file to see if a Live Data Dependency has been defined."
                                                        , typeof(T).Name));
                }
            
				throw;
            }
        }

		/// <summary>
		/// Uses dependency injection engine to return instantiated concrete object directly bound to passed in object Type.  
		/// Gets mock object.
		/// </summary>
		/// <typeparam name="T">Type of concrete object to return</typeparam>
		/// <returns></returns>
        public static T GetMockDependency<T>()
        {
            try
            {
                T obj = _kernel.Get<T>
                    (
                        metadata => metadata.Has(LikeMyDessertModule.MOCK_DATA_METADATA_KEY)
                        && metadata.Get<bool>(LikeMyDessertModule.MOCK_DATA_METADATA_KEY)
                    );

                return obj;
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    throw new InvalidOperationException(string.Format("No Mock Data Dependency for '{0}' could be found."
                                                        + " Check the dependecy injection definitions"
                                                        + " file to see if a Mock Data Dependency has been defined."
                                                        , typeof(T).Name));
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the concrete binding for generically passed in type
        /// </summary>
        /// <typeparam name="TDependency">The object to be bound</typeparam>
        /// <param name="concreteObject">The concrete implementation of parent object to bind</param>
        public static void BindDependency<T>(T concreteObject)
        {
            _kernel.Bind<T>().ToConstant(concreteObject);
        }
    }
}
