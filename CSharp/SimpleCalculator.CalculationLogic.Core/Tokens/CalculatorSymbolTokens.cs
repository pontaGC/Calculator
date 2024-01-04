using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents all symbol tokens in expression used in calculator.
    /// </summary>
    public class CalculatorSymbolTokens
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

            this.AllTokens = new CalculatorToken[]
            {
                this.LeftRoundBracket,
                this.RightRoundBracket,
                this.Add,
                this.Subtract,
                this.Multiply,
                this.Divide,
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a collection of all symbol tokens.
        /// </summary>
        public IReadOnlyCollection<CalculatorToken> AllTokens { get; }

        #region Brackets

        /// <summary>
        /// Gets a left round bracket token.
        /// </summary>
        public RoundBracketToken LeftRoundBracket { get; }

        /// <summary>
        /// Gets a right round bracket token.
        /// </summary>
        public RoundBracketToken RightRoundBracket { get; }

        /// <summary>
        /// Gets a collection of all brackets.
        /// </summary>
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
        public AddOperator Add { get; }

        /// <summary>
        /// Gets an operator to subtract.
        /// </summary>
        public SubtractOperator Subtract { get; }

        /// <summary>
        /// Gets an operator to multiply.
        /// </summary>
        public MultiplyOperator Multiply { get; }

        /// <summary>
        /// Gets an operator to divide.
        /// </summary>
        public DivideOperator Divide { get; }

        /// <summary>
        /// Gets a collection of all math operators.
        /// </summary>
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

        /// <summary>
        /// Gets a collection of all binary oeprators, e.g. "+", "/".
        /// </summary>
        public IEnumerable<MathOperator> AllBinaryOpeartors
            => this.AllOperators.Where(op => op.OperatorType == MathOperatorType.Binary);


        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the symbol token whose value is same as the given token value.
        /// </summary>
        /// <param name="tokenValue">The token value to look for.</param>
        /// <returns>A symbol token if finding symbol token is successful, otherwise, returns <c>null</c>.</returns>
        [return: MaybeNull]
        public CalculatorToken FindSymbolToken(string tokenValue)
        {
            if (string.IsNullOrEmpty(tokenValue))
            {
                return null;
            }

            var foundSymbolToken = this.AllTokens.FirstOrDefault(t => t.Value.Equals(tokenValue, StringComparison.OrdinalIgnoreCase));
            if (foundSymbolToken is not null)
            {
                return foundSymbolToken;
            }

            return null;
        }

        /// <summary>
        /// Finds the binary operator token whose value is same as the operator string.
        /// </summary>
        /// <param name="operatorString">The operator string to look for.</param>
        /// <returns>A binary operator if finding it is successful, otherwise, <c>null</c>.</returns>
        [return: MaybeNull]
        public MathOperator FindBinaryOperator(string operatorString)
        {
            if (string.IsNullOrEmpty(operatorString))
            {
                return null;
            }

            var foundOperator = this.AllBinaryOpeartors.FirstOrDefault(op => op.Value.Equals(operatorString, StringComparison.OrdinalIgnoreCase));
            return foundOperator;
        }

        #endregion
    }
}
