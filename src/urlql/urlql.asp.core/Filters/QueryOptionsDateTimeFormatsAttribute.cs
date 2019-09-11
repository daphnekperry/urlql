using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsDateTimeFormatsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Query Options Date Time Formats
        /// </summary>
        protected string[] dateTimeFormats { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsDateTimeFormatsAttribute(string[] dateTimeFormats)
        {
            this.dateTimeFormats = dateTimeFormats;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value?.GetType() == typeof(QueryOptions)).ToList();
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions ?? new QueryOptions();
                options.DateTimeFormats = dateTimeFormats;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }
    }
}
