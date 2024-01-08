using System.Diagnostics;

namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/> object.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Indicates whether a sequence is <c>null</c> or does not contain any element.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <returns><c>true</c> if a sequence is <c>null</c> or does not contain any element; otherwise, <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source is null || !source.Any();
        }

        /// <summary>
        /// Indicates whether a sequence does not contain any element.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <returns><c>true</c> if a sequence does not contain any element; otherwise, <c>false</c>.</returns>
        [DebuggerStepThrough]
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }
    }
}
