using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculationLogic.Core.Extensions
{
    /// <summary>
    /// Extension methods for the calculator token.
    /// </summary>
    public static class CalculatorTokenExtensions
    {
        #region Public Methods

        /// <summary>
        /// Gets a value indicating a given token value is the left round bracket, "(".
        /// </summary>
        /// <param name="sourceToken">The token to check.</param>
        /// <returns><c>true</c> if <c>sourceToken</c> value is the left round bracket, otherwise, <c>false</c>.</returns>
        public static bool IsLeftRoundBracket(this CalculatorToken sourceToken)
        {
            if (sourceToken is null)
            {
                return false;
            }

            return EqualsSymbol(sourceToken.Value, SymbolConstants.RoundBracket.Left);
        }

        /// <summary>
        /// Gets a value indicating a given token value is the right round bracket, ")".
        /// </summary>
        /// <param name="sourceToken">The token to check.</param>
        /// <returns><c>true</c> if <c>sourceToken</c> value is the right round bracket, otherwise, <c>false</c>.</returns>
        public static bool IsRightRoundBracket(this CalculatorToken sourceToken)
        {
            if (sourceToken is null)
            {
                return false;
            }

            return EqualsSymbol(sourceToken.Value, SymbolConstants.RoundBracket.Right);
        }

        /// <summary>
        /// Gets a value indicating there is a bracket token in the target token collection.
        /// </summary>
        /// <param name="tokens">The token collection to check.</param>
        /// <returns>
        /// <c>true</c> if a bracket symbol is found in <paramref name="tokens"/>, otherwise, <c>false</c>.
        /// Returns <c>false</c>, if <paramref name="tokens"/> is <c>null</c>.
        /// </returns>
        public static bool HasBrackets(this IEnumerable<CalculatorToken> tokens)
        {
            if (tokens is null)
            {
                return false;
            }

            foreach (var token in tokens)
            {
                if (EqualsSymbol(token.Value, SymbolConstants.RoundBracket.Right)
                    || EqualsSymbol(token.Value, SymbolConstants.RoundBracket.Left))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating there is a left bracket token in the target token collection.
        /// </summary>
        /// <param name="tokens">The token collection to check.</param>
        /// <returns>
        /// <c>true</c> if a left bracket symbol is found in <paramref name="tokens"/>, otherwise, <c>false</c>.
        /// Returns <c>false</c>, if <paramref name="tokens"/> is <c>null</c>.
        /// </returns>
        public static bool HasLeftBrackets(this IEnumerable<CalculatorToken> tokens)
        {
            if (tokens is null)
            {
                return false;
            }

            foreach (var token in tokens)
            {
                if (EqualsSymbol(token.Value, SymbolConstants.RoundBracket.Left))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Concatenates the given token values to get an expression.
        /// </summary>
        /// <param name="source">The source to concatenate.</param>
        /// <param name="tokenSeparator">The token separator.</param>
        /// <returns>The string obtained by concatenated token values. If <c>source</c> is <c>null</c>, returns an empty string.</returns>
        /// <exception cref="OutOfMemoryException">The length of the resulting string overflows the maximum allowed length (<see cref="Int32.MaxValue"/>).</exception>
        [return: NotNull]
        public static string ToExpressionString(this IEnumerable<CalculatorToken> source, char tokenSeparator = ' ')
        {
            if (source is null)
            {
                return string.Empty;
            }

            return string.Join(tokenSeparator, source.Select(t => t.Value));
        }

        #endregion

        #region Private Methods

        private static bool EqualsSymbol(string tokenValue, string symbol)
        {
            return string.Equals(tokenValue, symbol, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}
