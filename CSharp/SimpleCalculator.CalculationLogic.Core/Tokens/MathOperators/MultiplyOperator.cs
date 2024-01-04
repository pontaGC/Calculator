namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents an operator to multiply.
    /// </summary>
    public sealed class MultiplyOperator : MathOperator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiplyOperator"/> class.
        /// </summary>
        public MultiplyOperator()
            : base(
                  CalculatorTokenPriority.Multiply,
                  SymbolConstants.Multiply,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x * y;
        }

        #endregion
    }
}
