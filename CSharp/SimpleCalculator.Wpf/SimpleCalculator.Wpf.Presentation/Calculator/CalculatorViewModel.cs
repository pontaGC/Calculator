using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using System.Windows.Input;

using Prism.Commands;
using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.Core.Extensions;
using SimpleCalculator.Core.Mvvm;
using SimpleCalculator.Wpf.Presentation.Converters;

namespace SimpleCalculator.Wpf.Presentation.Calculator
{
    /// <summary>
    /// The view-model of a calculator.
    /// </summary>
    internal class CalculatorViewModel : ViewModelBase
    {
        private readonly IInfixNotationCalculateService infixNotationCalculateService;
        private readonly ICalculatorSymbolTokens symbolTokens;

        private readonly List<string> displayExpressionTokens = new List<string>();
        private readonly List<CalculatorToken> calculatorTokens; // for calculation logic

        private string currentExpression;
        private string numericalInput;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorViewModel"/> class.
        /// </summary>
        /// <param name="infixNotationCalculateService">The infix notation calculate service.</param>
        /// <param name="symbolTokens">The calculator symbol tokens.</param>
        public CalculatorViewModel(
            IInfixNotationCalculateService infixNotationCalculateService,
            ICalculatorSymbolTokens symbolTokens)
        {
            this.infixNotationCalculateService = infixNotationCalculateService;
            this.symbolTokens = symbolTokens;

            ResetCalculator();

            InputZeroCommand = new DelegateCommand<string>(this.InputZero);
            InputNonZeroIntegerCommand = new DelegateCommand<string>(this.InputNonZeroInteger);
            InputPeriodCommand = new DelegateCommand<string>(this.InputPeriod);
            InputBinaryOperatorCommand = new DelegateCommand<string>(this.InputBinaryOperator);
            ResetCommand = new DelegateCommand(this.ResetCalculator);
            CalculateCommand = new DelegateCommand(this.Calculate);
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating the calculator is initialized or the calculation is completed.
        /// </summary>
        /// <returns><c>true</c> if there is no calculator token, otherwise, <c>false</c>.</returns>
        private bool IsInitialized => calculatorTokens.IsEmpty();

        /// <summary>
        /// Gets or sets a current expresion to calculate.
        /// </summary>
        public string CurrentExpression
        {
            [return: NotNull]
            get => currentExpression;
            set => this.SetProperty(ref currentExpression, value);
        }

        /// <summary>
        /// Gets or sets an input correspondings the expression numeber term.
        /// </summary>
        public string NumericalInput
        {
            [return: NotNull]
            get => numericalInput ?? string.Empty;
            set => this.SetProperty(ref numericalInput, value);
        }

        /// <summary>
        /// Gets a command that is executed when zero is selected.
        /// </summary>
        public ICommand InputZeroCommand { get; }

        /// <summary>
        /// Gets a command that is executed when an integer is selected.
        /// </summary>
        public ICommand InputNonZeroIntegerCommand { get; }

        /// <summary>
        /// Gets a command that is executed when a period is selected.
        /// </summary>
        public ICommand InputPeriodCommand { get; }

        /// <summary>
        /// Gets a command that is executed when left round bracket is selected.
        /// </summary>
        public ICommand InputLeftRoundBracketCommand { get; }

        /// <summary>
        /// Gets a command that is executed when right round bracket is selected.
        /// </summary>
        public ICommand InputRightRoundBracketCommand { get; }

        /// <summary>
        /// Gets a command that is executed when an opeartor is selected.
        /// </summary>
        public ICommand InputBinaryOperatorCommand { get; }

        /// <summary>
        /// Gets a command that initializes a calculator.
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <summary>
        /// Gets a command that calculates.
        /// </summary>
        public ICommand CalculateCommand { get; }

        #endregion

        #region Private Methods

        private void InputZero(string zero)
        {
            Debug.Assert(IsZeroString(zero), $"Input must be zero. Actual input: {zero}");

            if (IsCurrentInputtingNumberZero())
            {
                // Not need to update (Keep input number zero)
                return;
            }

            if (IsInitialized)
            {
                // Select zero after calculation completion
                NumericalInput = NumericalCharacters.Zero;
            }
            else
            {
                NumericalInput += zero;
            }
        }

        private void InputNonZeroInteger(string selectedNumber)
        {
            if (string.IsNullOrEmpty(selectedNumber))
            {
                return;
            }

            if (IsInitialized || IsCurrentInputtingNumberZero())
            {
                // Replace initial value (zero) to selected number
                NumericalInput = selectedNumber;
            }
            else
            {
                NumericalInput += selectedNumber;
            }
        }

        private void InputPeriod(string period)
        {
            Debug.Assert(period == NumericalCharacters.Period, $"Input must be period. Actual input: {period}");

            var lastInputted = NumericalInput.GetLastCharacter();
            if (lastInputted == period)
            {
                // The number of decimal point must be one
                return;
            }

            NumericalInput += period;
        }

        private void InputBinaryOperator(string selectedOperator)
        {
            // The operators displayed on the calculator and used by calculation logic may be different.
            var operatorConverter = new OperatorConverter();
            if (!operatorConverter.TryConvertDisplayToLogicString(selectedOperator, out var operatorLogicString))
            {
                Debug.Fail($"Found the unexpected binary operator: {selectedOperator}");
                return;
            }

            var binaryOperatorToken = symbolTokens.FindBinaryOperator(operatorLogicString);
            if (binaryOperatorToken is null)
            {
                return;
            }

            if (IsPreviousInputBinaryOperator())
            {
                // Update binary operator
                calculatorTokens.RemoveLast(); // Remove previous operator
                calculatorTokens.Add(binaryOperatorToken);

                displayExpressionTokens.RemoveLast(); // Remove previous operator
                AppendDisplayTokens(selectedOperator);
                return;
            }

            // Determines number term
            calculatorTokens.Add(NumberTokenFactory.Create(NumericalInput));
            calculatorTokens.Add(binaryOperatorToken);

            AppendDisplayTokens(NumericalInput, selectedOperator);
        }

        private void Calculate()
        {
            if (IsInitialized)
            {
                return;
            }

            try
            {
                var calculatedResult = infixNotationCalculateService.Calculate(calculatorTokens);
                NumericalInput = calculatedResult.ToString(CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                NumericalInput = ex.Message;
            }
            finally
            {
                InitializeCalculatorTokens();
            }
        }

        private void ResetCalculator()
        {
            InitializeUserInput();
            InitializeCurrentExpression();
            InitializeCalculatorTokens();
        }

        private void InitializeCurrentExpression()
        {
            CurrentExpression = string.Empty;
        }

        private void InitializeCalculatorTokens()
        {
            calculatorTokens.Clear();
        }

        private void InitializeUserInput()
        {
            NumericalInput = NumericalCharacters.Zero;
        }

        private static bool IsZeroString(string numberString)
        {
            return numberString == NumericalCharacters.Zero || numberString == NumericalCharacters.ZeroZero;
        }

        private bool IsCurrentInputtingNumberZero()
        {
            return NumericalInput == NumericalCharacters.Zero;
        }

        private bool IsPreviousInputBinaryOperator()
        {
            var lastTermToken = calculatorTokens.Last();
            if (lastTermToken is not MathOperator @operator)
            {
                return false;
            }

            return @operator.IsBinaryOperator;
        }

        private void AppendDisplayTokens(params string[] tokens)
        {
            displayExpressionTokens.AddRange(tokens);
            CurrentExpression = string.Join(' ', displayExpressionTokens);
        }

        #endregion
    }
}
