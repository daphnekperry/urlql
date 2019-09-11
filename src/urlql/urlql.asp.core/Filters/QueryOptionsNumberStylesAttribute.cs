using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsNumberStylesAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Query Options Number Styles
        /// </summary>
        protected NumberStyles numberStyles { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsNumberStylesAttribute(NumberStyles numberStyles)
        {
            this.numberStyles = numberStyles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value?.GetType() == typeof(QueryOptions)).ToList();
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions ?? new QueryOptions();
                options.NumberStyles = numberStyles;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }
    }
}
