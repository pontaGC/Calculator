using System.Diagnostics;
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
        /// Throws the <see cref="ArgumentNullException"/>
        /// if <paramref name="source"/> is <c>null</c> or empty.
        /// </summary>
        /// <param name="source">The string to check.</param>
        /// <param name="parameterName">The parameter name of the string to check.</param>
        [DebuggerStepThrough]
        public static void ThrowArgumentNullOrEmptyException(this string source, string parameterName)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws the <see cref="ArgumentNullException"/>
        /// if <paramref name="source"/> is <c>null</c> or empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="source">The string to check.</param>
        /// <param name="parameterName">The parameter name of the string to check.</param>
        [DebuggerStepThrough]
        public static void ThrowArgumentNullOrWhitespaceException(this string source, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Gets a last character in the given string.
        /// </summary>
        /// <param name="source">The string to get.</param>
        /// <returns>
        /// An empty string if <c>source</c> is <c>null</c> or an empty string,
        /// otherwise, returns the last character.
        /// </returns>
        [DebuggerStepThrough]
        [return: NotNull]
        public static string GetLastCharacter(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            return source.Last().ToString();
        }

        /// <summary>
        /// Removes one target string at end of the given string.
        /// </summary>
        /// <param name="source">The source string to remove.</param>
        /// <param name="removeString">The removing target string.</param>
        /// <returns>A new string after removing the <paramref name="removeString"/> at end.</returns>
        [return: NotNull]
        public static string RemoveEndOnce(this string source, string removeString)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(removeString))
            {
                return string.Empty;
            }

            var lastIndex = source.LastIndexOf(removeString);
            if (lastIndex < 0)
            {
                // Not found the removing target string
                return source;
            }

            return source.Remove(lastIndex, removeString.Length);
        }

        /// <summary>
        /// Removes all whitespaces in the target string.
        /// </summary>
        /// <param name="target">The target string.</param>
        /// <returns>The string with all whitespaces removed.</returns>
        [return: NotNull]
        public static string RemoveAllWhitespaces(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            return Regex.Replace(target, "\\s", string.Empty);
        }

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
