using System.Diagnostics;

namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the given object is <c>null</c>.
        /// </summary>
        /// <param name="target">The object to check.</param>
        /// <param name="parameterName">The name of the parameter argument.</param>
        [DebuggerStepThrough]
        public static void ThrowArgumentNullException(this object target, string parameterName)
        {
            if (target is null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
