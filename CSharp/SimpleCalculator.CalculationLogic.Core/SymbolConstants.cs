namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// The constant parameters for symbol in the calculator expression tokens.
    /// </summary>
    public static class SymbolConstants
    {
        /// <summary>
        /// Gets the symbol of a round brackets.
        /// </summary>
        public static readonly (string Left, string Right) RoundBracket = (@"(", @")");

        /// <summary>
        /// Gets an add symbol.
        /// </summary>
        public const string Add = @"+";

        /// <summary>
        /// Gets a subtract symbol.
        /// </summary>
        public const string Subtract = @"-";

        /// <summary>
        /// Gets a multiply symbol.
        /// </summary>
        public const string Multiply = @"*";

        /// <summary>
        /// Gets a divide symbol.
        /// </summary>
        public const string Divide = @"/";
    }
}
