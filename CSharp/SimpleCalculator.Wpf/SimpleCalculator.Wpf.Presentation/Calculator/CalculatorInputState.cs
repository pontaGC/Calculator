namespace SimpleCalculator.Wpf.Presentation.Calculator
{
    /// <summary>
    /// The calculator input states.
    /// </summary>
    internal enum CalculatorInputState
    {
        /// <summary>
        /// The current input state is initialized (zero).
        /// </summary>
        Initialized,
        
        /// <summary>
        /// The last (previous) input is a number.
        /// </summary>
        NumberInputted,

        /// <summary>
        /// The last (previous) input is the decimal point.
        /// </summary>
        DecimalPointInpuuted,

        /// <summary>
        /// The last (previous) input is a binary operator.
        /// </summary>
        BinaryOperatorInputted,

        /// <summary>
        /// The last (previous) input is a left round bracket.
        /// </summary>
        LeftRoundBracketInputted,

        /// <summary>
        /// The last (previous) input is a right round bracket.
        /// </summary>
        RightRoundBracketInputted,

        /// <summary>
        /// The calculation has been executed.
        /// </summary>
        Calculated,
    }
}
