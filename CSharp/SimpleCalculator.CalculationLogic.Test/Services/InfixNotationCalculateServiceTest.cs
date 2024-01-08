using Moq;

using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.CalculationLogic.Core.Utils;
using SimpleCalculator.CalculationLogic.Services;

namespace SimpleCalculator.CalculationLogic.Test.Services
{
    public class InfixNotationCalculateServiceTest
    {
        private readonly ICalculatorSymbolTokens calculatorSymbolTokens;
        private readonly IInfixNotationCalculateService target;

        public InfixNotationCalculateServiceTest()
        {
            var container = ContainerProvider.GetContainer();
            this.calculatorSymbolTokens = container.Resolve<ICalculatorSymbolTokens>();
        }

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

            var inputExpressionTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(InputExpression, calculatorSymbolTokens);

            // Act & Assert
            Assert.Throws<ArithmeticException>(() => target.Calculate(inputExpressionTokens));
        }

        [Fact]
        public void Calculate_ExistUnknownIdentifier_ThrowArithmeticException()
        {
            // Arrange
            const string ExpressionWithAlphabet = @"10 + D";
            const string ExpressionWithMultiByteCharacter = @"1 + ‚Q"; // 2 is multi-byte character

            var tokensWithAlphabet = ExpressionHelper.GetTokensWithWhitespaceSplit(ExpressionWithAlphabet, calculatorSymbolTokens);
            var tokensWithMultiByte = ExpressionHelper.GetTokensWithWhitespaceSplit(ExpressionWithMultiByteCharacter, calculatorSymbolTokens);

            // Act & Arrange
            var alphabetStringActual = Assert.Throws<ArithmeticException>(() => target.Calculate(tokensWithAlphabet));
            var multiByteStringActual = Assert.Throws<ArithmeticException>(() => target.Calculate(tokensWithMultiByte));
        }

        [Fact]
        public void Calculate_UncloseBracketExpresion_ThrowArithmeticException()
        {
            // Arrange
            const string LeftBracketOnly = "( 1 + 1 (";
            const string RightBracketOnly = ") 2 + 2 )";
            const string UncloseBracket = "1 + 2 ) + ( 2 + 3";
            const string ManyLeftBracket = "( 1 + 2 ) + ( 3 + 4";
            const string ManyRightBracket = "( 1 + 2 ) + 3 + 4 )";

            var leftBracketOnlyTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(LeftBracketOnly, calculatorSymbolTokens);
            var rightBracketOnlyTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(RightBracketOnly, calculatorSymbolTokens);
            var uncloseBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(UncloseBracket, calculatorSymbolTokens);
            var manyLeftBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(ManyLeftBracket, calculatorSymbolTokens);
            var manyRightBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(ManyRightBracket, calculatorSymbolTokens);

            // Act & Arrange
            var leftBracktOnlyActual = Assert.Throws<ArithmeticException>(() => target.Calculate(leftBracketOnlyTokens));
            var rightBracktOnlyActual = Assert.Throws<ArithmeticException>(() => target.Calculate(rightBracketOnlyTokens));
            var uncloseBracketActual = Assert.Throws<ArithmeticException>(() => target.Calculate(uncloseBracketTokens));
            var manyLeftBracketActual = Assert.Throws<ArithmeticException>(() => target.Calculate(manyLeftBracketTokens));
            var manyRightBracketActual = Assert.Throws<ArithmeticException>(() => target.Calculate(manyRightBracketTokens));
        }

