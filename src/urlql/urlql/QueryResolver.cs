using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using urlql.Expressions;
using urlql.Parsers;

namespace urlql
{
    /// <summary>
    /// Applies a Query Against an IQueryable
    /// </summary>
    public class QueryResolver
    {
        /// <summary>
        /// The result of the applied query statements
        /// </summary>
        protected QueryResult result;

        /// <summary>
        /// The IQueryable to execute against
        /// </summary>
        protected readonly IQueryable sourceQueryable;

        /// <summary>
        /// The query arguments to apply
        /// </summary>
        protected readonly QueryArguments arguments;

        /// <summary>
        /// The type info for the object being queried
        /// </summary>
        protected readonly QueryableObjectTypeInfo typeInfo;

        /// <summary>
        /// The options and settings used for applying query statements
        /// </summary>
        protected readonly QueryOptions options;

        /// <summary>
        /// Query Validator
        /// </summary>
        protected readonly QueryArgumentsValidator validator;

        /// <summary>
        /// The options and settings used for applying query statements
        /// </summary>
        protected readonly QueryStatementFormatter formatter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="args"></param>
        /// <param name="opt"></param>
        public QueryResolver(IQueryable queryable, QueryArguments args, QueryOptions opt = null)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }
            sourceQueryable = queryable;

