using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for <c>string</c> type.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Splits the given string with whitespace.
        /// </summary>
        /// <param name="source">The string to split.</param>
        /// <returns>A collection of the split string with whitespace.</returns>
        [return: NotNull]
        public static IEnumerable<string> SplitWithWhitespace(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return Enumerable.Empty<string>();
            }

            // Extracts non-whitespace characters
            return Regex.Matches(source, "\\S+").OfType<Match>().Select(m => m.Value);
        }
    }
}
