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
        /// Query Options Maximum Page Size
        /// </summary>
        protected int maximumPageSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsMaximumPageSizeAttribute(int maximumPageSize)
        {
            this.maximumPageSize = maximumPageSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value.GetType() == typeof(QueryOptions));
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions ?? new QueryOptions();
                options.MaximumPageSize = maximumPageSize;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }

    }
}
