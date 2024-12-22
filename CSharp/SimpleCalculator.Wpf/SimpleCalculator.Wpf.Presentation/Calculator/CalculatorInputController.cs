using System.Reactive.Subjects;
using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Extensions;
using SimpleCalculator.Core.Extensions;

namespace SimpleCalculator.Wpf.Presentation.Calculator
{
    /// <summary>
    /// The calculator input controller.
    /// </summary>
    internal class CalculatorInputController
    {
        #region Fields

        private readonly Subject<string> numericalInputChanged = new Subject<string>();
        private readonly Subject<IEnumerable<CalculatorToken>> calculatorTokensChanged = new Subject<IEnumerable<CalculatorToken>>();
        private readonly List<CalculatorToken> calculatorTokens = new List<CalculatorToken>();

        private string numericalInput = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Notifies when the numerical input has been changed.
        /// </summary>
        public IObservable<string> NumericalInputChanged => this.numericalInputChanged;

        /// <summary>
        /// Notifies when the calculator token has been changed.
        /// </summary>
        public IObservable<IEnumerable<CalculatorToken>> CalculatorTokensChanged => this.calculatorTokensChanged;

        /// <summary>
        /// The collection of calculator tokens.
        /// </summary>
        public IEnumerable<CalculatorToken> CalculatorTokens => this.calculatorTokens;

        /// <summary>
        /// The calculator input state.
        /// </summary>
        public CalculatorInputState State { get; private set; } = CalculatorInputState.Initialized;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the calculator tokens.
        /// </summary>
        public void InitiaizeCalculatorToken()
        {
            this.calculatorTokens.Clear();
        }

        /// <summary>
        /// Initializes the numerical input.
        /// </summary>
        public void InitializeUserInput()
        {
            this.UpdateNumericalInput(NumericalCharacters.Zero);
            this.State = CalculatorInputState.Initialized;
        }

        /// <summary>
        /// Sets calculated resut.
        /// </summary>
        public void Calculated(string result)
        {
            this.UpdateNumericalInput(result);
            this.State = CalculatorInputState.Calculated;
        }

        /// <summary>
        /// Checks whethre zero can be inputted or not.
        /// </summary>
        /// <param name="addingCount">The count to add zero.</param>
        /// <returns><c>true</c> if inputting zero can be executed, otherwise, <c>false</c>.</returns>
        public bool CanInputZero(int addingCount = 1)
        {
            return addingCount >= 1;
        }

        /// <summary>
        /// Inputs zero.
        /// </summary>
        /// <param name="addingCount">The count to add zero.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="addingCount"/> is less than <c>1</c>.</exception>
        public void InputZero(int addingCount = 1)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(addingCount, 1);

            switch (this.State)
            {
                case CalculatorInputState.Initialized:
                    // Not need to update (Keep input number zero)
                    return;

                case CalculatorInputState.NumberInputted:
                    this.AppendNumericalInput(JoinSameCharacters(NumericalCharacters.Zero, addingCount));
                    return;

                case CalculatorInputState.DecimalPointInpuuted:
                    this.AppendNumericalInput(JoinSameCharacters(NumericalCharacters.Zero, addingCount));
                    this.State = CalculatorInputState.NumberInputted;
                    return;

                case CalculatorInputState.BinaryOperatorInputted:
                case CalculatorInputState.Calculated:
                    this.InitializeUserInput();
                    return;

                default:
                    return;
            }
        }

