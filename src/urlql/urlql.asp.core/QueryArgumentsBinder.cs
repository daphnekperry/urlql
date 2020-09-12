using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using urlql.Expressions;
using urlql.Parsers;
using urlql.Internal;

namespace urlql.asp.core
{
    /// <summary>
    /// QueryArguments Binder
    /// </summary>
    public class QueryArgumentsBinder : IModelBinder
    {
        /// <summary>
        /// Model Binding Context
        /// </summary>
        protected ModelBindingContext context { get; set; }

        /// <summary>
        /// Model Binding Context
        /// </summary>
        protected IQueryCollection queryStringValues { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryArgumentsBinder()
        { }

        /// <summary>
        /// Create a PagingArguments model and bind it to the context.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            context = bindingContext ?? throw new ArgumentNullException(nameof(bindingContext));

            queryStringValues = bindingContext.HttpContext.Request.Query;
            if (!queryStringValues.Any())
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            QueryArguments args = new QueryArguments();
            args.Paging = GetPagingArguments();
            args.Filtering = GetArguments<IFilteringStatement>(new FilteringExpressionParser(), "filter", "where");
            args.Ordering = GetArguments<IOrderingStatement>(new OrderingExpressionParser(), "order", "orderby");
            args.Grouping = GetArguments<IGroupingStatement>(new GroupingExpressionParser(), "group", "groupby");
            args.Selections = GetArguments<ISelectionStatement>(new SelectionExpressionParser(), "select");

            if (bindingContext.ModelState.ErrorCount > 0)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (!args.HasArguments)
            {
                args = null;
            }

            bindingContext.Result = ModelBindingResult.Success(args);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieve paging (skip, take) values from the Query String
        /// </summary>
        /// <returns></returns>
        protected Paging GetPagingArguments()
        {
            if (context == null)
            {
                throw new InvalidOperationException("bindingContext is null");
            }

            var parameters = context.HttpContext.Request.Query;

            try
            {
                // Both Skip and Take must be provided
                bool hasSkip = parameters.Where(x => x.Key?.CompareCaseInsensitive(@"skip") ?? false).Any();
                bool hasTake = parameters.Where(x => x.Key?.CompareCaseInsensitive(@"take") ?? false).Any();
                if (!hasTake && hasSkip)
                {
                    throw new QueryException("take: missing value");
                }
                if (!hasSkip && hasTake)
                {
                    throw new QueryException("skip: missing value");
                }

                // must be integers
                var skipExpression = parameters.Where(x => x.Key?.CompareCaseInsensitive(@"skip") ?? false).FirstOrDefault().Value;
                bool isValidSkip = int.TryParse(skipExpression, out int skip);
                if (hasSkip && !isValidSkip)
                {
                    throw new QueryException($"skip: invalid statement {skipExpression}");
                }
                var takeExpression = parameters.Where(x => x.Key?.CompareCaseInsensitive(@"take") ?? false).FirstOrDefault().Value;
                bool isValidTake = int.TryParse(takeExpression, out int take);
                if (hasTake && !isValidTake)
                {
                    throw new QueryException($"take: invalid statement {takeExpression}");
                }

                if (hasSkip && isValidSkip && hasTake && isValidTake)
                {
                    Paging paging;
                    try
                    {
                        paging = new Paging(skip, take);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        throw new QueryException($"{ex.ParamName}: invalid value of {ex.ActualValue}");
                    }
                    return paging;
                }
            }
            catch (QueryException ex)
            {
                context.ModelState.TryAddModelError(context.ModelType.Name, ex.Message);
                return null;
            }

            return null;
        }

        /// <summary>
        /// Parse a specific set of arguments from the Request Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parser"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        protected IList<T> GetArguments<T>(IExpressionParser<T> parser, params string[] keywords) where T : IStatement
        {
            var keywordList = keywords.Select(k => k.ToLowerInvariant()).ToList();
            var paramKeys = queryStringValues.Keys.Select(k => k.ToLowerInvariant()).Intersect(keywordList).ToList();
            if (!paramKeys.Any())
            {
                return null;
            }
            if (paramKeys.Count > 1)
            {
                context.ModelState.TryAddModelError(context.ModelType.Name, $"ambiguous arguments: [{string.Join(", ", paramKeys)}]");
                return null;
            }

            var key = paramKeys.FirstOrDefault();
            string expression = queryStringValues.Where(x => x.Key?.CompareCaseInsensitive(key) ?? false).FirstOrDefault().Value;
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            try
            {
                return parser.ParseExpression(expression);
            }
            catch (QueryException ex)
            {
                context.ModelState.TryAddModelError(context.ModelType.Name, ex.Message);
                return null;
            }
        }
    }
}
