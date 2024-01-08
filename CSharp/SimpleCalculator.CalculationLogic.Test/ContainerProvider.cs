using SimpleCalculator.CalculateLogic;
using SimpleCalculator.Core.Injectors;

namespace SimpleCalculator.CalculationLogic.Test
{
    internal class ContainerProvider
    {
        static ContainerProvider()
        {
            Container = ContainerFactory.Create();

            var serviceRegistrant = new CalculationLogicCoreDependencyRegistrant();
            serviceRegistrant.Register(Container);
        }

        private ContainerProvider()
        {
            // Do Not change accesibility for singleton instance
        }

        private static readonly IIoCContainer Container;

        internal static IIoCContainer GetContainer()
        {
            return Container;
        }
    }
}
