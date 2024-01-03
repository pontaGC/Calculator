namespace SimpleCalculator.Core.Injectors
{
    /// <summary>
    /// The registrant that registers the dependencies to a container.
    /// </summary>
    public interface IDependencyRegistrant
    {
        /// <summary>
        /// Registers the services to container.
        /// </summary>
        /// <param name="container">The target container to register.</param>
        /// <exception cref="ArgumentNullException"><c>container</c> is <c>null</c>.</exception>
        void RegisterServices(IIoCContainer container);
    }
}
