using System.Data;

namespace SimpleCalculator.CalculationLogic.Core.Services
{
    /// <summary>
    /// Responsible for converting the given notation to the different notation.
    /// </summary>
    public interface IExpressionNotationConverter
    {
        /// <summary>
        /// Converts the infix notation to the reverse Polish notation (RPN).
        /// </summary>
        /// <param name="infixNotationTokens">The tokens in an expression with the infix notation.</param>
        /// <returns>A converted tokens.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="infixNotationTokens"/> is <c>null</c>.</exception>
        /// <exception cref="SyntaxErrorException">
        /// <para>Found an unknown token in <paramref name="infixNotationTokens"/>.</para>
        /// <para>Found an invalid bracket format the given expression.</para>
        /// </exception>
        Stack<CalculatorToken> ConvertInfixToRPNStack(IEnumerable<CalculatorToken> infixNotationTokens);
    }
}
