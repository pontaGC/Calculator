using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.CalculationLogic.Services;
using SimpleCalculator.Core.Injectors;

namespace SimpleCalculator.CalculateLogic
{
    /// <summary>
    /// The depdency registrant.
    /// </summary>
    public sealed class CalculationLogicDependencyRegistrant : IDependenyRegistrant
    {
        /// <inheritdoc />
        public void Register(IIoCContainer container)
        {
            ArgumentNullException.ThrowIfNull(container);

            container.Register<IExpressionNotationConverter, ExpressionNotationConverter>(InstanceLifeStyle.Singleton);
            container.Register<IRPNCalculateService, RPNCalculateService>(InstanceLifeStyle.Singleton);
            container.Register<IInfixNotationCalculateService, InfixNotationCalculateService>(InstanceLifeStyle.Singleton);
        }
    }
}