            result = null;
            arguments = args ?? new QueryArguments();
            options = opt ?? new QueryOptions();
            typeInfo = new QueryableObjectTypeInfo(queryable.ElementType);
            validator = new QueryArgumentsValidator(options, typeInfo);
            formatter = new QueryStatementFormatter(options, typeInfo);
            this.ApplyOptionsToArguments();
        }

        /// <summary>
        /// Apply the Query Arguments and return the results
        /// </summary>
        /// <returns></returns>
        public virtual QueryResult GetResults()
        {
            return this.GetResultsAsync().Result;
        }

        /// <summary>
        /// Apply the Query Arguments and return the results asynchronously
        /// </summary>
        /// <returns></returns>
        public virtual async Task<QueryResult> GetResultsAsync()
        {
            if (result == null)
            {
                var query = ApplyArguments();
                IList<dynamic> resultObjects = await query.ToDynamicListAsync();

                if (arguments.HasPaging)
                {
                    bool hasMorePages = (resultObjects.Count > arguments.Paging.Take);
                    if (hasMorePages)
                    {
                        resultObjects.RemoveAt(resultObjects.Count - 1);
                    }
                    result = new QueryResult(resultObjects, arguments.Paging, !(hasMorePages));
                }
                else
                {
                    result = new QueryResult(resultObjects);
                }
            }

            return result;
        }


        /// <summary>
        /// Apply the Query Arguments in the correct order
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable ApplyArguments()
        {
            var query = sourceQueryable;

            if (arguments.HasFiltering)
            {
                query = ApplyFiltering(query, typeInfo);
            }

            if (arguments.HasGrouping)
            {
                query = ApplyGrouping(query, typeInfo);
            }

            if (arguments.HasSelections)
            {
                query = ApplySelections(query, typeInfo);
            }

            if (arguments.HasOrdering)
            {
                query = ApplyOrdering(query, typeInfo);
            }

            if (arguments.HasPaging)
            {
                query = ApplyPaging(query, typeInfo, true);
            }
            return query;
        }

        /// <summary>
        /// Apply Fitering Statements
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual IQueryable ApplyFiltering(IQueryable query, QueryableObjectTypeInfo type)
        {
            if (arguments.HasFiltering)
            {
                foreach (var a in arguments.Filtering)
                {
                    if (!validator.Validate(a))
                    {
                        throw new QueryException($"filter: invalid expression {a.ToExpression()}");
                    }
                }

                var filteringExpression = string.Join(" ", arguments.Filtering.Select(a => a.ToString(formatter)));
                query = query.Where(filteringExpression);
            }

            return query;
        }

        /// <summary>
        /// Apply Ordering Statements
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual IQueryable ApplyOrdering(IQueryable query, QueryableObjectTypeInfo type)
        {
            if (arguments.HasOrdering)
            {
                IList<IOrderingStatement> orderingStatements = new List<IOrderingStatement>();
                foreach (var order in arguments.Ordering)
                {
                    if (arguments.HasSelections)
                    {
                        var selections = arguments.Selections.GetSelections();
                        var aggregations = arguments.Selections.GetAggregations();

                        if (!selections.Where(s => string.Compare(order.PropertyName, s?.Alias?.NewName ?? s.PropertyName, StringComparison.OrdinalIgnoreCase) == 0).Any() && !aggregations.Where(a => string.Compare(order.PropertyName, a?.Alias?.NewName, StringComparison.OrdinalIgnoreCase) == 0).Any())
                        {
                            throw new QueryException($"order: cannot order by property {order.PropertyName}");
                        }
                    }
                    else
                    {
                        if (type.GetPropertyTypeInfo(order.PropertyName) == null)
                        {
                            throw new QueryException($"order: cannot order by property {order.PropertyName}");
                        }
                    }
                    orderingStatements.Add(order);
                }

                foreach (var order in arguments.Ordering.Reverse())
                {
                    query = query.OrderBy(order.ToString());
                }
            }

            return query;
        }

        /// <summary>
        /// Apply Grouping Statements
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual IQueryable ApplyGrouping(IQueryable query, QueryableObjectTypeInfo type)
        {
            if (arguments.HasGrouping)
            {
                if (!arguments.HasSelections)
                {
                    throw new QueryException("group: missing 'select' statement");
                }
                var selections = arguments.Selections.GetSelections();
                var aggregations = arguments.Selections.GetAggregations();

                IList<IGroupingStatement> groupStatements = new List<IGroupingStatement>();
                foreach (var g in arguments.Grouping.Distinct())
                {
                    if (!selections.Where(s => string.Compare(g.PropertyName, s?.Alias?.NewName ?? s.PropertyName, StringComparison.OrdinalIgnoreCase) == 0).Any() || aggregations.Where(a => string.Compare(g.PropertyName, a?.Alias?.NewName, StringComparison.OrdinalIgnoreCase) == 0).Any())
                    {
                        throw new QueryException($"group: cannot group on {g}");
                    }
                    Grouping group = new Grouping(arguments.Selections.OfType<IAliasableStatement>().Where(s => string.Compare(g.PropertyName, s.Alias?.NewName ?? s.PropertyName, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault().PropertyName);
                    if (!validator.Validate(group))
                    {
                        throw new QueryException($"group: cannot group on {g}");
                    }
                    groupStatements.Add(group);
                }

                var groupingStatement = $"new ({string.Join(", ", groupStatements)})";
                query = query.GroupBy(groupingStatement);
            }

            return query;
        }

        /// <summary>
        /// Apply Selection Statements
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual IQueryable ApplySelections(IQueryable query, QueryableObjectTypeInfo type)
        {
            if (arguments.HasSelections)
            {
                foreach (var s in arguments.Selections.OfType<IAliasableStatement>())
                {
                    if (!validator.Validate(s))
                    {
                        throw new QueryException($"select: invalid statement with property {s.PropertyName}");
                    }
                }

                var selections = arguments.Selections.GetSelections();
                var aggregations = arguments.Selections.GetAggregations();

                if (!arguments.HasGrouping && aggregations.Any() && selections.Any())
                {
                    throw new QueryException("select: missing 'group' statement");
                }

                if (!arguments.HasGrouping && aggregations.Any() && !selections.Any())
                {
                    var dynamicGroupColumnName = @"__urlql_dynamic_group__";
                    StringBuilder aggValues = new StringBuilder($"1L as {dynamicGroupColumnName}");
                    foreach (var name in aggregations.Where(a => a.AggregationOperation.OperandCount != 1).Select(a => a.PropertyName).Distinct())
                    {
                        aggValues.Append($", it.{name}");
                    }
                    var groupObjectForScalarCalc = $"new ( {aggValues.ToString()} )";
                    query = query.Select(groupObjectForScalarCalc);
                    query = query.GroupBy(dynamicGroupColumnName);
                }
                var selectAgg = string.Join(", ", aggregations.Select(s => s.ToString()));

                string selectProps = string.Empty;
                if (arguments.HasGrouping)
                {
                    selectProps = string.Join(", ", selections.Select(s => $"it.Key.{s}"));
                }
                else
                {
                    selectProps = string.Join(", ", selections.Select(s => $"it.{s}"));
                }

                string selectionStatement = string.Empty;
                if (!string.IsNullOrEmpty(selectProps) && !string.IsNullOrEmpty(selectAgg))
                {
                    selectionStatement = $"new ({selectProps}, {selectAgg})";
                }
                else
                {
                    selectionStatement = $"new ({selectProps} {selectAgg})";
                }

                query = query.Select(selectionStatement);

                if (arguments.Selections.HasDistinct())
                {
                    query = query.Distinct();
                }
            }

            return query;
        }

        /// <summary>
        /// Apply Paging Arguements
        /// </summary>
        /// <param name="query"></param>
        /// <param name="typeInfo"></param>
        /// <param name="fetchAdditional"></param>
        /// <returns></returns>
        protected virtual IQueryable ApplyPaging(IQueryable query, QueryableObjectTypeInfo typeInfo, bool fetchAdditional = false)
        {
            var paging = arguments.Paging;
            if (paging != null)
            {
                if (!validator.Validate(paging))
                {
                    throw new QueryException($"paging: invalid page size of {arguments.Paging.Take}");
                }
                int skip = paging.Skip;
                int take = fetchAdditional ? paging.Take + 1 : paging.Take;
                query = query.Skip(skip).Take(take);
            }

            return query;
        }

        /// <summary>
        /// Apply Options to they 
        /// </summary>
        protected virtual void ApplyOptionsToArguments()
        {
            if (options.RequirePaging && arguments.Paging == null)
            {
                arguments.Paging = new Paging(0, options.PageSize);
            }
        }
    }
}
