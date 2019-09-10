using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using urlql.Expressions;
using urlql.Internal;

namespace urlql.Parsers
{
    public class OrderingExpressionParser : IExpressionParser<IOrderingStatement>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderingExpressionParser()
        {
        }

        /// <summary>
        /// Create a list of Ordering objects from a query string expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<IOrderingStatement> ParseExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new QueryException("order: empty expression");
            }

            char[] splitOn = { ',' };
            var expressionSplit = expression.Split(splitOn).Select(s => s.Trim());
            Stack<string> tokens = new Stack<string>(expressionSplit.Reverse());

            IList<IOrderingStatement> orderingProperties = new List<IOrderingStatement>();
            while (tokens.Count > 0)
            {
                tokens.TryPop(out string statement);
                var orderingTokens = Regex.Split(statement, @"\s").Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).ToList();

                Ordering ordering = GetStatement(orderingTokens);
                if (ordering == null)
                {
                    throw new QueryException(string.Format("order: invalid statement {0}", statement));
                }
                orderingProperties.Add(ordering);
            }

            return orderingProperties;
        }

        protected Ordering GetStatement(IList<string> statementTokens)
        {
            if (statementTokens.Count() < 1 || statementTokens.Count() > 2)
            {
                return null;
            }

            var property = statementTokens.FirstOrDefault();
            var operation = OrderingOperation.GetOrderingOperationByName(statementTokens.ElementAtOrDefault(1)) ?? OrderingOperation.asc;

            return new Ordering(property, operation);
        }
    }
}
