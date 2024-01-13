using Moq;

using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.CalculationLogic.Core.Utils;
using SimpleCalculator.CalculationLogic.Services;
using SimpleCalculator.CalculationLogic.Test.Mocks;

namespace SimpleCalculator.CalculationLogic.Test.Services
{
    public class InfixNotationCalculateServiceTest
    {
        #region Calcuate

        [Fact]
        public void Calculate_ExpressionIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var expressionConverterMock = new Mock<IExpressionNotationConverter>();
            var rpnCalculateServiceMock = new Mock<IRPNCalculateService>();
            var target = new InfixNotationCalculateService(
                expressionConverterMock.Object, rpnCalculateServiceMock.Object);
                 
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.Calculate(null));
        }

        [Fact]
        public void Calculate_ExpressionContainsUnknownOperator_ThrowArithmeticException()
        {
            // Arrange
            const string InputExpression = "1 | 3";
            var symbolTokenMock = new CalculatorSymbolTokensMock();
            var inputExpressionTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(InputExpression, symbolTokenMock.Object);

            var expressionConverterMock = new Mock<IExpressionNotationConverter>();
            expressionConverterMock.Setup(x => x.ConvertInfixToRPNStack(inputExpressionTokens)).Throws<ArithmeticException>();
            var rpnCalculateServiceMock = new Mock<IRPNCalculateService>();
            var target = new InfixNotationCalculateService(
                expressionConverterMock.Object, rpnCalculateServiceMock.Object);

            // Act & Assert
            Assert.Throws<ArithmeticException>(() => target.Calculate(inputExpressionTokens));
        }

        [Theory,
         InlineData(@"10 + D"), // Contains alphabet
         InlineData(@"1 + ‚Q")] // 2 is multi-byte character
        public void Calculate_ExistUnknownIdentifier_ThrowArithmeticException(string expression)
        {
            // Arrange
            var symbolTokenMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, new CalculatorSymbolTokensMock().Object);

            var expressionConverterMock = new Mock<IExpressionNotationConverter>();
            expressionConverterMock.Setup(x => x.ConvertInfixToRPNStack(tokens)).Throws<ArithmeticException>();
            var rpnCalculateServiceMock = new Mock<IRPNCalculateService>();
            var target = new InfixNotationCalculateService(
                expressionConverterMock.Object, rpnCalculateServiceMock.Object);

            // Act & Arrange
            Assert.Throws<ArithmeticException>(() => target.Calculate(tokens));
        }

        [Theory,
         InlineData("( 1 + 1 ("), // left bracket only
         InlineData(") 2 + 2 )"), // right bracket only
         InlineData("1 + 2 ) + ( 2 + 3"), // unclose 
         InlineData("( 1 + 2 ) + ( 3 + 4"), // More left braket         
         InlineData("( 1 + 2 ) + 3 + 4 )")] // More right braket
        public void Calculate_UncloseBracketExpresion_ThrowArithmeticException(string expression)
        {
            // Arrange
            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            var expressionConverterMock = new Mock<IExpressionNotationConverter>();
            expressionConverterMock.Setup(x => x.ConvertInfixToRPNStack(tokens)).Throws<ArithmeticException>();
            var rpnCalculateServiceMock = new Mock<IRPNCalculateService>();
            var target = new InfixNotationCalculateService(
                expressionConverterMock.Object, rpnCalculateServiceMock.Object);

            // Act & Arrange
            var actual = Assert.Throws<ArithmeticException>(() => target.Calculate(tokens));
        }

        [Fact]
        public void Calculate_EmptyExpressionToken_ReturnZero()
        {
            const double Epsilon = 5.0e-15;

            var tokens = Enumerable.Empty<CalculatorToken>();

            var expressionConverterMock = new Mock<IExpressionNotationConverter>();
            expressionConverterMock.Setup(x => x.ConvertInfixToRPNStack(tokens)).Returns(new Stack<CalculatorToken>());
            var rpnCalculateServiceMock = new Mock<IRPNCalculateService>();
            var target = new InfixNotationCalculateService(
                expressionConverterMock.Object, rpnCalculateServiceMock.Object);

            // Act
            var actual = target.Calculate(tokens);

            // Assert
            Assert.Equal(0.0, actual, Epsilon);
        }

        [Theory,
         InlineData(21, "1 + 20"),
         InlineData(11.58, "10.2 + 1.38"),
         InlineData(67, "100 - 33"),
         InlineData(10.18, "11.48 - 1.3"),
         InlineData(3300, "100 * 33"),
         InlineData(14.924, "11.48 * 1.3"),
         InlineData(5, "100 / 20"),
         InlineData(8.830769230769231, "11.48 / 1.3")]
        public void Calculate_OneBinaryOperator_ReturnCalculatedResut(double expected, string expression)
        {
            // Arrange
            const double Epsilon = 5.0e-15;

            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            var dummyRpnStack = new Stack<CalculatorToken>();
            dummyRpnStack.Push(tokens.Skip(1).First()); // binary operator
            dummyRpnStack.Push(tokens.Skip(2).First());
            dummyRpnStack.Push(tokens.First());

            var expressionConverterMock = new Mock<IExpressionNotationConverter>();
            expressionConverterMock.Setup(x => x.ConvertInfixToRPNStack(It.IsAny<IEnumerable<CalculatorToken>>())).Returns(dummyRpnStack);
            var rpnCalculateServiceMock = new Mock<IRPNCalculateService>();
            rpnCalculateServiceMock.Setup(x => x.Calculate(dummyRpnStack))
                .Returns(() =>
                {
                    var num1 = double.Parse(dummyRpnStack.Pop().Value);
                    var num2 = double.Parse(dummyRpnStack.Pop().Value);
                    var @operator = dummyRpnStack.Pop() as MathOperator;
                    return @operator.BinaryOperate(num1, num2);
                });
            var target = new InfixNotationCalculateService(
                expressionConverterMock.Object, rpnCalculateServiceMock.Object);

            // Act
            var actual = target.Calculate(tokens);

            // Assert
            Assert.Equal(expected, actual, Epsilon);
        }

        #endregion
    }
}