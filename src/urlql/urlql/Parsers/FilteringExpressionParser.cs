using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using urlql.Expressions;
using urlql.Internal;

namespace urlql.Parsers
{
    public class FilteringExpressionParser : IExpressionParser<IFilteringStatement>
    {
        /// <summary>
        /// Create a list of FilteringExpressionStatement objects from a query string expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IList<IFilteringStatement> ParseExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new QueryException("filter: empty expression");
            }

            var keywords = new List<string>()
            {
                LogicalOperation.and.Keyword.ToUpper(),
                LogicalOperation.or.Keyword.ToUpper(),
                LogicalOperation.and.Keyword.ToLower(),
                LogicalOperation.or.Keyword.ToLower(),
                AssociationOperation.OpenParenthesis.Keyword,
                AssociationOperation.CloseParenthesis.Keyword,
            };
            var statements = Tokenize(expression, keywords);

            IList<IFilteringStatement> filterStatements = new List<IFilteringStatement>();
            foreach (var statement in statements)
            {
                var tokens = statement.Tokenize();
                if (tokens.Count() == 3)
                {
                    var property = tokens.ElementAtOrDefault(0);
                    var comparison = tokens.ElementAtOrDefault(1);
                    var argument = tokens.ElementAtOrDefault(2);

                    if (string.IsNullOrEmpty(property) || string.IsNullOrEmpty(comparison) || string.IsNullOrEmpty(argument))
                    {
                        throw new QueryException($"filter: invalid statement {statement}");
                    }

                    var operation = ComparisonOperation.GetComparisonByName(comparison);
                    if (operation == null)
                    {
                        throw new QueryException($"filter: invalid statement {statement}");
                    }

                    filterStatements.Add(new Comparison(operation, property, argument));
                }
                else if (tokens.Count() == 1 && LogicalOperation.GetLogicalByName(tokens.FirstOrDefault()) != null)
                {
                    filterStatements.Add(new LogicalConnective(LogicalOperation.GetLogicalByName(tokens.FirstOrDefault())));
                }
                else if (tokens.Count() == 1 && AssociationOperation.GetAssociationByName(tokens.FirstOrDefault()) != null)
                {
                    filterStatements.Add(new Association(AssociationOperation.GetAssociationByName(tokens.FirstOrDefault())));
                }
                else
                {
                    throw new QueryException($"filter: invalid statement {statement}");
                }
            }

            return filterStatements;
        }

        protected IEnumerable<string> Tokenize(string str, IEnumerable<string> tokens)
        {
            var outputTokens = new List<string>();
            var inputTokens = str.Tokenize();
            StringBuilder builder = new StringBuilder();
            foreach (var t in inputTokens)
            {
                if (tokens.Any(a => a.ToLower() == t.ToLower()))
                {
                    if (builder.Length > 0)
                    {
                        outputTokens.Add(builder.ToString().Trim());
                        outputTokens.Add(t);
                        builder = new StringBuilder();
                        continue;
                    }
                }
                else
                {
                    builder.Append(t).Append(" ");
                }
            }
            if (builder.Length > 0)
            {
                outputTokens.Add(builder.ToString().Trim());
            }
            return outputTokens;
        }
    }
}
