namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents a round bracket ("(", ")") token.
    /// </summary>
    internal sealed class RoundBracketToken : CalculatorToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundBracketToken"/> class.
        /// </summary>
        /// <param name="value">The token value.</param>
        public RoundBracketToken(string value) 
            : base(
                  CalculatorTokenPriority.RoundBracket,
                  value,
                  CalculatorTokenType.Symbol)
        {
        }
    }
}
