using SimpleCalculator.CalculationLogic.Core;

namespace SimpleCalculator.CalculationLogic.Test.Mocks
{
    internal class UnknownMathOperatorMock : MathOperator
    {
        internal UnknownMathOperatorMock()
            : this(
                  CalculatorTokenPriority.Top,
                  "\\\\",
                  MathOperatorType.Unknown)
        {
        }

        private UnknownMathOperatorMock(uint priority, string value, MathOperatorType type) : base(priority, value, type)
        {
        }
    }
}
