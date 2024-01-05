using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// The symbol token for the calculation logic.
    /// </summary>
    public interface ICalculatorSymbolTokens
    {
        #region Properties

        /// <summary>
        /// Gets a collection of all symbol tokens.
        /// </summary>
        IEnumerable<CalculatorToken> AllTokens { get; }

        /// <summary>
        /// Gets a collection of all brackets.
        /// </summary>
        IEnumerable<CalculatorToken> AllBrackets { get; }

        /// <summary>
        /// Gets a collection of all math operators.
        /// </summary>
        IEnumerable<MathOperator> AllOperators { get; }

        /// <summary>
        /// Gets a collection of all binary oeprators, e.g. "+", "/".
        /// </summary>
        IEnumerable<MathOperator> AllBinaryOpeartors { get;}

        #endregion

        #region Methods

        /// <summary>
        /// Finds the symbol token whose value is same as the given token value.
        /// </summary>
        /// <param name="tokenValue">The token value to look for.</param>
        /// <returns>A symbol token if finding symbol token is successful, otherwise, returns <c>null</c>.</returns>
        [return: MaybeNull]
        CalculatorToken FindSymbolToken(string tokenValue);

        /// <summary>
        /// Finds the binary operator token whose value is same as the operator string.
        /// </summary>
        /// <param name="operatorString">The operator string to look for.</param>
        /// <returns>A binary operator if finding it is successful, otherwise, <c>null</c>.</returns>
        [return: MaybeNull]
        MathOperator FindBinaryOperator(string operatorString);

        #endregion
    }
}
