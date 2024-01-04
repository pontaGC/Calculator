namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents an operator to add.
    /// </summary>
    public sealed class AddOperator : MathOperator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddOperator"/> class.
        /// </summary>
        public AddOperator()
            : base(
                  CalculatorTokenPriority.Add,
                  SymbolConstants.Add,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x + y;
        }

        #endregion
    }
}
