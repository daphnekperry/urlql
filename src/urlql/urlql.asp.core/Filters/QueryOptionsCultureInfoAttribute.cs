using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsCultureInfoAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Query Options Culture Info
        /// </summary>
        protected CultureInfo cultureInfo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsCultureInfoAttribute(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value.GetType() == typeof(QueryOptions));
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions ?? new QueryOptions();
                options.CultureInfo = cultureInfo;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }
    }
}
