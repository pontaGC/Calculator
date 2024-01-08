using System.Data;

using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Extensions;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.CalculationLogic.Core.Utils;
using SimpleCalculator.CalculationLogic.Services;
using SimpleCalculator.CalculationLogic.Test;

namespace CalculateLogicTest.Services
{
    public class ExpressionNotationConverterTest
    {
        private readonly ICalculatorSymbolTokens calculatorSymbolTokens;
        private readonly IExpressionNotationConverter target;

        public ExpressionNotationConverterTest()
        {
            var container = ContainerProvider.GetContainer();
            this.calculatorSymbolTokens = container.Resolve<ICalculatorSymbolTokens>();
            this.target = new ExpressionNotationConverter();
        }

        #region ConvertInfixToRPNStack

        [Fact]
        public void ConvertInfixToRPNStack_ExpressionArgIsNull_ThrowArgumentNullException()
        {
            // Arrange

            // Act & Assert
            var nullEx = Assert.Throws<ArgumentNullException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(null);
                });
        }

        [Fact]
        public void ConvertInfixToRPNStack_ExistUnknownSymbol_ThrowSyntaxErrorException()
        {
            // Arrange
            const string UnknownOperator = @"10 % 3";
            const string UnknownBracket = @"1 + 2 { 2 * 3 }";

            var unknownOperatorTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(UnknownOperator, this.calculatorSymbolTokens);
            var unknownBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(UnknownBracket, this.calculatorSymbolTokens);

            // Act & Assert
            var unknownOperatorEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(unknownOperatorTokens);
                });

            var unknownBracketEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(unknownBracketTokens);
                });
        }

        [Fact]
        public void ConvertInfixToRPNStack_InvalidRoundBracketFormat_ThrowSyntaxErrorException()
        {
            // Arrange
            const string LeftBracketOnly = "( 1 + 1 (";
            const string RightBracketOnly = ") 2 + 2 )";
            const string UncloseBracket = "1 + 2 ) + ( 2 + 3";
            const string ManyLeftBracket = "( 1 + 2 ) + ( 3 + 4";
            const string ManyRightBracket = "( 1 + 2 ) + 3 + 4 )";

            var leftBracketOnlyTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(LeftBracketOnly, this.calculatorSymbolTokens);
            var rightBracketOnlyTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(RightBracketOnly, this.calculatorSymbolTokens);
            var uncloseBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(UncloseBracket, this.calculatorSymbolTokens);
            var manyLeftBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(ManyLeftBracket, this.calculatorSymbolTokens);
            var manyRightBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(ManyRightBracket, this.calculatorSymbolTokens);

            // Act & Assert
            var leftBracketOnlyEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(leftBracketOnlyTokens);
                });

            var rightBracketOnlyEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(rightBracketOnlyTokens);
                });

            var uncloseBracketEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(uncloseBracketTokens);
                });

            var manyLeftBracketEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(manyLeftBracketTokens);
                });

            var manyRightBracketEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(manyRightBracketTokens);
                });
        }

        [Fact]
        public void ConvertInfixToRPNStack_ExistUnknownIdentifier_ThrowSyntaxErrorException()
        {
            // Arrange
            const string ExpressionWithAlphabet = @"10 + D";
            const string ExpressionWithMultiByteCharacter = @"1 + ２"; // 2 is multi-byte character

            var tokensWithAlphabet = ExpressionHelper.GetTokensWithWhitespaceSplit(ExpressionWithAlphabet, this.calculatorSymbolTokens);
            var tokensWithMultiByte = ExpressionHelper.GetTokensWithWhitespaceSplit(ExpressionWithMultiByteCharacter, this.calculatorSymbolTokens);

            // Act & Assert
            var alphabetStringEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(tokensWithAlphabet);
                });

            var multiByteStringEx = Assert.Throws<SyntaxErrorException>(
                () =>
                {
                    this.target.ConvertInfixToRPNStack(tokensWithMultiByte);
                });
        }

        [Fact]
        public void ConvertInfixToRPNStack_SingleOperator_ReturnSimpleRpnExpression()
        {
            // Arrange
            const string AddIntegersExpression = "1 + 20";
            const string AddRealNumbersExpression = "10.2 + 1.38";
            const string SubtractIntegersExpression = "100 - 33";
            const string SubtractRealNumbersExpression = "11.48 - 1.3";
            const string MultiplyIntegersExpression = "100 33 *";
            const string MultiplyRealNumbersExpression = "11.48 * 1.3";
            const string DivideIntegersExpression = "100 / 33";
            const string DivideRealNumbersExpression = "11.48 / 1.3";

            var addIntegerTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(AddIntegersExpression, this.calculatorSymbolTokens);
            var addRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(AddRealNumbersExpression, this.calculatorSymbolTokens);
            var subtractIntegersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SubtractIntegersExpression, this.calculatorSymbolTokens);
            var subtractRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SubtractRealNumbersExpression, this.calculatorSymbolTokens);
            var multiplyIntegersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(MultiplyIntegersExpression, this.calculatorSymbolTokens);
            var multiplyRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(MultiplyRealNumbersExpression, this.calculatorSymbolTokens);
            var divideIntegersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DivideIntegersExpression, this.calculatorSymbolTokens);
            var divideRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DivideRealNumbersExpression, this.calculatorSymbolTokens);

            // Act
            var addIntegersActual = this.target.ConvertInfixToRPNStack(addIntegerTokens).ToExpressionString();
            var addRealNumbersActual = this.target.ConvertInfixToRPNStack(addRealNumbersTokens).ToExpressionString();
            var subtractIntegersActual = this.target.ConvertInfixToRPNStack(subtractIntegersTokens).ToExpressionString();
            var subractRealNumbersActual = this.target.ConvertInfixToRPNStack(subtractRealNumbersTokens).ToExpressionString();
            var multiplyIntegersActual = this.target.ConvertInfixToRPNStack(multiplyIntegersTokens).ToExpressionString();
            var multiplyRealNumbersActual = this.target.ConvertInfixToRPNStack(multiplyRealNumbersTokens).ToExpressionString();
            var divideIntegersActual = this.target.ConvertInfixToRPNStack(divideIntegersTokens).ToExpressionString();
            var divideRealNumbersActual = this.target.ConvertInfixToRPNStack(divideRealNumbersTokens).ToExpressionString();

            // Assert
            Assert.Equal("1 20 +", addIntegersActual);
            Assert.Equal("10.2 1.38 +", addRealNumbersActual);
            Assert.Equal("100 33 -", subtractIntegersActual);
            Assert.Equal("11.48 1.3 -", subractRealNumbersActual);
            Assert.Equal("100 33 *", multiplyIntegersActual);
            Assert.Equal("11.48 1.3 *", multiplyRealNumbersActual);
            Assert.Equal("100 33 /", divideIntegersActual);
            Assert.Equal("11.48 1.3 /", divideRealNumbersActual);
        }

        [Fact]
        public void ConvertInfixToRPNStack_MultipleOperator_ReturnRPNWithPriority()
        {
            // Arrange
            const string SamePriorityExpression1 = "50 + 100 - 30";
            const string SamePriorityExpression2 = "50 * 100 / 30";
            const string DifferentPriorityExpression1 = "50 + 100 * 30";
            const string DifferentPriorityExpression2 = "50 * 100 + 30";
            const string AllOperatorExpression = "50 * 100 + 50 / 10 - 10";

            var samePriority1Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SamePriorityExpression1, this.calculatorSymbolTokens);
            var samePriority2Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SamePriorityExpression2, this.calculatorSymbolTokens);
            var differentPriority1Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DifferentPriorityExpression1, this.calculatorSymbolTokens);
            var differentPriority2Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DifferentPriorityExpression2, this.calculatorSymbolTokens);
            var allOperatorTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(AllOperatorExpression, this.calculatorSymbolTokens);

            // Act
            var samePriorityExp1 = this.target.ConvertInfixToRPNStack(samePriority1Tokens).ToExpressionString();
            var samePriorityExp2 = this.target.ConvertInfixToRPNStack(samePriority2Tokens).ToExpressionString();
            var DiffPriorityExp1 = this.target.ConvertInfixToRPNStack(differentPriority1Tokens).ToExpressionString();
            var DiffPriorityExp2 = this.target.ConvertInfixToRPNStack(differentPriority2Tokens).ToExpressionString();
            var allPriorityExp = this.target.ConvertInfixToRPNStack(allOperatorTokens).ToExpressionString();

            // Assert
            Assert.Equal("50 100 + 30 -", samePriorityExp1);
            Assert.Equal("50 100 * 30 /", samePriorityExp2);
            Assert.Equal("50 100 30 * +", DiffPriorityExp1);
            Assert.Equal("50 100 * 30 +", DiffPriorityExp2);
            Assert.Equal("50 100 * 50 10 / + 10 -", allPriorityExp);
        }

        [Fact]
        public void ConvertInfixToRPNStack_ContainsRoundBracket_ReturnRPNWithPriority()
        {
            // Arrange
            const string StartBracketExpression = "( 50 +  100 ) * 30";
            const string IntermidiateBracketExpression = "50 * ( 100 + 30 ) - 10";
            const string EndBracketExpression = "( 50 +  100 ) / 30";
            const string MultiBracketExpression = "( 10 + 20 ) * ( 110 - 10 )";

            var startBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(StartBracketExpression, this.calculatorSymbolTokens);
            var intermidiateBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(IntermidiateBracketExpression, this.calculatorSymbolTokens);
            var endBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(EndBracketExpression, this.calculatorSymbolTokens);
            var multiBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(MultiBracketExpression, this.calculatorSymbolTokens);

            // Act
            var startBracketExp = this.target.ConvertInfixToRPNStack(startBracketTokens).ToExpressionString();
            var intermidiateBracketExp = this.target.ConvertInfixToRPNStack(intermidiateBracketTokens).ToExpressionString();
            var endBracketExpression = this.target.ConvertInfixToRPNStack(endBracketTokens).ToExpressionString();
            var multiBracketExp = this.target.ConvertInfixToRPNStack(multiBracketTokens).ToExpressionString();

            // Assert
            Assert.Equal("50 100 + 30 *", startBracketExp);
            Assert.Equal("50 100 30 + * 10 -", intermidiateBracketExp);
            Assert.Equal("50 100 + 30 /", endBracketExpression);
            Assert.Equal("10 20 + 110 10 - *", multiBracketExp);
        }

        [Fact]
        public void ConvertInfixToRPNStack_ContainsNestedRoundBracket_ReturnRPNWithPriority()
        {
            // Arrange
            const string SequentialOpenBracketExpression = "( ( 50 +  100 ) * 60 ) * 30";
            const string IntermidiateBracketExpression = "( 50 * ( 100 + 30 ) / 10 ) / 10";
            const string SequentialCloseBracketExpression = "( 10 * ( 50 +  100 ) ) * 20";
            const string MultipleNestBracketExpression = "3 / ( ( ( 6 + 3 ) / 3 ) * 10 )";

            var sequentialOpenBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SequentialOpenBracketExpression, this.calculatorSymbolTokens);
            var IntermidiateBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(IntermidiateBracketExpression, this.calculatorSymbolTokens);
            var sequentialCloseBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SequentialCloseBracketExpression, this.calculatorSymbolTokens);
            var multipleNestBracketToken = ExpressionHelper.GetTokensWithWhitespaceSplit(MultipleNestBracketExpression, this.calculatorSymbolTokens);

            // Act
            var sequntialOpenBracketExp = this.target.ConvertInfixToRPNStack(sequentialOpenBracketTokens).ToExpressionString();
            var intermidiateBracketExp = this.target.ConvertInfixToRPNStack(IntermidiateBracketTokens).ToExpressionString();
            var sequentialCloseBracketExp = this.target.ConvertInfixToRPNStack(sequentialCloseBracketTokens).ToExpressionString();
            var multipleNestExpression = this.target.ConvertInfixToRPNStack(multipleNestBracketToken).ToExpressionString();

            // Assert
            Assert.Equal("50 100 + 60 * 30 *", sequntialOpenBracketExp);
            Assert.Equal("50 100 30 + * 10 / 10 /", intermidiateBracketExp);
            Assert.Equal("10 50 100 + * 20 *", sequentialCloseBracketExp);
            Assert.Equal("3 6 3 + 3 / 10 * /", multipleNestExpression);
        }

        #endregion
    }
}
