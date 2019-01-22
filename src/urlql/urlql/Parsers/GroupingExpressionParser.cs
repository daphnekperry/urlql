using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using urlql.Expressions;
using urlql.Internal;

namespace urlql.Parsers
{
    public class GroupingExpressionParser : IExpressionParser<IGroupingStatement>
    {
        /// <summary>
        /// Create a list of Grouping objects from a query string expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<IGroupingStatement> ParseExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new QueryException("group: empty expression");
            }

            char[] splitOn = { ',' };
            var expressionSplit = expression.Split(splitOn).Select(s => s.Trim());
            Stack<string> tokens = new Stack<string>(expressionSplit.Reverse());

            IList<IGroupingStatement> groupingProperties = new List<IGroupingStatement>();
            while (tokens.Count > 0)
            {
                tokens.TryPop(out string statement);
                var statementTokens = Regex.Split(statement, @"\s").Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).ToList();

                if (statementTokens.Count != 1)
                {
                    throw new QueryException(string.Format("group: invalid grouping statement {0}", statement));
                }
                groupingProperties.Add(new Grouping(statement));
            }

            return groupingProperties;
        }
    }
}