        [Fact]
        public void Calculate_OneBinaryOperator_ReturnCalculatedResut()
        {
            // Arrange
            const double Epsilon = 5.0e-15;

            const string AddIntegersExpression = "1 + 20";
            const string AddRealNumbersExpression = "10.2 + 1.38";
            const string SubtractIntegersExpression = "100 - 33";
            const string SubtractRealNumbersExpression = "11.48 - 1.3";
            const string MultiplyIntegersExpression = "100 33 *";
            const string MultiplyRealNumbersExpression = "11.48 * 1.3";
            const string DivideIntegersExpression = "100 / 20";
            const string DivideRealNumbersExpression = "11.48 / 1.3";

            var addIntegerTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(AddIntegersExpression, calculatorSymbolTokens);
            var addRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(AddRealNumbersExpression, calculatorSymbolTokens);
            var subtractIntegersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SubtractIntegersExpression, calculatorSymbolTokens);
            var subtractRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SubtractRealNumbersExpression, calculatorSymbolTokens);
            var multiplyIntegersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(MultiplyIntegersExpression, calculatorSymbolTokens);
            var multiplyRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(MultiplyRealNumbersExpression, calculatorSymbolTokens);
            var divideIntegersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DivideIntegersExpression, calculatorSymbolTokens);
            var divideRealNumbersTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DivideRealNumbersExpression, calculatorSymbolTokens);

            // Act
            var addIntegersActual = target.Calculate(addIntegerTokens);
            var addRealNumbersActual = target.Calculate(addRealNumbersTokens);
            var subtractIntegersActual = target.Calculate(subtractIntegersTokens);
            var subractRealNumbersActual = target.Calculate(subtractRealNumbersTokens);
            var multiplyIntegersActual = target.Calculate(multiplyIntegersTokens);
            var multiplyRealNumbersActual = target.Calculate(multiplyRealNumbersTokens);
            var divideIntegersActual = target.Calculate(divideIntegersTokens);
            var divideRealNumbersActual = target.Calculate(divideRealNumbersTokens);

            // Assert
            Assert.Equal(21, (int)addIntegersActual);
            Assert.Equal(11.58, addRealNumbersActual, Epsilon);
            Assert.Equal(67, (int)subtractIntegersActual);
            Assert.Equal(10.18, subractRealNumbersActual, Epsilon);
            Assert.Equal(3300, (int)multiplyIntegersActual);
            Assert.Equal(14.924, multiplyRealNumbersActual, Epsilon);
            Assert.Equal(5, (int)divideIntegersActual);
            Assert.Equal(8.830769230769231, divideRealNumbersActual, Epsilon);
        }

        [Fact]
        public void Calculate_MultipleOperator_ReturnCalculatedResult()
        {
            // Arrange
            const double Epsilon = 5.0E-15;

            const string SamePriorityExpression1 = "50 + 100 - 30";
            const string SamePriorityExpression2 = "50 * 100 / 30";
            const string DifferentPriorityExpression1 = "50 + 100 * 30";
            const string DifferentPriorityExpression2 = "50 * 100 + 30";
            const string AllOperatorExpression = "50 * 100 + 50 / 10 - 10";

            var samePriority1Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SamePriorityExpression1, calculatorSymbolTokens);
            var samePriority2Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SamePriorityExpression2, calculatorSymbolTokens);
            var differentPriority1Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DifferentPriorityExpression1, calculatorSymbolTokens);
            var differentPriority2Tokens = ExpressionHelper.GetTokensWithWhitespaceSplit(DifferentPriorityExpression2, calculatorSymbolTokens);
            var allOperatorTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(AllOperatorExpression, calculatorSymbolTokens);

            // Act
            var samePriority1Actual = target.Calculate(samePriority1Tokens);
            var samePriority2Actual = target.Calculate(samePriority2Tokens);
            var DiffPriority1Actual = target.Calculate(differentPriority1Tokens);
            var DiffPriority2Actual = target.Calculate(differentPriority2Tokens);
            var allOperatorActual = target.Calculate(allOperatorTokens);

            // Assert
            Assert.Equal(120, (int)samePriority1Actual);
            Assert.Equal(166.666666666666667, samePriority2Actual, Epsilon);
            Assert.Equal(3050, (int)DiffPriority1Actual);
            Assert.Equal(5030, (int)DiffPriority2Actual);
            Assert.Equal(4995, (int)allOperatorActual);
        }

        [Fact]
        public void Calculate_ContainsRoundBracket_ReturnCorrectResult()
        {
            // Arrange
            const double Epsilon = 5.0E-15;

            const string StartBracketExpression = "( 50 +  100 ) * 30";
            const string IntermidiateBracketExpression = "50 * ( 100 + 30 ) - 10";
            const string EndBracketExpression = "( 50 +  100 ) / 30";
            const string MultiBracketExpression = "( 10 + 20 ) * ( 110 - 10 ) / 4000";

            var startBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(StartBracketExpression, calculatorSymbolTokens);
            var intermidiateBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(IntermidiateBracketExpression, calculatorSymbolTokens);
            var endBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(EndBracketExpression, calculatorSymbolTokens);
            var multiBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(MultiBracketExpression, calculatorSymbolTokens);

            // Act
            var startBracketActual = target.Calculate(startBracketTokens);
            var intermidiateBracketActual = target.Calculate(intermidiateBracketTokens);
            var endBracketActual = target.Calculate(endBracketTokens);
            var multiBracketActual = target.Calculate(multiBracketTokens);

            // Assert
            Assert.Equal(4500, (int)startBracketActual);
            Assert.Equal(6490, (int)intermidiateBracketActual);
            Assert.Equal(5, (int)endBracketActual);
            Assert.Equal(0.75, multiBracketActual, Epsilon);
        }

        [Fact]
        public void Calculate_ContainsNestedRoundBracket_ReturnCorrectResult()
        {
            // Arrange
            const double Epsilon = 5.0e-15;

            const string SequentialOpenBracketExpression = "( ( 50 +  100 ) * 60 ) * 30";
            const string IntermidiateBracketExpression = "( 50 * ( 100 + 30 ) / 10 ) / 10";
            const string SequentialCloseBracketExpression = "( 10 * ( 50 +  100 ) ) * 20";
            const string MultipleNestBracketExpression = "3 / ( ( ( 6 + 3 ) / 3 ) * 10 )";

            var sequentialOpenBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SequentialOpenBracketExpression, calculatorSymbolTokens);
            var IntermidiateBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(IntermidiateBracketExpression, calculatorSymbolTokens);
            var sequentialCloseBracketTokens = ExpressionHelper.GetTokensWithWhitespaceSplit(SequentialCloseBracketExpression, calculatorSymbolTokens);
            var multipleNestBracketToken = ExpressionHelper.GetTokensWithWhitespaceSplit(MultipleNestBracketExpression, calculatorSymbolTokens);

            // Act
            var sequntialOpenBracketActual = target.Calculate(sequentialOpenBracketTokens);
            var intermidiateBracketActual = target.Calculate(IntermidiateBracketTokens);
            var sequentialCloseBracketActual = target.Calculate(sequentialCloseBracketTokens);
            var multipleNestBracketActual = target.Calculate(multipleNestBracketToken);

            // Assert
            Assert.Equal(270000, (int)sequntialOpenBracketActual);
            Assert.Equal(65, (int)intermidiateBracketActual);
            Assert.Equal(30000, (int)sequentialCloseBracketActual);
            Assert.Equal(0.1, multipleNestBracketActual, Epsilon);
        }

        #endregion
    }
}