        /// <summary>
        /// Checks whethre non-zero integer can be inputted or not.
        /// </summary>
        /// <param name="nonZeroNumber">The number string.</param>
        /// <returns><c>true</c> if inputting non-zero integer can be executed, otherwise, <c>false</c>.</returns>
        public bool CanInputNonZeroNumber(string nonZeroNumber)
        {
            if (string.IsNullOrEmpty(nonZeroNumber) || IsZeroString(nonZeroNumber))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Inputs non-zero number.
        /// </summary>
        /// <param name="nonZeroNumber">The number string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nonZeroNumber"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="nonZeroNumber"/> is an empty string.</exception>
        public void InputNonZeroNumber(string nonZeroNumber)
        {
            ArgumentException.ThrowIfNullOrEmpty(nonZeroNumber);

            switch (this.State)
            {
                case CalculatorInputState.Initialized:
                case CalculatorInputState.BinaryOperatorInputted:
                case CalculatorInputState.LeftRoundBracketInputted:
                case CalculatorInputState.Calculated:
                    this.UpdateNumericalInput(nonZeroNumber);
                    break;

                case CalculatorInputState.NumberInputted:
                    this.AppendNumericalInput(nonZeroNumber);
                    break;

                default:
                    if (this.calculatorTokens.Any(t => t.IsNumber))
                    {
                        this.AppendNumericalInput(nonZeroNumber);
                    }
                    else
                    {
                        this.UpdateNumericalInput(nonZeroNumber);
                    }
                    break;
            }

            this.State = CalculatorInputState.NumberInputted;
        }

        /// <summary>
        /// Checks whethre decimal point can be inputted or not.
        /// </summary>
        /// <param name="decimalPoint">The decimal point string.</param>
        /// <returns><c>true</c> if inputting decimal point can be executed, otherwise, <c>false</c>.</returns>
        public bool CanInputDecimalPoint(string decimalPoint)
        {
            if (string.IsNullOrEmpty(decimalPoint))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Inputs decimal point.
        /// </summary>
        /// <param name="decimalPoint">The decimal point string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="decimalPoint"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="decimalPoint"/> is an empty string.</exception>
        public void InputDecimalPoint(string decimalPoint)
        {
            ArgumentException.ThrowIfNullOrEmpty(decimalPoint);

            switch (this.State)
            {
                case CalculatorInputState.DecimalPointInpuuted:
                    // The number of decimal point must be one
                    break;

                case CalculatorInputState.Initialized:
                case CalculatorInputState.BinaryOperatorInputted:
                case CalculatorInputState.Calculated:
                    this.UpdateNumericalInput($"{NumericalCharacters.Zero}{decimalPoint}"); // 0.
                    break;

                case CalculatorInputState.NumberInputted:
                    this.AppendNumericalInput(decimalPoint);
                    break;

                default:
                    break;
            }

            this.State = CalculatorInputState.DecimalPointInpuuted;
        }

        /// <summary>
        /// Checks whethre binary operator can be inputted or not.
        /// </summary>
        /// <param name="operator">The binary operator string.</param>
        /// <returns><c>true</c> if inputting binary operator can be executed, otherwise, <c>false</c>.</returns>
        public bool CanInputBinaryOperator(MathOperator @operator)
        {
            if (@operator is null || !@operator.IsBinaryOperator)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Inputs binary operator.
        /// </summary>
        /// <param name="operator">The decimal point string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="operator"/> is <c>null</c>.</exception>
        public void InputBinaryOperator(MathOperator @operator)
        {
            ArgumentNullException.ThrowIfNull(@operator);

            if (this.State == CalculatorInputState.BinaryOperatorInputted)
            {
                // To update binary operator
                this.calculatorTokens.RemoveLast();
            }
            else if (this.State != CalculatorInputState.RightRoundBracketInputted)
            {
                // Register constant term before registering binary operator
                this.RegisterCurrentNumberInput();
            }

            this.calculatorTokens.Add(@operator);
            this.State = CalculatorInputState.BinaryOperatorInputted;

            this.calculatorTokensChanged.OnNext(this.calculatorTokens);
        }

        /// <summary>
        /// Checks whethre left round bracket be inputted or not.
        /// </summary>
        /// <param name="bracketToken">The left round bracket token string.</param>
        /// <returns><c>true</c> if inputting left round bracket can be executed, otherwise, <c>false</c>.</returns>
        public bool CanInputLeftRoundBracket(CalculatorToken bracketToken)
        {
            if (bracketToken is null || !bracketToken.IsLeftRoundBracket())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Inputs left round bracket.
        /// </summary>
        /// <param name="bracketToken">The left round bracket token string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bracketToken"/> is <c>null</c>.</exception>
        public void InputLeftRoundBracket(CalculatorToken bracketToken)
        {
            ArgumentNullException.ThrowIfNull(bracketToken);

            this.calculatorTokens.Add(bracketToken);
            this.State = CalculatorInputState.LeftRoundBracketInputted;

            this.calculatorTokensChanged.OnNext(this.calculatorTokens);
        }

        /// <summary>
        /// Checks whethre right round bracket be inputted or not.
        /// </summary>
        /// <param name="bracketToken">The right round bracket token string.</param>
        /// <returns><c>true</c> if inputting right round bracket can be executed, otherwise, <c>false</c>.</returns>
        public bool CanInputRightRoundBracket(CalculatorToken bracketToken)
        {
            if (bracketToken is null || !bracketToken.IsRightRoundBracket())
            {
                return false;
            }

            if (!this.calculatorTokens.HasLeftBrackets())
            {
                return false;
            }

            return this.State == CalculatorInputState.Initialized
                || this.State == CalculatorInputState.NumberInputted
                || this.State == CalculatorInputState.RightRoundBracketInputted;
        }

        /// <summary>
        /// Inputs right round bracket.
        /// </summary>
        /// <param name="bracketToken">The right round bracket token string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bracketToken"/> is <c>null</c>.</exception>
        public void InputRightRoundBracket(CalculatorToken bracketToken)
        {
            ArgumentNullException.ThrowIfNull(bracketToken);

            if (this.State != CalculatorInputState.RightRoundBracketInputted)
            {
                if (!this.RegisterCurrentNumberInput())
                {
                    // Prevents invalid expression
                    return;
                }
            }

            this.calculatorTokens.Add(bracketToken);
            this.State = CalculatorInputState.RightRoundBracketInputted;

            this.calculatorTokensChanged.OnNext(this.calculatorTokens);
        }

        /// <summary>
        /// Pre-execute to calculate.
        /// </summary>
        /// <returns><c>true</c> if pre-executing is successful, otherwise, <c>false</c>.</returns>
        public bool PreExecuteToCalculate()
        {
            if (this.State == CalculatorInputState.RightRoundBracketInputted)
            {
                // Do not need to prepare
                return true;
            }

            if (this.RegisterCurrentNumberInput())
            {
                this.calculatorTokensChanged.OnNext(this.calculatorTokens);
                return true;
            }

            return false;
        }

        #endregion

        #region Private Methods

        private static bool IsZeroString(string numberString)
        {
            return numberString == NumericalCharacters.Zero || numberString == NumericalCharacters.ZeroZero;
        }

        private void UpdateNumericalInput(string value)
        {
            if (this.numericalInput != value)
            {
                this.numericalInput = value;
                this.numericalInputChanged.OnNext(this.numericalInput);
            }
        }

        private void AppendNumericalInput(string value)
        {
            this.numericalInput += value;
            this.numericalInputChanged.OnNext(this.numericalInput);
        }

        private static string JoinSameCharacters(string sameChar, int joinCount)
        {
            var result = string.Empty;
            for(int i=0; i < joinCount; ++i)
            {
                result += sameChar;
            }

            return result;
        }

        private bool RegisterCurrentNumberInput()
        {
            var numberToken = NumberTokenFactory.Create(this.numericalInput);
            if (numberToken is not null)
            {
                this.calculatorTokens.Add(numberToken);
                return true;
            }

            return false;
        }

        #endregion
    }
}
