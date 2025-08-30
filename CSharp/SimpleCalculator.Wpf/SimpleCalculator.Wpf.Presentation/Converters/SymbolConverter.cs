using System.Diagnostics;
using System.Globalization;

using System.Windows;
using System.Windows.Data;

using LogicSymbols = SimpleCalculator.CalculationLogic.Core.SymbolConstants;

namespace SimpleCalculator.Wpf.Presentation.Converters
{
    /// <summary>
    /// Converts the binding source value which type is <c>string</c> to the binding target value which type is <c>string</c>.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    internal class SymbolConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<string, string> DisplaySymbolConversions = new Dictionary<string, string>()
        {
            { SymbolCharacters.Add, LogicSymbols.Add },
            { SymbolCharacters.Subtract, LogicSymbols.Subtract },
            { SymbolCharacters.Multiply, LogicSymbols.Multiply },
            { SymbolCharacters.Divide, LogicSymbols.Divide },
            { SymbolCharacters.LeftRoundBracket, LogicSymbols.RoundBracket.Left },
            { SymbolCharacters.RightRoundBracket, LogicSymbols.RoundBracket.Right },
        };

        // Reverse conversions from display symbols to logic symbols
        private static readonly IReadOnlyDictionary<string, string> LogicSymbolConversions = DisplaySymbolConversions.ToDictionary(x => x.Value, x => x.Key);

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var displaySymbolValue = value as string;
            if (string.IsNullOrEmpty(displaySymbolValue))
            {
                return Binding.DoNothing;
            }

            if (TryConvertDisplayToLogicString(displaySymbolValue, out var logicOperatorString))
            {
                return logicOperatorString;
            }

            Debug.Fail($"Found unknown operator: {displaySymbolValue}");
            return displaySymbolValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logicSymbolValue = value as string;
            if (string.IsNullOrEmpty(logicSymbolValue))
            {
                return DependencyProperty.UnsetValue;
            }

            if (TryConvertLogicToDisplayString(logicSymbolValue, out var displayString))
            {
                return displayString;
            }

            Debug.Fail($"Found unknown operator: {logicSymbolValue}");
            return logicSymbolValue;
        }

        /// <summary>
        /// Try to convert dispaly string to logic string.
        /// </summary>
        /// <param name="operatorDisplaytring">The operator display string.</param>
        /// <param name="result">The logic string.</param>
        /// <returns><c>true</c> if the conversion is successful, otherwise, <c>false</c>.</returns>
        public static bool TryConvertDisplayToLogicString(string operatorDisplaytring, out string result)
        {
            return DisplaySymbolConversions.TryGetValue(operatorDisplaytring, out result);
        }

        /// <summary>
        /// Try to convert logic string to dispaly string.
        /// </summary>
        /// <param name="operatorLogicString">The operator string used by calculration logic.</param>
        /// <param name="result">The logic string.</param>
        /// <returns><c>true</c> if the conversion is successful, otherwise, <c>false</c>.</returns>
        public static bool TryConvertLogicToDisplayString(string operatorLogicString, out string result)
        {
            return LogicSymbolConversions.TryGetValue(operatorLogicString, out result);
        }
    }
}
