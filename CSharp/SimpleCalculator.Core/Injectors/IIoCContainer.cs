
namespace SimpleCalculator.Core.Injectors
{
    /// <summary>
    /// A container to create an instance of type for registration of dependencies.
    /// </summary>
    public interface IIoCContainer : IDisposable
    {
        /// <summary>
        /// Registers a pair of the interface type and the implementation type of interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of <c>TInterface"</c>.</typeparam>
        /// <param name="lifeStyle">The life style of a registered instance. Default value is transient.</param>
        void Register<TInterface, TImplementation>(InstanceLifeStyle lifeStyle = InstanceLifeStyle.Transient)
            where TInterface : class
            where TImplementation : class, TInterface;

        /// <summary>
        /// Registers a new instances of the implementation type which life style is transient.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        void Register<TImplementation>()
           where TImplementation : class;

        /// <summary>
        /// Registers an implementation type to a collection of the interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of <c>TInterface</c>.</typeparam>
        /// <param name="lifeStyle">The life style of a registered instance. Default value is transient.</param>
        void RegisterAll<TInterface, TImplementation>(InstanceLifeStyle lifeStyle = InstanceLifeStyle.Transient)
            where TInterface : class
            where TImplementation : class, TInterface;

        /// <summary>
        /// Registers a single instance that will be returned when an instance of type.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <param name="instance">The single instance.</param>
        void RegisterInstance<TInterface>(TInterface instance)
            where TInterface : class;

        /// <summary>
        /// Registers a single concrete instance that will be returned when the type of it requested.
        /// </summary>
        /// <typeparam name="TService">The interface or base class type of the single concrete instance.</typeparam>
        void RegisterSingleton<TService>()
            where TService : class;

        ///// <summary>
        ///// Registers a single concrete instance that will be returned when the type of it requested.
        ///// </summary>
        ///// <typeparam name="TService">The interface or base class type of the single concrete instance.</typeparam>
        ///// <param name="serviceInstanceFactory">The delegate to create a concrete instance of the <c>TService</c>.</param>
        //void RegisterSingleton<TService>(Func<TService> serviceInstanceFactory)
        //    where TService : class;

        /// <summary>
        /// Gets all registered instances of the interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the requested interface.</typeparam>
        /// <returns>A sequence of instances of the requested interface</returns>
        IEnumerable<TInterface> GetAllInstances<TInterface>()
            where TInterface : class;

        /// <summary>
        /// Resolves an instance that has been registered.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <returns>An instance of the implementation type has been registered.</returns>
        TInterface Resolve<TInterface>()
            where TInterface : class;

        /// <summary>
        /// Releases all registered instances.
        /// </summary>
        void Release();

        /// <summary>
        /// Verifies and diagnoses the registered instances in the container. Please call this method after all instances were registered.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws the invalid registered instance is found.</exception>
        void Verify();
    }
}
