using SimpleCalculator.Core.Injectors;

using System.Windows;

namespace SimpleCalculator.Wpf.Presentation
{
    /// <summary>
    /// Responsible for creating the main window.
    /// </summary>
    public class MainWindowFactory
    {
        private readonly IIoCContainer container;

        /// <summary>
        /// Initialzies a new instance of the <see cref="MainWindowFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException"><paramref name="container"/> is <c>null</c>.</exception>
        public MainWindowFactory(IIoCContainer container)
        {
            ArgumentNullException.ThrowIfNull(container);

            this.container = container;
        }

        /// <summary>
        /// Creates a main window.
        /// </summary>
        /// <returns>An instance of the main widonw.</returns>
        public Window Create()
        {
            this.container.Register<MainWindowViewModel>();

            return new MainWindow()
            {
                DataContext = this.container.Resolve<MainWindowViewModel>(),
            };
        }
    }
}
