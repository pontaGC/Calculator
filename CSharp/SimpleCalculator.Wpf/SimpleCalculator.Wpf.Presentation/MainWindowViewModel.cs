using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.Core.Mvvm;
using SimpleCalculator.Core.Utils;
using SimpleCalculator.Wpf.Presentation.Calculator;

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
        public MainWindowViewModel(IInfixNotationCalculateService infixNotationCalculateService)
        {
            ArgumentGuard.RequireNotNull(infixNotationCalculateService, nameof(infixNotationCalculateService));

            this.CalcularatorViewModel = new CalculatorViewModel(infixNotationCalculateService, new CalculatorSymbolTokens());
        }

        /// <summary>
        /// Gets a view-model of the calculator.
        /// </summary>
        public CalculatorViewModel CalcularatorViewModel { get; }
    }
}
