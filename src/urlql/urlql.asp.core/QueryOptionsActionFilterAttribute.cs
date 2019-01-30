using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using urlql.Expressions;

namespace urlql.asp.core
{
    /// <summary>
    /// Overrides Query Options for an Action or Controller when
    /// provided by a method or constructor argument.
    /// </summary>
    class QueryOptionsActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string argumentName;

        /// <summary>
        /// Ordering Property Name
        /// </summary>
        protected string orderingPropertyName;

        /// <summary>
        /// Options
        /// </summary>
        protected QueryOptions options;

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsActionFilterAttribute(string actionArgumentName,
                                            int? maximumPageSize,
                                            int? pageSize,
                                            CultureInfo cultureInfo,
                                            NumberStyles? numberStyles,
                                            string[] dateTimeFormats,
                                            DateTimeStyles? dateTimeStyles,
                                            DateTimeKind? dateTimeKind)
        {
            options = new QueryOptions();
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }

            options.MaximumPageSize = maximumPageSize.HasValue ? maximumPageSize.Value : options.MaximumPageSize;
            options.PageSize = pageSize.HasValue ? pageSize.Value : options.PageSize;
            options.CultureInfo = cultureInfo != null ? cultureInfo : options.CultureInfo;
            options.NumberStyles = numberStyles.HasValue ? numberStyles.Value : options.NumberStyles;
            options.DateTimeFormats = (dateTimeFormats != null && dateTimeFormats.Any()) ? dateTimeFormats : options.DateTimeFormats;
            options.DateTimeStyles = dateTimeStyles.HasValue ? dateTimeStyles.Value : options.DateTimeStyles;
            options.DateTimeKind = dateTimeKind.HasValue ? dateTimeKind.Value : options.DateTimeKind;
        }

        /// <summary>
        /// Add a model
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Validate that the provided argument name is correct
            var param = context.ActionDescriptor.Parameters.SingleOrDefault(x => x.Name == argumentName);
            if (param == null || param.ParameterType != typeof(QueryOptions))
            {
                throw new InvalidOperationException($"{argumentName} is not an argument or of type QueryOptions for this action");
            }

            // Validate that the paging arguments are sane.
            if (options.PageSize > options.MaximumPageSize)
            {
                throw new InvalidOperationException($"{options.PageSize} cannot be larger than {options.MaximumPageSize}");
            }

            // Apply options
            context.ActionArguments[argumentName] = options;

            return;
        }
    }
}
