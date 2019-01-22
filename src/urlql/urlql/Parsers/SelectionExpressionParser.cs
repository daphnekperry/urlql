using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using urlql.Expressions;
using urlql.Internal;

namespace urlql.Parsers
{
    public class SelectionExpressionParser : IExpressionParser<ISelectionStatement>
    {
        protected List<string> Keywords { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SelectionExpressionParser()
        {
            Keywords = new List<string>();
            Keywords.AddRange(AggregationOperation.Keywords);
            Keywords.Add(AliasOperation.Alias.Keyword);
            Keywords.Add(DistinctOperation.Distinct.Keyword);
        }

        /// <summary>
        /// Create a list of Selection Expression Statement objects from a query string expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<ISelectionStatement> ParseExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new QueryException("select: empty expression");
            }

            char[] splitOn = { ',' };
            var statements = expression.Split(splitOn).Select(s => s.Trim());
            Stack<string> tokens = new Stack<string>(statements.Reverse());

            IList<ISelectionStatement> selections = new List<ISelectionStatement>();
            while (tokens.Count > 0)
            {
                tokens.TryPop(out string statement);
                var statementTokens = Regex.Split(statement.Trim(), @"\s").Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).ToList();

                // distinct expression test
                if (!selections.Any() && statement.StartsWith(DistinctOperation.Distinct.Keyword, StringComparison.OrdinalIgnoreCase))
                {
                    statementTokens.RemoveAt(0);
                    selections.Add(new Distinct());
                }

                var statementObject = GetStatement(statementTokens);
                if (statementObject == null)
                {
                    throw new QueryException(string.Format("select: invalid statement {0}", statement));
                }
                selections.Add(statementObject);
            }
            return selections;
        }

        protected Alias GetAlias(IEnumerable<string> statementTokens)
        {
            var reversed = statementTokens.Reverse();
            var aliasName = reversed.ElementAtOrDefault(0);
            var aliasToken = reversed.ElementAtOrDefault(1);
            var propertyName = reversed.ElementAtOrDefault(2);
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(aliasToken) || aliasToken.ToLower() != AliasOperation.Alias.Keyword.ToLower())
            {
                return null;
            }
            return new Alias(propertyName, aliasName);
        }

        protected ISelectionStatement GetStatement(IEnumerable<string> statementTokens)
        {
            if (statementTokens.Count() == 2 || statementTokens.Count() > 4)
            {
                return null;
            }

            var alias = GetAlias(statementTokens);

            var nameOrOper = statementTokens.ElementAtOrDefault(0);
            if (AggregationOperation.Keywords.Contains(nameOrOper))
            {
                if (alias == null)
                {
                    return null;
                }

                var operation = AggregationOperation.GetAggregationByName(nameOrOper);
                if (operation.OperationType == AggregationOperationType.Count && statementTokens.Count() == 3)
                {
                    return new Aggregation(operation, statementTokens.ElementAtOrDefault(0), alias);
                }
                else if (statementTokens.Count() == 4)
                {
                    return new Aggregation(operation, statementTokens.ElementAtOrDefault(1), alias);
                }
                else
                {
                    return null;
                }
            }

            return new Selection(nameOrOper, alias);
        }
    }
}
