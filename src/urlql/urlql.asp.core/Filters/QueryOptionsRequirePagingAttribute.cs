using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsRequirePagingAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Query Options Require Paging
        /// </summary>
        protected bool requirePaging { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsRequirePagingAttribute(bool requirePaging)
        {
            this.requirePaging = requirePaging;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value?.GetType() == typeof(QueryOptions)).ToList();
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions;
                options.RequirePaging = requirePaging;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }

    }
}
