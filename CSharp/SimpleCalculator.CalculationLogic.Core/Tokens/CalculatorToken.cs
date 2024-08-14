using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculationLogic.Core
{
    /// <summary>
    /// Represents a token used by calculator.
    /// </summary>
    public class CalculatorToken
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorToken"/> class.
        /// </summary>
        /// <param name="priority">The token priority.</param>
        /// <param name="tokenType">The token type.</param>
        /// <param name="value">The token value.</param>
        public CalculatorToken(
            uint priority,
            string value,
            CalculatorTokenType tokenType)
        {
            this.Priority = priority;
            this.TokenType = tokenType;
            this.Value = value ?? string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating this token is symbol.
        /// </summary>
        /// <returns><c>true</c> if the token type is <c>Symbol</c>, otherwise, <c>false</c>.</returns>
        public bool IsSymbol => this.TokenType == CalculatorTokenType.Symbol;

        /// <summary>
        /// Gets a value indicating this token is numerical value.
        /// </summary>
        /// <returns><c>true</c> if the token type is <c>Number</c>, otherwise, <c>false</c>.</returns>
        public bool IsNumber => this.TokenType == CalculatorTokenType.Number;

        /// <summary>
        /// Gets an integer indicating the calculator token priority.
        /// The higher values ​​have higher priority.
        /// </summary>
        public uint Priority { get; }

        /// <summary>
        /// Gets a token value.
        /// </summary>
        [MemberNotNull]
        public string Value { get; }

        /// <summary>
        /// Gets a token type.
        /// </summary>
        public CalculatorTokenType TokenType { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks wthether or not this token's priorty is lower than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns>
        /// <c>true</c> if this token's priority is lower than <paramref name="tokenToCompare"/>'s priority,
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool HasLowerPriority(CalculatorToken tokenToCompare)
        {
            return !this.HasSameOrHigherPriority(tokenToCompare);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns>
        /// <c>true</c> if this token's priority is higher than <paramref name="tokenToCompare"/>'s priority,
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool HasHigherPriority(CalculatorToken tokenToCompare)
        {
            if (tokenToCompare is null)
            {
                return true;
            }

            return this.HasHigherPriority(tokenToCompare.Priority);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns><c>true</c> if this token's priority is higher than <paramref name="targetPriority"/>, otherwise, <c>false</c>.</returns>
        public bool HasHigherPriority(uint targetPriority)
        {
            return this.Priority > targetPriority;
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is equal to or higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns>
        /// <c>true</c> if this token's priority is equal to or higher than <paramref name="tokenToCompare"/>'s priority,
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool HasSameOrHigherPriority(CalculatorToken tokenToCompare)
        {
            if (tokenToCompare is null)
            {
                return true;
            }

            return this.HasSameOrHigherPriority(tokenToCompare.Priority);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is equal to or higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns><c>true</c> if this token's priority is equal to or higher than <paramref name="targetPriority"/>, otherwise, <c>false</c>.</returns>
        public bool HasSameOrHigherPriority(uint targetPriority)
        {
            return  this.Priority >= targetPriority;
        }

        #endregion
    }
}
