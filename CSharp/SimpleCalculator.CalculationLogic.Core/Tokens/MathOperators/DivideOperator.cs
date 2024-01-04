using SimpleCalculator.CalculateLogic.Core.Constants;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents an operator to divide.
    /// </summary>
    public sealed class DivideOperator : MathOperator
    {
        #region Constractors

        /// <summary>
        /// Initializes a new instance of the <see cref="DivideOperator"/> class.
        /// </summary>
        public DivideOperator()
            : base(
                  CalculatorTokenPriority.Divide,
                  SymbolConstants.Divide,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x / y;
        }

        #endregion
    }
}
