namespace SimpleCalculator.Core.Injectors
{
    /// <summary>
    /// Responsible for creating an instance of container.
    /// </summary>
    public class ContainerFactory
    {
        /// <summary>
        /// Creates the default container.
        /// </summary>
        /// <returns>A default instance of the <see cref="IIoCContainer"/>.</returns>
        public static IIoCContainer Create()
        {
            return new SimpleInjectorContainer();
        }
    }
}
