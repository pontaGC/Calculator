using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Utils;
using SimpleCalculator.CalculationLogic.Services;
using SimpleCalculator.CalculationLogic.Test.Mocks;

namespace SimpleCalculator.CalculationLogic.Test.Services
{
    public class RPNCalculateServiceTest
    {
        #region Calculate

        [Fact]
        public void Calculate_ArgumentIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var target = new RPNCalculateService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.Calculate(null));
        }

        [Fact]
        public void Calculate_ArgumentIsEmpty_ReturnZero()
        {
            // Arrange
            const double Epsilon = 5e-15;

            var target = new RPNCalculateService();
            var arg = new Stack<CalculatorToken>();

            // Act
            var actual = target.Calculate(arg);

            // Assert
            Assert.Equal(0.0, actual, Epsilon);
        }

        [Fact]
        public void Calculate_ContainsUnaryOperator_ThrowNotSupportedException()
        {
            // Arrange
            var target = new RPNCalculateService();

            var arg = new Stack<CalculatorToken>();
            arg.Push(new UnaryOperatorMock("++"));
            arg.Push(NumberTokenFactory.Create(2));

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => target.Calculate(arg));
        }

        [Fact]
        public void Calculate_ContainsUnknownMathOperator_ThrowIndexOutOfRangeException()
        {
            // Arrange
            var target = new RPNCalculateService();

            var arg = new Stack<CalculatorToken>();
            arg.Push(new UnknownMathOperatorMock());
            arg.Push(NumberTokenFactory.Create(1));

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => target.Calculate(arg));
        }

        [Fact]
        public void Calculate_NumberTokenLessThan1_ThrowArithmeticException()
        {
            // Arrange
            var target = new RPNCalculateService();
            var symbolTokensMock = new CalculatorSymbolTokensMock().Object;

            var arg = new Stack<CalculatorToken>();
            arg.Push(symbolTokensMock.FindSymbolToken(SymbolConstants.Add));
            arg.Push(NumberTokenFactory.Create(1));

            // Act & Assert
            Assert.Throws<ArithmeticException>(() => target.Calculate(arg));
        }

        [Theory,
        InlineData(4995, "50 100 * 50 10 / + 10 -")] // All binary operator 
        public void Calculate_NormalArgs_ReturnExpectedValue(double expectedValue, string rpnExpresion)
        {
            // Arrange
            const double Epsilon = 5e-15;

            var target = new RPNCalculateService();
            var symbolTokensMock = new CalculatorSymbolTokensMock();

            var expressionTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(rpnExpresion, symbolTokensMock.Object).ToArray();
            var reveresedTokens = expressionTokens.Reverse();
            var arg = new Stack<CalculatorToken>(reveresedTokens);

            // Act
            var actual = target.Calculate(arg);

            // Assert
            Assert.Equal(expectedValue, actual, Epsilon);
        }

        #endregion

        #region UnaryOpeartorMock

        private class UnaryOperatorMock : MathOperator
        {
            internal UnaryOperatorMock(string operatorSymbol) 
                : this (
                      CalculatorTokenPriority.Top,
                      operatorSymbol,
                      MathOperatorType.Unary)
            {
            }

            private UnaryOperatorMock(uint priority, string value, MathOperatorType type) : base(priority, value, type)
            {
            }
        }

        #endregion
    }
}
