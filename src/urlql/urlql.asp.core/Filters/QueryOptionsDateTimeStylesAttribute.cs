using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsDateTimeStylesAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Date Time Styles
        /// </summary>
        protected DateTimeStyles dateTimeStyles { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsDateTimeStylesAttribute(string actionArgumentName, DateTimeStyles dateTimeStyles)
        {
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.dateTimeStyles = dateTimeStyles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.DateTimeStyles = dateTimeStyles;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }
    }
}
