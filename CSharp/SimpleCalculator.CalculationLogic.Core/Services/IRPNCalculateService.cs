namespace SimpleCalculator.CalculationLogic.Core.Services
{
    /// <summary>
    /// Responsible for calculating an expression with reverse Polish notation.
    /// </summary>
    public interface IRPNCalculateService
    {
        /// <summary>
        /// Calculates a RPN stack.
        /// </summary>
        /// <param name="rpnStack">The tokens with RPN expression.</param>
        /// <returns>A calculated result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="rpnStack"/> is <c>null</c>.</exception>
        /// <exception cref="ArithmeticException">Failed to calculate the given expression token.</exception>
        double Calculate(Stack<CalculatorToken> rpnStack);
    }
}
