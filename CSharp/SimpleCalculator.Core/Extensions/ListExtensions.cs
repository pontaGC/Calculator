namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for List type.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the last occurrence of a specific object from the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="item">The object to remove from the <see cref="IList{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns>
        /// The <c>true</c> if <c>item</c> is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c>
        /// if <c>item</c> was not found in the <see cref="IList{T}"/>.
        /// </returns>
        public static bool RemoveLast<T>(this IList<T> source, T item)
        {
            if (source is null || !source.Any())
            {
                return false;
            }

            if (source.IsReadOnly)
            {
                return false;
            }

            var lastIndex = source.IndexOf(item);
            if (lastIndex < 0)
            {
                // Not found item
                return false;
            }

            try
            {
                source.RemoveAt(lastIndex);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the last element of a specific object from the <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The <c>true</c> if the last element is successfully removed; otherwise, <c>false</c>.
        /// </returns>
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

            try
            {
                source.RemoveAt(source.Count - 1);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
