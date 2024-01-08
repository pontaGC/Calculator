namespace SimpleCalculator.Wpf.Presentation
{
    /// <summary>
    /// Numerical constant parameters for display.
    /// </summary>
    internal static class NumericalCharacters
    {
        public const string Zero = "0";
        public const string ZeroZero = "00";
        public const string One = "1";
        public const string Two = "2";
        public const string Three = "3";
        public const string Four = "4";
        public const string Five = "5";
        public const string Six = "6";
        public const string Seven = "7";
        public const string Eight = "8";
        public const string Nine = "9";

        public const string Period = ".";

        /// <summary>
        /// Enumerates all numerical characters.
        /// </summary>
        public static IEnumerable<string> EnumerateAll
        {
            get
            {
                yield return Zero;
                yield return One;
                yield return Two;
                yield return Three;
                yield return Four;
                yield return Five;
                yield return Six;
                yield return Seven;
                yield return Eight;
                yield return Nine;
                yield return Period;
            }
        }

    }
}
