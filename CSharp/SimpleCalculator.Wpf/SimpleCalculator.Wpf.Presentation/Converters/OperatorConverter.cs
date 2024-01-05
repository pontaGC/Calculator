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
    internal class OperatorConverter : IValueConverter
    {
        private readonly IReadOnlyDictionary<string, string> operatorConversions;

        /// <summary>
        /// Intializes a new instance of the <see cref="OperatorConverter"/> class.
        /// </summary>
        public OperatorConverter()
        {
            this.operatorConversions = new Dictionary<string, string>()
            {
                { OperatorCharacters.Add, LogicSymbols.Add },
                { OperatorCharacters.Subtract, LogicSymbols.Subtract },
                { OperatorCharacters.Multiply, LogicSymbols.Multiply },
                { OperatorCharacters.Divide, LogicSymbols.Divide },
                { OperatorCharacters.LeftRoundBracket, LogicSymbols.RoundBracket.Left },
                { OperatorCharacters.RightRoundBracket, LogicSymbols.RoundBracket.Right },
            };
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logicOperatorValue = value as string;
            if (string.IsNullOrEmpty(logicOperatorValue))
            {
                return DependencyProperty.UnsetValue;
            }

            if (this.TryConvertLogicToDisplayString(logicOperatorValue, out var displayString))
            {
                return displayString;
            }

            Debug.Fail($"Found unknown operator: {logicOperatorValue}");
            return logicOperatorValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var displayOperatorString = value as string;
            if (string.IsNullOrEmpty(displayOperatorString))
            {
                return Binding.DoNothing;
            }

            if (TryConvertDisplayToLogicString(displayOperatorString, out var logicOperatorString))
            {
                return logicOperatorString;
            }

            Debug.Fail($"Found unknown operator: {displayOperatorString}");
            return displayOperatorString;
        }

        /// <summary>
        /// Try to convert dispaly string to logic string.
        /// </summary>
        /// <param name="operatorDisplaytring">The operator display string.</param>
        /// <param name="result">The logic string.</param>
        /// <returns><c>true</c> if the conversion is successful, otherwise, <c>false</c>.</returns>
        public bool TryConvertDisplayToLogicString(string operatorDisplaytring, out string result)
        {
            return this.operatorConversions.TryGetValue(operatorDisplaytring, out result);
        }

        /// <summary>
        /// Try to convert logic string to dispaly string.
        /// </summary>
        /// <param name="operatorLogicString">The operator string used by calculration logic.</param>
        /// <param name="result">The logic string.</param>
        /// <returns><c>true</c> if the conversion is successful, otherwise, <c>false</c>.</returns>
        public bool TryConvertLogicToDisplayString(string operatorLogicString, out string result)
        {
            var reverseOperatorConversions = operatorConversions.ToDictionary(x => x.Value, x => x.Key);
            return reverseOperatorConversions.TryGetValue(operatorLogicString, out result);
        }
    }
}
