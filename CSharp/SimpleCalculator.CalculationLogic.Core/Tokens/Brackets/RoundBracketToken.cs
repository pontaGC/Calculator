namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents a round bracket ("(", ")") token.
    /// </summary>
    public abstract class RoundBracketToken : CalculatorToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundBracketToken"/> class.
        /// </summary>
        /// <param name="value">The token value.</param>
        protected RoundBracketToken(string value) 
            : base(
                  CalculatorTokenPriority.RoundBracket,
                  value,
                  CalculatorTokenType.Symbol)
        {
        }
    }

    /// <summary>
    /// The left round bracket token.
    /// </summary>
    public sealed class LeftRoundBracketToken : RoundBracketToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeftRoundBracketToken"/> class.
        /// </summary>
        public LeftRoundBracketToken()
            : base(SymbolConstants.RoundBracket.Left)
        {
        }
    }

    /// <summary>
    /// The right round bracket token.
    /// </summary>
    public sealed class RightRoundBracketToken : RoundBracketToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightRoundBracketToken"/> class.
        /// </summary>
        public RightRoundBracketToken()
            : base(SymbolConstants.RoundBracket.Right)
        {
        }
    }
}
