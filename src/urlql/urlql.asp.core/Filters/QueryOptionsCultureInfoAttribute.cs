using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using urlql.asp.core.Extensions;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsCultureInfoAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// QueryArgument action argument name
        /// </summary>
        protected string actionArgumentName { get; set; }

        /// <summary>
        /// Query Options Culture Info
        /// </summary>
        protected CultureInfo cultureInfo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsCultureInfoAttribute(string actionArgumentName, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(actionArgumentName))
            {
                throw new ArgumentNullException(nameof(actionArgumentName));
            }
            this.actionArgumentName = actionArgumentName;
            this.cultureInfo = cultureInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ValidateParameter(actionArgumentName, typeof(QueryOptions));
            var options = (context.ActionArguments[actionArgumentName] as QueryOptions) ?? new QueryOptions();
            options.CultureInfo = cultureInfo;
            context.ActionArguments[actionArgumentName] = options;
            return;
        }
    }
}
