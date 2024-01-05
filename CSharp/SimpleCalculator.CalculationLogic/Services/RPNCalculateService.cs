using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.Core.Extensions;
using SimpleCalculator.Core.Utils;

namespace SimpleCalculator.CalculationLogic.Services
{
    /// <summary>
    /// Responsible for calculating an expression with reverse Polish notation.
    /// </summary>
    internal class RPNCalculateService : IRPNCalculateService
    {
        #region Public Methods

        /// <inheritdoc />
        public double Calculate(Stack<CalculatorToken> rpnStack)
        {
            ArgumentNullException.ThrowIfNull(rpnStack);

            if (rpnStack.IsEmpty())
            {
                return 0.0;
            }

            var workingStack = new Stack<double>();
            while (rpnStack.Any())
            {
                var topToken = rpnStack.Pop();
                if (topToken is NumberToken numberToken)
                {
                    workingStack.Push(numberToken.NumericalValue);
                    continue;
                }

                if (topToken is MathOperator @operator)
                {
                    switch (@operator.OperatorType)
                    {
                        case MathOperatorType.Unary:
                            throw new NotSupportedException("Unary operator is not supported");

                        case MathOperatorType.Binary:
                            var binaryOperateResult = ExecuteBinaryOperator(workingStack, @operator);
                            workingStack.Push(binaryOperateResult);
                            break;

                        default:
                            throw new IndexOutOfRangeException($"Found the unknown operator: {@operator.Value}");
                    }
                }
            }

            return workingStack.Pop();
        }

        #endregion

        #region Private Methods

        private double ExecuteBinaryOperator(Stack<double> numberStack, MathOperator @operator)
        {
            if (numberStack.Count < 2)
            {
                throw new ArithmeticException("Not enough constant terms to execute binary operator.");
            }

            // num1 {binary operator} num2 as infix notation
            var num2 = numberStack.Pop();
            var num1 = numberStack.Pop();
            return @operator.BinaryOperate(num1, num2);
        }

        #endregion
    }


}
