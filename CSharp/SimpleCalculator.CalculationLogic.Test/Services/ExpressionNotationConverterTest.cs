using System.Data;

using SimpleCalculator.CalculationLogic.Core.Extensions;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.CalculationLogic.Core.Utils;
using SimpleCalculator.CalculationLogic.Services;
using SimpleCalculator.CalculationLogic.Test.Mocks;

namespace CalculateLogicTest.Services
{
    public class ExpressionNotationConverterTest
    {
        #region ConvertInfixToRPNStack

        [Fact]
        public void ConvertInfixToRPNStack_ExpressionArgIsNull_ThrowArgumentNullException()
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();

            // Act & Assert
            var nullEx = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    target.ConvertInfixToRPNStack(null);
                });
        }

        [Theory,
         InlineData("10 % 3"), // Has unknown operator
         InlineData(@"1 + 2 { 2 * 3 }")] // Has unknown bracket
        public void ConvertInfixToRPNStack_ExistUnknownSymbol_ThrowSyntaxErrorException(string expression)
        {
            // Arrange
            var symbolTokenMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokenMock.Object);
            IExpressionNotationConverter target = new ExpressionNotationConverter();

            // Act & Assert
            var actualEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    target.ConvertInfixToRPNStack(tokens);
                });
        }

        [Theory,
         InlineData("( 1 + 1 ("), // left bracket only
         InlineData(") 2 + 2 )"), // right bracket only
         InlineData("1 + 2 ) + ( 2 + 3"), // unclose 
         InlineData("( 1 + 2 ) + ( 3 + 4"), // More left braket         
         InlineData("( 1 + 2 ) + 3 + 4 )")] // More right braket
        public void ConvertInfixToRPNStack_InvalidRoundBracketFormat_ThrowSyntaxErrorException(string expression)
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();
            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            // Act & Assert
            var actualEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    target.ConvertInfixToRPNStack(tokens);
                });
        }

        [Theory,
         InlineData(@"10 + D"), // Contains alphabet
         InlineData(@"1 + ２")] // 2 is multi-byte character
        public void ConvertInfixToRPNStack_ExistUnknownIdentifier_ThrowSyntaxErrorException(string expression)
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();
            var symbolTokensMock = new CalculatorSymbolTokensMock();

            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            // Act & Assert
            var actualEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    target.ConvertInfixToRPNStack(tokens);
                });
        }

        [Theory,
         InlineData("1 20 +", "1 + 20"), // Add integer
         InlineData("10.2 1.38 +", "10.2 + 1.38"), // Add real number
         InlineData("100 33 -", "100 - 33"), // Subtract integer
         InlineData("11.48 1.3 -", "11.48 - 1.3"), // Subtract real number
         InlineData("100 33 *", "100 * 33"), // Multiply integer
         InlineData("11.48 1.3 *", "11.48 * 1.3"), // Multiply real number
         InlineData("100 33 /", "100 / 33"), // Divide integer
         InlineData("11.48 1.3 /", "11.48 / 1.3")] // Divide real number
        public void ConvertInfixToRPNStack_SingleOperator_ReturnSimpleRpnExpression(string expected, string expression)
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();
            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            // Act
            var actual = target.ConvertInfixToRPNStack(tokens).ToExpressionString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory,
         InlineData("50 100 + 30 -", "50 + 100 - 30"), // Same priority 
         InlineData("50 100 * 30 /", "50 * 100 / 30"), // Same priority 
         InlineData("50 100 30 * +", "50 + 100 * 30"), // Different priority
         InlineData("50 100 * 30 +", "50 * 100 + 30"), // Different priority
         InlineData("50 100 * 50 10 / + 10 -", "50 * 100 + 50 / 10 - 10")] // All binary operators
        public void ConvertInfixToRPNStack_MultipleOperator_ReturnRPNWithPriority(string expected, string expression)
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();
            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            // Act
            var actual = target.ConvertInfixToRPNStack(tokens).ToExpressionString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory,
         InlineData("50 100 + 30 *", "( 50 +  100 ) * 30"), // Start bracket
         InlineData("50 100 30 + * 10 -", "50 * ( 100 + 30 ) - 10"), // Intermidiate bracket
         InlineData("30 50 100 + /", "30 / ( 50 +  100 )"), // End bracket
         InlineData("10 20 + 110 10 - *", "( 10 + 20 ) * ( 110 - 10 )")] // Multi bracket
        public void ConvertInfixToRPNStack_ContainsRoundBracket_ReturnRPNWithPriority(string expected, string expression)
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();
            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            // Act
            var actual = target.ConvertInfixToRPNStack(tokens).ToExpressionString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory,
         InlineData("50 100 + 60 * 30 *", "( ( 50 +  100 ) * 60 ) * 30"), // Sequential open bracket
         InlineData("50 100 30 + * 10 / 10 /", "( 50 * ( 100 + 30 ) / 10 ) / 10"), // Intermidiate bracket
         InlineData("10 50 100 + * 20 *", "( 10 * ( 50 +  100 ) ) * 20"), // Sequential close bracket
         InlineData("3 6 3 + 3 / 10 * /", "3 / ( ( ( 6 + 3 ) / 3 ) * 10 )")] // Multiple nest brackets
        public void ConvertInfixToRPNStack_ContainsNestedRoundBracket_ReturnRPNWithPriority(string expected, string expression)
        {
            // Arrange
            IExpressionNotationConverter target = new ExpressionNotationConverter();
            var symbolTokensMock = new CalculatorSymbolTokensMock();
            var tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(expression, symbolTokensMock.Object);

            // Act
            var actual = target.ConvertInfixToRPNStack(tokens).ToExpressionString();

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
