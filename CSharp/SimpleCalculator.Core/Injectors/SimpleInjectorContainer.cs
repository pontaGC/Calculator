using System.Diagnostics;

using SimpleInjector;

namespace SimpleCalculator.Core.Injectors
{
    /// <summary>
    /// IoC Container using a SimpleInjector library.
    /// SimpleInjector: https://simpleinjector.org/index.html
    /// </summary>
    internal sealed class SimpleInjectorContainer : IIoCContainer
    {
        #region Fields

        private static readonly IReadOnlyDictionary<InstanceLifeStyle, Lifestyle> LifeStyles = new Dictionary<InstanceLifeStyle, Lifestyle>()
        {
            { InstanceLifeStyle.Transient, Lifestyle.Transient },
            { InstanceLifeStyle.Singleton, Lifestyle.Singleton },
        };

        private readonly Container container = new Container();

        #endregion

        #region Methods

        /// <inheritdoc />
        void IIoCContainer.Register<TInterface, TImplementation>(InstanceLifeStyle lifeStyle)
        {
            this.container.Register<TInterface, TImplementation>(LifeStyles[lifeStyle]);
        }

        /// <inheritdoc />
        void IIoCContainer.Register<TImplementation>() where TImplementation : class
        {
            this.container.Register<TImplementation>();
        }

        /// <inheritdoc />
        void IIoCContainer.RegisterAll<TInterface, TImplementation>(InstanceLifeStyle lifeStyle)
        {
            this.container.Register<TInterface, TImplementation>(LifeStyles[lifeStyle]);
        }

        /// <inheritdoc />
        void IIoCContainer.RegisterInstance<TInterface>(TInterface instance) where TInterface : class
        {
            this.container.RegisterInstance(instance);
        }

        /// <inheritdoc />
        void IIoCContainer.RegisterSingleton<TService>() where TService : class
        {
            this.container.RegisterSingleton<TService>();
        }

        /// <inheritdoc />
        TInterface IIoCContainer.Resolve<TInterface>() where TInterface : class
        {
            return this.container.GetInstance<TInterface>();
        }

        /// <inheritdoc />
        IEnumerable<TInterface> IIoCContainer.GetAllInstances<TInterface>() where TInterface : class
        {
            return this.container.GetAllInstances<TInterface>();
        }

        /// <inheritdoc />
        void IIoCContainer.Release()
        {
            this.Dispose();
        }

        /// <inheritdoc />
        void IIoCContainer.Verify()
        {
            this.container.Verify();

            // All registration were guaranteed to be valid when debugging.
            this.container.Options.EnableAutoVerification = false;
            this.EnableAutoVerificationForDebug();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.container?.Dispose();
        }

        [Conditional("DEBUG")]
        private void EnableAutoVerificationForDebug()
        {
            this.container.Options.EnableAutoVerification = true;
        } 

        #endregion
    }
}
