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
    public class QueryEngine
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
        protected readonly QueryValidator validator;

        /// <summary>
        /// The options and settings used for applying query statements
        /// </summary>
        protected readonly QueryComparisonFormatter formatter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="args"></param>
        /// <param name="opt"></param>
        public QueryEngine(IQueryable queryable, QueryArguments args, QueryOptions opt = null)
        {
            sourceQueryable = queryable;
            typeInfo = new QueryableObjectTypeInfo(queryable.ElementType);
            result = null;
            arguments = args;
            if (opt == null)
            {
                opt = new QueryOptions();
            }
            options = opt;
            validator = new QueryValidator(options, typeInfo);
            formatter = new QueryComparisonFormatter(options, typeInfo);
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
                try
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
                catch (QueryException ex)
                {
                    IList<string> errMsgs = new List<string>()
                    {
                        ex.Message
                    };
                    result = new QueryResult(errMsgs);
                }
            }

            return result;
        }

        /// <summary>
        /// Apply the Query Arguments and return a list of objects
        /// </summary>
        /// <returns></returns>
        public virtual IList<dynamic> GetObjects()
        {
            return this.GetResults().ToList();
        }

        /// <summary>
        /// Apply the Query Arguments and return a list of objects asynchronously
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IList<dynamic>> GetObjectsAsync()
        {
            return (await this.GetResultsAsync()).ToList();
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
        protected IQueryable ApplyFiltering(IQueryable query, QueryableObjectTypeInfo type)
        {
            if (arguments.HasFiltering)
            {
                foreach (var a in arguments.Filtering)
                {
                    if (!validator.Validate(a))
                    {
                        throw new QueryException($"filter: invalid expression {a}");
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
        protected IQueryable ApplyOrdering(IQueryable query, QueryableObjectTypeInfo type)
        {
            if (arguments.HasOrdering)
            {
                IList<IOrderingStatement> orderingStatements = new List<IOrderingStatement>();
                foreach (var order in arguments.Ordering)
                {
                    if (arguments.HasSelections)
                    {
                        if (!arguments.Selections.OfType<IQueryableStatement>().Where(s => s.PropertyName == order.PropertyName).Any())
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
        protected IQueryable ApplyGrouping(IQueryable query, QueryableObjectTypeInfo type)
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
                    if (!selections.Where(s => g.PropertyName == (s.Alias?.NewName ?? s.PropertyName)).Any() || aggregations.Where(a => g.PropertyName == a.Alias.NewName).Any())
                    {
                        throw new QueryException($"group: cannot group on {g}");
                    }
                    Grouping group = new Grouping(arguments.Selections.OfType<IAliasableStatement>().Where(s => g.PropertyName == (s.Alias?.NewName ?? s.PropertyName)).FirstOrDefault().PropertyName);
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
        protected IQueryable ApplySelections(IQueryable query, QueryableObjectTypeInfo type)
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
                    throw new QueryException($"group: must include {selections.FirstOrDefault()?.Alias.NewName} from 'select' statment");
                }

                if (!arguments.HasGrouping && aggregations.Any() && !selections.Any())
                {
                    var dynamicGroupColumnName = $"__urlql_dynamic_group__";
                    StringBuilder aggValues = new StringBuilder($"1L as {dynamicGroupColumnName}");
                    foreach (var a in aggregations)
                    {
                        if (a.AggregationOperation.Name != AggregationOperation.cnt.Name)
                        {
                            aggValues.Append($", it.{a.PropertyName}");
                        }
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
        /// <param name="fetchOneMore"></param>
        /// <returns></returns>
        protected IQueryable ApplyPaging(IQueryable query, QueryableObjectTypeInfo typeInfo, bool fetchAdditional = false)
        {
            if (arguments.HasPaging)
            {
                if (arguments.Paging.Take > options.MaximumPageSize)
                {
                    throw new QueryException($"take: cannot be greater than {options.MaximumPageSize}");
                }

                int skip = arguments.Paging.Skip;
                int take = fetchAdditional ? arguments.Paging.Take + 1 : arguments.Paging.Take;

                query = query.Skip(skip).Take(take);
            }

            return query;
        }

    }
}
