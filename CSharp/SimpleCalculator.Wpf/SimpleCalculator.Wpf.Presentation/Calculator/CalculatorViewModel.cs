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

        private readonly OperatorConverter operatorConverter = new OperatorConverter();
        private readonly List<string> displayExpressionTokens = new List<string>();
        private readonly List<CalculatorToken> calculatorTokens = new List<CalculatorToken>(); // for calculation logic

        private string currentExpression;
        private string numericalInput;
        private string lastInput;

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

            this.ResetCalculator();

            this.InputZeroCommand = new DelegateCommand<string>(this.InputZero);
            this.InputNonZeroIntegerCommand = new DelegateCommand<string>(this.InputNonZeroInteger);
            this.InputPeriodCommand = new DelegateCommand<string>(this.InputPeriod);
            this.InputBinaryOperatorCommand = new DelegateCommand<string>(this.InputBinaryOperator);
            this.ResetCommand = new DelegateCommand(this.ResetCalculator);
            this.CalculateCommand = new DelegateCommand(this.Calculate);
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating the calculator is initialized or the calculation is completed.
        /// </summary>
        /// <returns><c>true</c> if there is no calculator token, otherwise, <c>false</c>.</returns>
        private bool IsInitialized => this.calculatorTokens.IsEmpty();

        /// <summary>
        /// Gets or sets a current expresion to calculate.
        /// </summary>
        public string CurrentExpression
        {
            [return: NotNull]
            get => this.currentExpression;
            set => this.SetProperty(ref this.currentExpression, value);
        }

        /// <summary>
        /// Gets or sets an input correspondings the expression numeber term.
        /// </summary>
        public string NumericalInput
        {
            [return: NotNull]
            get => this.numericalInput ?? string.Empty;
            set
            {
                if (this.SetProperty(ref this.numericalInput, value))
                {
                    // Updated
                    this.lastInput = value;
                }
            }
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

            if (this.IsInitialized)
            {
                this.InitializeCurrentExpression();

                if (this.IsFirstInput())
                {
                    this.NumericalInput = NumericalCharacters.Zero;
                }
                else
                {
                    this.NumericalInput += zero;
                }

                return;
            }

            if (this.IsPreviousInputMathOperator())
            {
                this.NumericalInput = NumericalCharacters.Zero;
            }
            else
            {
                this.NumericalInput += zero;
            }
        }

        private void InputNonZeroInteger(string selectedNumber)
        {
            if (string.IsNullOrEmpty(selectedNumber))
            {
                return;
            }

            if (this.IsInitialized)
            {
                this.InitializeCurrentExpression();

                if (this.IsFirstInput())
                {
                    this.NumericalInput = selectedNumber;
                }
                else
                {
                    this.NumericalInput += selectedNumber;
                }

                return;
            }

            if (this.IsPreviousInputMathOperator())
            {
                this.NumericalInput = selectedNumber;
            }
            else
            {
                this.NumericalInput += selectedNumber;
            }
        }

        private void InputPeriod(string period)
        {
            Debug.Assert(period == NumericalCharacters.Period, $"Input must be period. Actual input: {period}");

            if (this.IsInitialized)
            {
                this.InitializeCurrentExpression();
            }

            if ( this.NumericalInput.Contains(period))
            {
                // The number of decimal point must be one
                return;
            }

            this.NumericalInput += period;

            this.lastInput = period;
        }

        private void InputBinaryOperator(string selectedOperator)
        {
            if (!this.TryGetMathOperator(selectedOperator,  out var @operator))
            {
                Debug.Fail($"Found the unexpected binary operator: {selectedOperator}");
                return;
            }

            if (!@operator.IsBinaryOperator)
            {
                return;
            }

            if (this.IsPreviousInputBinaryOperator())
            {
                // Update binary operator
                this.calculatorTokens.RemoveLast(); // Remove previous operator
                this.calculatorTokens.Add(@operator);

                this.displayExpressionTokens.RemoveLast(); // Remove previous operator
                this.AppendDisplayExpressionTokens(selectedOperator);
            }
            else
            {
                // Determines number term
                this.calculatorTokens.Add(NumberTokenFactory.Create(NumericalInput));
                this.calculatorTokens.Add(@operator);

                this.AppendDisplayExpressionTokens(this.NumericalInput, selectedOperator);
            }

            this.lastInput = selectedOperator;
        }

        private void Calculate()
        {
            if (this.IsInitialized)
            {
                return;
            }

            // Determines number term
            this.calculatorTokens.Add(NumberTokenFactory.Create(this.NumericalInput));
            this.AppendDisplayExpressionTokens(this.NumericalInput);

            try
            {
                var calculatedResult = this.infixNotationCalculateService.Calculate(calculatorTokens);
                if (int.TryParse(calculatedResult.ToString(), out var integerResult))
                {
                    this.NumericalInput = integerResult.ToString();
                }
                else
                {
                    this.NumericalInput = Math.Round(calculatedResult, 15).ToString();
                }
            }
            catch (Exception ex)
            {
                this.NumericalInput = ex.Message;
            }
            finally
            {
                // Current expression is not be initialized here,
                // so that the target calculation can be checked 
                this.InitializeCalculatorTokens();
                this.lastInput = string.Empty;
            }
        }

        private void ResetCalculator()
        {
            this.InitializeUserInput();
            this.InitializeCurrentExpression();
            this.InitializeCalculatorTokens();
        }

        private void InitializeCurrentExpression()
        {
            this.CurrentExpression = string.Empty;
        }

        private void InitializeCalculatorTokens()
        {
            this.displayExpressionTokens.Clear();
            this.calculatorTokens.Clear();
        }

        private void InitializeUserInput()
        {
            this.NumericalInput = NumericalCharacters.Zero;
            this.lastInput = string.Empty;
        }

        private static bool IsZeroString(string numberString)
        {
            return numberString == NumericalCharacters.Zero || numberString == NumericalCharacters.ZeroZero;
        }

        private bool IsCurrentInputtingNumberZero()
        {
            return this.NumericalInput == NumericalCharacters.Zero;
        }

        private bool TryGetMathOperator(string input, out MathOperator result)
        {
            result = null;

            // The operators displayed on the calculator and used by calculation logic may be different.
            if (!this.operatorConverter.TryConvertDisplayToLogicString(input, out var operatorLogicCharacter))
            {
                return false;
            }

            result = this.symbolTokens.FindMathOperator(operatorLogicCharacter);
            return result is not null;
         }

        private bool IsPreviousInputBinaryOperator()
        {
            if (!this.operatorConverter.TryConvertDisplayToLogicString(this.lastInput, out var operatorLogicCharacter))
            {
                return false;
            }

            var @operator = this.symbolTokens.FindBinaryOperator(operatorLogicCharacter);
            return @operator is not null;
        }

        private bool IsPreviousInputMathOperator()
        {
            return this.TryGetMathOperator(this.lastInput, out _);
        }

        private bool IsFirstInput()
        {
            return string.IsNullOrEmpty(this.lastInput);
        }

        private void AppendDisplayExpressionTokens(params string[] tokens)
        {
            if (tokens.IsNullOrEmpty())
            {
                return;
            }

            this.displayExpressionTokens.AddRange(tokens);
            this.CurrentExpression = string.Join(' ', displayExpressionTokens);
        }

        #endregion
    }
}
