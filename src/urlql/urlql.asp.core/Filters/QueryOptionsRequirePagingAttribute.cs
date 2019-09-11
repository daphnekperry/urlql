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
            var keys = context.ActionArguments.Where(aa => aa.Value.GetType() == typeof(QueryOptions)).Select(a => a.Key);
            foreach (var k in keys)
            {
                var options = context.ActionArguments[k] as QueryOptions ?? new QueryOptions();
                options.RequirePaging = requirePaging;
                context.ActionArguments[k] = options;
            }
            return;
        }
    }
}
