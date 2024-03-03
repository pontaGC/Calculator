using System.Diagnostics;

namespace SimpleCalculator.Core.Extensions
{
    /// <summary>
    /// Extension methods for an <c>Exception</c>.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets an exception message.
        /// If the given exception has an inner exception,
        /// returns the message that contains the inner exception message.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The exception message, if <c>exception</c> is not <c>null</c>, otherwise, retruns an empty string.</returns>
        [DebuggerStepThrough]
        public static string GetMessage(this Exception exception)
        {
            if (exception is null)
            {
                return string.Empty;
            }

            return exception.InnerException is null
                ? exception.Message
                : $"{exception.Message}, {exception.InnerException.Message}";
        }
    }
}
