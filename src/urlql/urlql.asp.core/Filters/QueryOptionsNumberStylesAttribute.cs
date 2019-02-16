using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using urlql.asp.core.Extensions;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsNumberStylesAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Number Styles
        /// </summary>
        protected NumberStyles numberStyles { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsNumberStylesAttribute(string actionArgumentName, NumberStyles numberStyles)
        {
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.numberStyles = numberStyles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.NumberStyles = numberStyles;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }
    }
}
