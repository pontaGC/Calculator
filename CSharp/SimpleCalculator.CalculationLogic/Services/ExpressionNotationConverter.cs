using System.Data;

using SimpleCalculator.CalculationLogic.Core;
using SimpleCalculator.CalculationLogic.Core.Extensions;
using SimpleCalculator.CalculationLogic.Core.Services;
using SimpleCalculator.Core.Utils;

namespace SimpleCalculator.CalculationLogic.Services
{
    /// <summary>
    /// The expression notation converter.
    /// </summary>
    internal class ExpressionNotationConverter : IExpressionNotationConverter
    {
        #region Convert infix notation to reverse Polish notation

        /// <inheritdoc />
        public Stack<CalculatorToken> ConvertInfixToRPNStack(IEnumerable<CalculatorToken> infixNotationTokens)
        {
            ArgumentGuard.RequireNotNull(infixNotationTokens, nameof(infixNotationTokens));

            var results = new List<CalculatorToken>();
            var workingStack = new Stack<CalculatorToken>();

            foreach (var token in infixNotationTokens)
            {
                if (token.IsLeftRoundBracket())
                {
                    // Left round bracket '(' will be removed
                    // when right round bracket ')' is found
                    workingStack.Push(token);
                    continue;
                }

                if (token.IsRightRoundBracket())
                {
                    if (UpdateWorkingStackWhenRightRoundBracketFound(workingStack, results) == false)
                    {
                        throw new SyntaxErrorException("The given expression has invalid bracket format.");
                    }

                    continue;
                }

                if (token is MathOperator foundOperator)
                {
                    UpdateWorkingStackWhenMathOperatorFound(foundOperator, workingStack, results);
                    continue;
                }

                if (token.IsNumber)
                {
                    results.Add(token);
                    continue;
                }

                // Found unknown token
                throw new SyntaxErrorException($"Found unknown or invalid token in the expression: {token.Value}");
            }

            // Stores remained operators
            while (workingStack.Any())
            {
                results.Add(workingStack.Pop());
            }

            if (results.HasBrackets())
            {
                // Bracket must not remain
                throw new SyntaxErrorException("The given expression has invalid bracket format.");
            }

            // Sorts the left-hand side of the expreession
            // to the top of the stack for calculation
            results.Reverse();
            return new Stack<CalculatorToken>(results);
        }

        private bool UpdateWorkingStackWhenRightRoundBracketFound(
            Stack<CalculatorToken> workingStack,
            ICollection<CalculatorToken> mainExpressionTokens)
        {
            bool success = false;

            // Moves tokens in working stack to result
            while (workingStack.TryPeek(out var topToken))
            {
                if (topToken.IsLeftRoundBracket())
                {
                    success = true;

                    // Removed left round bracket
                    workingStack.Pop();
                    break;
                }

                mainExpressionTokens.Add(workingStack.Pop());
            }

            return success;
        }

        private void UpdateWorkingStackWhenMathOperatorFound(
            MathOperator foundOperator,
            Stack<CalculatorToken> workingStack,
            ICollection<CalculatorToken> mainExpressionTokens)
        {
            if (workingStack.TryPeek(out var topToken) == false)
            {
                // Working stack is empty
                workingStack.Push(foundOperator);
                return;
            }

            if (foundOperator.HasHigherPriority(topToken))
            {
                workingStack.Push(foundOperator);
                return;
            }

            while (workingStack.TryPeek(out topToken) && topToken.HasSameOrHigherPriority(foundOperator))
            {
                if (topToken.IsLeftRoundBracket())
                {
                    // Left round bracket '(' must be popped when right round bracket ')' found
                    break;
                }

                // Moves top token in working stack to that in main
                mainExpressionTokens.Add(workingStack.Pop());
            }

            workingStack.Push(foundOperator);
        }

        #endregion
    }
}
