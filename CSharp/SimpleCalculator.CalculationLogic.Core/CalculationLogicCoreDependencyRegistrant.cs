using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.Core.Injectors;

namespace SimpleCalculator.CalculateLogic
{
    /// <summary>
    /// The depdency registrant.
    /// </summary>
    public sealed class CalculationLogicCoreDependencyRegistrant : IDependenyRegistrant
    {
        /// <inheritdoc />
        public void Register(IIoCContainer container)
        {
            ArgumentNullException.ThrowIfNull(container);

            container.Register<ICalculatorSymbolTokens, CalculatorSymbolTokens>();
        }
    }
}
