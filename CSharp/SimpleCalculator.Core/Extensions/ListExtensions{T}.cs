using System.Diagnostics;

namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for List type.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the last element of a specific object from the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The <c>true</c> if the last element is successfully removed; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool RemoveLast<T>(this IList<T> source)
        {
            if (source is null || !source.Any())
            {
                return false;
            }

            if (source.IsReadOnly)
            {
                return false;
            }

            var lastIndex = source.Count - 1;
            if (lastIndex < 0)
            {
                return false;
            }

            source.RemoveAt(lastIndex);
            return true;
        }
    }
}
