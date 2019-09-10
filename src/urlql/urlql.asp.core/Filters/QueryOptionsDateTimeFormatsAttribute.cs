using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsDateTimeFormatsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Date Time Formats
        /// </summary>
        protected string[] dateTimeFormats { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsDateTimeFormatsAttribute(string actionArgumentName, string[] dateTimeFormats)
        {
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.dateTimeFormats = dateTimeFormats;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.DateTimeFormats = dateTimeFormats;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }
    }
}
