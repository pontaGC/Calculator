using System.Diagnostics.CodeAnalysis;

using SimpleCalculator.Core.Extensions;

namespace SimpleCalculator.CalculationLogic.Core.Utils
{
    /// <summary>
    /// The mathematical expression helper.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Gets all tokens by spliting an expression with whitespaces.
        /// </summary>
        /// <param name="expression">The target expression to get tokens.</param>
        /// <param name="symbolTokens">The defined symbol tokens.</param>
        /// <returns>A collection of split tokens.</returns>
        [return: NotNull]
        public static IEnumerable<CalculatorToken> GetTokensWithWhitespaceSplit(
            string expression,
            ICalculatorSymbolTokens symbolTokens)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return Enumerable.Empty<CalculatorToken>();
            }

            var result = new List<CalculatorToken>();
            foreach (var splitedValue in expression.SplitWithWhitespace())
            {
                var foundSymbol = symbolTokens.FindSymbolToken(splitedValue);
                if (foundSymbol is not null)
                {
                    result.Add(foundSymbol);
                    continue;
                }

                var numberToken = NumberTokenFactory.Create(splitedValue);
                if (numberToken is not null)
                {
                    result.Add(numberToken);
                    continue;
                }

                result.Add(new CalculatorToken(CalculatorTokenPriority.Any, splitedValue, CalculatorTokenType.Unknown));
            }

            return result;
        }
    }
}
