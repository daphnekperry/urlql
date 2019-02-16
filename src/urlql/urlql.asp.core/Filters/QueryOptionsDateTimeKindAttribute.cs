using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using urlql.asp.core.Extensions;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsDateTimeKindAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Date Time Kind
        /// </summary>
        protected DateTimeKind dateTimeKind { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsDateTimeKindAttribute(string actionArgumentName, DateTimeKind dateTimeKind)
        { 
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.dateTimeKind = dateTimeKind;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.DateTimeKind = dateTimeKind;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }
    }
}
