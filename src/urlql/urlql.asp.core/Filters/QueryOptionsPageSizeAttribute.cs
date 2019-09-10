using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsPageSizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Default Page Size
        /// </summary>
        protected int pageSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsPageSizeAttribute(string actionArgumentName, int pageSize)
        {
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.pageSize = pageSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.PageSize = pageSize;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }
    }
}
