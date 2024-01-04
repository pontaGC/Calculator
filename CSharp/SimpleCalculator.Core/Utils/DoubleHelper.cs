using System.Globalization;

namespace SimpleCalculator.Core.Utils
{
    /// <summary>
    /// The utility class for the double struct type.
    /// </summary>
    public static class DoubleHelper
    {
        private const NumberStyles DefaultNumberStyle = NumberStyles.AllowThousands | NumberStyles.Float;

        /// <summary>
        /// Try to convert the given string to double value.
        /// </summary>
        /// <param name="input">The string to covnert.</param>
        /// <param name="result">The double value after converting.</param>
        /// <returns>
        /// <c>true</c> if converting <paramref name="input"/> to the double value is successful,
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool TryParseInvariantCulture(string input, out double result)
        {
            return double.TryParse(
                input,
                DefaultNumberStyle,
                CultureInfo.InvariantCulture,
                out result);
        }
    }
}
