namespace SimpleCalculator.CalculationLogic.Core.Services
{
    /// <summary>
    /// Responsible for dealing with an expression written by infix notation.
    /// </summary>
    public interface IInfixNotationCalculateService
    {
        /// <summary>
        /// Calculates an expression with infix notation.
        /// </summary>
        /// <param name="infixExpressionTokens">The tokens in infix expression to calculate.</param>
        /// <returns>A calculated value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="infixExpressionTokens"/> is <c>null</c>.</exception>
        /// <exception cref="ArithmeticException">Failed to calculate the given expression. See inner exception for details.</exception>
        double Calculate(IEnumerable<CalculatorToken> infixExpressionTokens);
    }
}
