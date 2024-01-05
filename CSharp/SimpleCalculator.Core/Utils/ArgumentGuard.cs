using System.Diagnostics;

namespace SimpleCalculator.Core.Utils
{
    /// <summary>
    /// The guard to safisfy the argments' requirement.
    /// </summary>
    public static class ArgumentGuard
    {
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the given object is <c>null</c>.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameterName">The name of the parameter argument.</param>
        [DebuggerStepThrough]
        public static void RequireNotNull<T>(T parameter, string parameterName)
            where T : class
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the given object is <c>null</c>.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <param name="parameterName">The name of the parameter argument.</param>
        [DebuggerStepThrough]
        public static void RequireNotNull<T>(T? parameter, string parameterName)
            where T : struct // Prevents boxing 
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
