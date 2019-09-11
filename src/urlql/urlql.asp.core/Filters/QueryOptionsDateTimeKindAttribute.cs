using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsDateTimeKindAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Query Options Date Time Kind
        /// </summary>
        protected DateTimeKind dateTimeKind { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsDateTimeKindAttribute(DateTimeKind dateTimeKind)
        { 
            this.dateTimeKind = dateTimeKind;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value?.GetType() == typeof(QueryOptions)).ToList();
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions ?? new QueryOptions();
                options.DateTimeKind = dateTimeKind;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }
    }
}
