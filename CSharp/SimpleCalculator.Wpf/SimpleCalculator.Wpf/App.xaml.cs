using System.Windows;
using System.Windows.Threading;

using SimpleCalculator.CalculateLogic;
using SimpleCalculator.Core;
using SimpleCalculator.Core.Injectors;
using SimpleCalculator.Wpf.Presentation;

namespace SimpleCalculator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            this.RegisterAppUnhandledExceptionHandler();

            var container = ContainerFactory.Create();
            RegisterDependencies(container, new CalculationLogicCoreDependencyRegistrant());
            RegisterDependencies(container, new CalculationLogicDependencyRegistrant());

            var mainWindowFactory = new MainWindowFactory(container);
            var mainWindow = mainWindowFactory.Create();
            mainWindow.ShowDialog();
        }

        private static void RegisterDependencies(IIoCContainer container, IDependenyRegistrant dependencyRegistrant)
        {
            dependencyRegistrant.Register(container);
        }

        #region Unhandled exception handlers

        private void RegisterAppUnhandledExceptionHandler()
        {
            // Unhandled exception caused in main thread (UI thread as WPF)
            DispatcherUnhandledException += this.OnAppDispatcherUnhandledException;

            // Unhandled exception caused in working threads
            TaskScheduler.UnobservedTaskException += this.OnUnobservedTaskException;

            AppDomain.CurrentDomain.UnhandledException += this.OnCurrentDomainUnhandledException;
        }

        private void OnAppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                GetUnhandledErrorMessage(e.Exception),
                SimpleCalculatorConstants.ToolTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true;

            Environment.Exit(1);
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception.InnerException;
            MessageBox.Show(
                GetUnhandledErrorMessage(exception),
                SimpleCalculatorConstants.ToolTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.SetObserved();

            Environment.Exit(1);
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            MessageBox.Show(
                GetUnhandledErrorMessage(exception),
                SimpleCalculatorConstants.ToolTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Environment.Exit(1);
        }

        private static string GetUnhandledErrorMessage(Exception ex)
        {
            if (string.IsNullOrEmpty(ex?.Message))
            {
                return "The unexpected error has occurred.";
            }

            return $"The unexpected error has occurred.\r\n{ex.Message}";
        }

        #endregion
    }
}
