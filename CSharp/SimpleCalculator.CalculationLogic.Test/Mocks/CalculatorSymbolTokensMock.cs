using Moq;
using SimpleCalculator.CalculationLogic.Core;

namespace SimpleCalculator.CalculationLogic.Test.Mocks
{
    internal class CalculatorSymbolTokensMock
    {
        private readonly Mock<ICalculatorSymbolTokens> mock;

        internal CalculatorSymbolTokensMock()
        {
            this.mock = new Mock<ICalculatorSymbolTokens>();

            var roundBrackets = CreateRoundBrackets();

            this.mock.Setup(x => x.FindSymbolToken(SymbolConstants.RoundBracket.Left)).Returns(roundBrackets.Left);
            this.mock.Setup(x => x.FindSymbolToken(SymbolConstants.RoundBracket.Right)).Returns(roundBrackets.Right);
            this.mock.Setup(x => x.FindSymbolToken(SymbolConstants.Add)).Returns(new AddOperator());
            this.mock.Setup(x => x.FindSymbolToken(SymbolConstants.Subtract)).Returns(new SubtractOperator());
            this.mock.Setup(x => x.FindSymbolToken(SymbolConstants.Multiply)).Returns(new MultiplyOperator());
            this.mock.Setup(x => x.FindSymbolToken(SymbolConstants.Divide)).Returns(new DivideOperator());
        }

        public ICalculatorSymbolTokens Object => this.mock.Object;

        private static (RoundBracketToken Left, RoundBracketToken Right) CreateRoundBrackets()
        {
            return (new LeftRoundBracketToken(), new RightRoundBracketToken());
        }
    }
}
