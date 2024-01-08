using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents all symbol tokens in expression used in calculator.
    /// </summary>
    public class CalculatorSymbolTokens : ICalculatorSymbolTokens
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorSymbolTokens"/> class.
        /// </summary>
        public CalculatorSymbolTokens()
        {
            this.LeftRoundBracket = new RoundBracketToken(SymbolConstants.RoundBracket.Left);
            this.RightRoundBracket = new RoundBracketToken(SymbolConstants.RoundBracket.Right);

            this.Add = new AddOperator();
            this.Subtract = new SubtractOperator();
            this.Multiply = new MultiplyOperator();
            this.Divide = new DivideOperator();
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public IEnumerable<CalculatorToken> AllTokens
            => this.AllBrackets.Concat(this.AllOperators);

        #region Brackets

        /// <summary>
        /// Gets a left round bracket token.
        /// </summary>
        internal RoundBracketToken LeftRoundBracket { get; }

        /// <summary>
        /// Gets a right round bracket token.
        /// </summary>
        internal RoundBracketToken RightRoundBracket { get; }

        /// <inheritdoc />
        public IEnumerable<CalculatorToken> AllBrackets
        {
            get
            {
                yield return this.LeftRoundBracket;
                yield return this.RightRoundBracket;
            }
        }

        #endregion

        #region Math operators

        /// <summary>
        /// Gets an operator to add.
        /// </summary>
        internal AddOperator Add { get; }

        /// <summary>
        /// Gets an operator to subtract.
        /// </summary>
        internal SubtractOperator Subtract { get; }

        /// <summary>
        /// Gets an operator to multiply.
        /// </summary>
        internal MultiplyOperator Multiply { get; }

        /// <summary>
        /// Gets an operator to divide.
        /// </summary>
        internal DivideOperator Divide { get; }

        /// <inheritdoc />
        public IEnumerable<MathOperator> AllOperators
        {
            get
            {
                yield return this.Add;
                yield return this.Subtract;
                yield return this.Multiply;
                yield return this.Divide;
            }
        }

        /// <inheritdoc />
        public IEnumerable<MathOperator> AllBinaryOpeartors
            => this.AllOperators.Where(op => op.OperatorType == MathOperatorType.Binary);

        #endregion

        #endregion

        #region Public Methods

        /// <inheritdoc />
        [return: MaybeNull]
        public CalculatorToken FindSymbolToken(string tokenValue)
        {
            if (string.IsNullOrEmpty(tokenValue))
            {
                return null;
            }

            return this.AllTokens.SingleOrDefault(t => t.Value.Equals(tokenValue, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        [return: MaybeNull]
        public CalculatorToken FindBracketToken(string tokenValue)
        {
            if (string.IsNullOrEmpty(tokenValue))
            {
                return null;
            }

            return this.AllBrackets.SingleOrDefault(b => b.Value.Equals(tokenValue, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        [return: MaybeNull]
        public MathOperator FindMathOperator(string operatorString)
        {
            if (string.IsNullOrEmpty(operatorString))
            {
                return null;
            }

            var foundOperator = this.AllOperators.SingleOrDefault(op => op.Value.Equals(operatorString, StringComparison.OrdinalIgnoreCase));
            return foundOperator;
        }

        /// <inheritdoc />
        [return: MaybeNull]
        public MathOperator FindBinaryOperator(string operatorString)
        {
            if (string.IsNullOrEmpty(operatorString))
            {
                return null;
            }

            return this.AllBinaryOpeartors.FirstOrDefault(op => op.Value.Equals(operatorString, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
