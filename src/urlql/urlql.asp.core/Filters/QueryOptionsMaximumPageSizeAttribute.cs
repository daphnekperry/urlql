using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsMaximumPageSizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Maximum Page Size
        /// </summary>
        protected int maximumPageSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsMaximumPageSizeAttribute(string actionArgumentName, int maximumPageSize)
        {
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.maximumPageSize = maximumPageSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.MaximumPageSize = maximumPageSize;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }

    }
}
