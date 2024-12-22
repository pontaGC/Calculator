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

        private readonly IDisposable numericalInputSubscriber;
        private readonly IDisposable calculatorTokensSubscriber;
        private readonly CalculatorInputController inputController = new CalculatorInputController();
        private readonly SymbolConverter symbolConverter = new SymbolConverter();

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

            this.numericalInputSubscriber = this.inputController.NumericalInputChanged.Subscribe(this.OnNumericalInputChanged);
            this.calculatorTokensSubscriber = this.inputController.CalculatorTokensChanged.Subscribe(this.OnCalculatorTokensChanged);

            this.InputZeroCommand = new DelegateCommand<string>(this.InputZero);
            this.InputZeroZeroCommand = new DelegateCommand<string>(this.InputZeroZero);
            this.InputNonZeroIntegerCommand = new DelegateCommand<string>(this.InputNonZeroInteger);
            this.InputPeriodCommand = new DelegateCommand<string>(this.InputPeriod);
            this.InputLeftRoundBracketCommand = new DelegateCommand<string>(this.InputLeftRoundBracket);
            this.InputRightRoundBracketCommand = new DelegateCommand<string>(this.InputRightRoundBracket);
            this.InputBinaryOperatorCommand = new DelegateCommand<string>(this.InputBinaryOperator);
            this.ResetNumericalInputCommand = new DelegateCommand(this.ResetNumericalInput);
            this.ResetCommand = new DelegateCommand(this.ResetCalculator);
            this.CalculateCommand = new DelegateCommand(this.Calculate);

            this.ResetCalculator();
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not it is necessary to initialize the expression.
        /// </summary>
        private bool NeedsExpressionInitialization => this.inputController.State == CalculatorInputState.Calculated;

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
            set => this.SetProperty(ref this.numericalInput, value);
        }

        /// <summary>
        /// Gets a command that is executed when zero is selected.
        /// </summary>
        public ICommand InputZeroCommand { get; }

        /// <summary>
        /// Gets a command that is executed when two consecutive zero ('00') is selected.
        /// </summary>
        public ICommand InputZeroZeroCommand { get; }

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
        /// Gets a command that is executed when a clear entry is selected.
        /// </summary>
        public ICommand ResetNumericalInputCommand { get; }

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
            if (this.NeedsExpressionInitialization)
            {
                this.InitializeCurrentExpression();
            }

            if (this.inputController.CanInputZero())
            {
                this.inputController.InputZero();
            }
        }

        private void InputZeroZero(string zeroZero)
        {
            if (this.NeedsExpressionInitialization)
            {
                this.InitializeCurrentExpression();
            }

            if (this.inputController.CanInputZero(2))
            {
                this.inputController.InputZero(2);
            }
        }

        private void InputNonZeroInteger(string selectedNumber)
        {
            if (this.NeedsExpressionInitialization)
            {
                this.InitializeCurrentExpression();
            }
            
            if (this.inputController.CanInputNonZeroNumber(selectedNumber))
            {
                this.inputController.InputNonZeroNumber(selectedNumber);
            }
        }

        private void InputPeriod(string period)
        {
            if (this.NeedsExpressionInitialization)
            {
                this.InitializeCurrentExpression();
            }

            if (this.inputController.CanInputDecimalPoint(period))
            {
                this.inputController.InputDecimalPoint(period);
            }
        }

        private void InputLeftRoundBracket(string leftRoundBracket)
        {
            if (!this.symbolConverter.TryConvertDisplayToLogicString(leftRoundBracket, out var logicSymbol))
            {
                return;
            }

            var bracketToken = this.symbolTokens.FindBracketToken(logicSymbol);
            if (bracketToken is null)
            {
                // Invalid token
                return;
            }

            if (this.NeedsExpressionInitialization)
            {
                this.InitializeCurrentExpression();
            }

            if (this.inputController.CanInputLeftRoundBracket(bracketToken))
            {
                this.inputController.InputLeftRoundBracket(bracketToken);
            }
        }

        private void InputRightRoundBracket(string rightRoundBracket)
        {
            if (!this.symbolConverter.TryConvertDisplayToLogicString(rightRoundBracket, out var logicSymbol))
            {
                return;
            }

            var bracketToken = this.symbolTokens.FindBracketToken(logicSymbol);
            if (bracketToken is null)
            {
                // Invalid token
                return;
            }

            if (this.NeedsExpressionInitialization)
            {
                this.InitializeCurrentExpression();
            }

            if (this.inputController.CanInputRightRoundBracket(bracketToken))
            {
                this.inputController.InputRightRoundBracket(bracketToken);
            }
        }

        private void InputBinaryOperator(string selectedOperator)
        {
            if (!this.TryGetMathOperator(selectedOperator,  out var @operator))
            {
                Debug.Fail($"Found the unexpected binary operator: {selectedOperator}");
                return;
            }

            if (this.inputController.CanInputBinaryOperator(@operator))
            {
                this.inputController.InputBinaryOperator(@operator);
            }
        }

        private void Calculate()
        {
            if (this.inputController.CalculatorTokens.IsEmpty())
            {
                return;
            }

            if (!this.inputController.PreExecuteToCalculate())
            {
                return;
            }

            var result = string.Empty;
            try
            {
                var calculatedResult = this.infixNotationCalculateService.Calculate(this.inputController.CalculatorTokens);
                if (int.TryParse(calculatedResult.ToString(), out var integerResult))
                {
                    result = integerResult.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    // 15 is the number to ignore binary errors
                    result = Math.Round(calculatedResult, 15).ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                result = ex.GetMessage();
            }
            finally
            {
                this.inputController.Calculated(result);

                // Current expression is not be initialized here,
                // so that the target calculation can be checked 
                this.InitializeCalculatorTokens();
            }
        }

        private void ResetNumericalInput()
        {
            this.InitializeUserInput();
        }

        private void ResetCalculator()
        {
            this.InitializeUserInput();
            this.InitializeCurrentExpression();
            this.InitializeCalculatorTokens();
        }

        /// <summary>
        /// Initializes a calculation expression displayed in view.
        /// </summary>
        private void InitializeCurrentExpression()
        {
            this.CurrentExpression = string.Empty;
        }

        /// <summary>
        /// Initializes the expression tokens for diplay and internal logic.
        /// </summary>
        private void InitializeCalculatorTokens()
        {
            this.inputController.InitiaizeCalculatorToken();
        }

        /// <summary>
        /// Initializes the numerical input and clears the last input record.
        /// </summary>
        private void InitializeUserInput()
        {
            this.inputController.InitializeUserInput();
        }

        private bool TryGetMathOperator(string input, out MathOperator result)
        {
            result = null;

            // The operators displayed on the calculator and used by calculation logic may be different.
            if (!this.symbolConverter.TryConvertDisplayToLogicString(input, out var operatorLogicCharacter))
            {
                return false;
            }

            result = this.symbolTokens.FindMathOperator(operatorLogicCharacter);
            return result is not null;
         }

        private void OnNumericalInputChanged(string newValue)
        {
            this.NumericalInput = newValue;
        }

        private void OnCalculatorTokensChanged(IEnumerable<CalculatorToken> newTokens)
        {
            // Result of converting internal logic tokens to display tokens
            var displayTokens = this.ConvertCalculatorTokensToDisplayToken(newTokens);
            this.CurrentExpression = string.Join(' ', displayTokens);
        }

        private IReadOnlyCollection<string> ConvertCalculatorTokensToDisplayToken(IEnumerable<CalculatorToken> calculatorTokens)
        {
            var displayTokens = new List<string>();
            foreach (var token in calculatorTokens)
            {
                if (token.IsNumber)
                {
                    displayTokens.Add(token.Value);
                    continue;
                }

                if (token.IsSymbol)
                {
                    if (this.symbolConverter.TryConvertLogicToDisplayString(token.Value, out var displayTokenValue))
                    {
                        displayTokens.Add(displayTokenValue);
                    }
                    else
                    {
                        displayTokens.Add(token.Value);
                    }
                    continue;
                }
            }

            return displayTokens;
        }

        #endregion
    }
}
