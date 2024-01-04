namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// The definition of calculator token priority.
    /// The higher values ​represents higher priority.
    /// </summary>
    public static class CalculatorTokenPriority
    {
        public const uint Top = uint.MaxValue;
        public const uint Any = uint.MinValue;

        public const uint RoundBracket = 200;

        public const uint Multiply = 100;
        public const uint Divide = Multiply;

        public const uint Add = 50;
        public const uint Subtract = Add;
    }
}
