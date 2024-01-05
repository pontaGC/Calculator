using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.CalculationLogic.Services;
using SimpleCalculator.Core.Injectors;

namespace SimpleCalculator.CalculateLogic
{
    /// <summary>
    /// The depdency registrant.
    /// </summary>
    public sealed class CalculatorLogicDependencyRegistrant : IDependencyRegistrant
    {
        /// <inheritdoc />
        public void RegisterServices(IIoCContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Register<IExpressionNotationConverter, ExpressionNotationConverter>();
            container.Register<IRPNCalculateService, RPNCalculateService>();
            container.Register<IInfixNotationCalculateService, InfixNotationCalculateService>();
        }
    }
}
