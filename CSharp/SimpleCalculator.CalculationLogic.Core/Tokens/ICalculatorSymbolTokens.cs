﻿using System.Diagnostics.CodeAnalysis;

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
        /// Finds the bracket symbol token whose value is same as the given token value.
        /// </summary>
        /// <param name="tokenValue">The token value to look for.</param>
        /// <returns>A symbol token if finding symbol token is successful, otherwise, returns <c>null</c>.</returns>
        [return: MaybeNull]
        CalculatorToken FindBracketToken(string tokenValue);

        /// <summary>
        /// Finds the math operator token whose value is same as the operator charater.
        /// </summary>
        /// <param name="operatorCharacter">The operator string to look for.</param>
        /// <returns>A math operator if finding it is successful, otherwise, <c>null</c>.</returns>
        [return: MaybeNull]
        MathOperator FindMathOperator(string operatorCharacter);

        /// <summary>
        /// Finds the binary operator token whose value is same as the operator charater.
        /// </summary>
        /// <param name="operatorCharacter">The operator string to look for.</param>
        /// <returns>A binary operator if finding it is successful, otherwise, <c>null</c>.</returns>
        [return: MaybeNull]
        MathOperator FindBinaryOperator(string operatorCharacter);

        #endregion
    }
}
