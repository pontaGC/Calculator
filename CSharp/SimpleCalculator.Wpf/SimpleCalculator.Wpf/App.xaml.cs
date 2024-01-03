using SimpleCalculator.Core;
using System.Windows;

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
            var mainWindow = new Window1()
            {
                Title = SimpleCalculatorConstants.ToolTitle,
                DataContext = new Window1ViewModel(),
            };
            mainWindow.ShowDialog();
        }
    }

}
