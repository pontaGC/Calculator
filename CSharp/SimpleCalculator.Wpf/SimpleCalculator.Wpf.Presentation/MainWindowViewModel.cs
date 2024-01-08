using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.Core.Mvvm;
using SimpleCalculator.Wpf.Presentation.Calculator;
using System.ComponentModel;

namespace SimpleCalculator.Wpf.Presentation
{
    /// <summary>
    /// The view-model of the main window.
    /// </summary>
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="infixNotationCalculateService">The infix notation calculate service.</param>
        /// <param name="calculatorSymbolTokens">The calculator symbol token model.</param>
        /// <exception cref="ArgumentNullException"><paramref name="infixNotationCalculateService"/> or <paramref name="calculatorSymbolTokens"/> is <c>null</c>.</exception>
        public MainWindowViewModel(
            IInfixNotationCalculateService infixNotationCalculateService,
            ICalculatorSymbolTokens calculatorSymbolTokens)
        {
            ArgumentNullException.ThrowIfNull(infixNotationCalculateService);
            ArgumentNullException.ThrowIfNull(calculatorSymbolTokens);

            this.CalcularatorViewModel = new CalculatorViewModel(infixNotationCalculateService, calculatorSymbolTokens);
        }

        /// <summary>
        /// Gets a view-model of the calculator.
        /// </summary>
        public INotifyPropertyChanged CalcularatorViewModel { get; }
    }
}
