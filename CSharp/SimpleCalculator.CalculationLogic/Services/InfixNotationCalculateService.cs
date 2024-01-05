using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Services;

namespace SimpleCalculator.CalculationLogic.Services
{
    /// <summary>
    /// Responsible for dealing with an expression written by infix notation.
    /// </summary>
    internal class InfixNotationCalculateService : IInfixNotationCalculateService
    {
        #region Fields

        private readonly IExpressionNotationConverter expressionNotationConverter;
        private readonly IRPNCalculateService rpnCalculateService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InfixNotationCalculateService"/> class.
        /// </summary>
        /// <param name="expressionNotationConverter">The expression notation converter.</param>
        /// <param name="rpnCalculateService">The RPN calculate service.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="expressionNotationConverter"/> or <paramref name="rpnCalculateService"/> is <c>null</c>.</exception>
        public InfixNotationCalculateService(
            IExpressionNotationConverter expressionNotationConverter,
            IRPNCalculateService rpnCalculateService)
        {
            ArgumentNullException.ThrowIfNull(expressionNotationConverter);
            ArgumentNullException.ThrowIfNull(rpnCalculateService);

            this.expressionNotationConverter = expressionNotationConverter;
            this.rpnCalculateService = rpnCalculateService;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Calculate(IEnumerable<CalculatorToken> infixExpressionTokens)
        {
            ArgumentNullException.ThrowIfNull(infixExpressionTokens);

            try
            {
                var rpnStack = this.expressionNotationConverter.ConvertInfixToRPNStack(infixExpressionTokens);
                return this.rpnCalculateService.Calculate(rpnStack);
            }
            catch (Exception e)
            {
                throw new ArithmeticException("Failed to calculate the given expression.", e);
            }
        }

        #endregion
    }
}
