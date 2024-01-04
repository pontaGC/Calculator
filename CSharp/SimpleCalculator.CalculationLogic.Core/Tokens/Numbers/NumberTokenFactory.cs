using SimpleCalculator.Core.Utils;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Responsible for creating the number token.
    /// </summary>
    public class NumberTokenFactory
    {
        /// <summary>
        /// Creates a number token.
        /// </summary>
        /// <param name="value">The numerical value.</param>
        /// <returns>An instance of the calculator number token.</returns>
        public static NumberToken Create(double value)
        {
            return new NumberToken(value);
        }

        /// <summary>
        /// Creates a number token.
        /// </summary>
        /// <param name="value">The string of numerical value.</param>
        /// <returns>An instance of the calculator number token if <paramref name="value"/> can be converted to numerical value, otherwise, returns <c>null</c>.</returns>
        [return: MaybeNull]
        public static NumberToken Create(string value)
        {
            if (DoubleHelper.TryParseInvariantCulture(value, out var result))
            {
                return Create(result);
            }

            return null;
        }
    }
}
