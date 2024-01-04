using System.Globalization;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// The number calculator token.
    /// </summary>
    public class NumberToken : CalculatorToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberToken"/> class.
        /// </summary>
        /// <param name="value">The numerical value.</param>
        internal NumberToken(double value)
            : this(value.ToString(CultureInfo.InvariantCulture))
        {
            NumericalValue = value;
        }

        /// <inheritdoc />
        private NumberToken(string value)
            : base(
                  CalculatorTokenPriority.Any,
                  value,
                  CalculatorTokenType.Number
                  )
        {
        }

        /// <summary>
        /// Gets a numerical value.
        /// </summary>
        public double NumericalValue { get; } = double.NaN;
    }
}
