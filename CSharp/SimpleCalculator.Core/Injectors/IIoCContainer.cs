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
        /// Resolves an instance that has been registered.
        /// </summary>
        /// <typeparam name="TInterface">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <returns>An instance of the implementation type has been registered.</returns>
        TInterface Resolve<TInterface>()
            where TInterface : class;

        /// <summary>
        /// Verifies and diagnoses the registered instances in the container. Please call this method after all instances were registered.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws the invalid registered instance is found.</exception>
        void Verify();
    }
}
