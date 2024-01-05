namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents an operator to subtract.
    /// </summary>
    internal sealed class SubtractOperator : MathOperator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractOperator"/> class.
        /// </summary>
        public SubtractOperator()
            : base(
                  CalculatorTokenPriority.Subtract,
                  SymbolConstants.Subtract,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x - y;
        }

        #endregion
    }
}
