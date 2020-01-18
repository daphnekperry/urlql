﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryOptionsPageSizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Query Options Default Page Size
        /// </summary>
        protected int pageSize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryOptionsPageSizeAttribute(int pageSize)
        {
            this.pageSize = pageSize;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryOptions = context.ActionArguments.Where(aa => aa.Value?.GetType() == typeof(QueryOptions)).ToList();
            foreach (var o in queryOptions)
            {
                var options = o.Value as QueryOptions ?? new QueryOptions();
                options.PageSize = pageSize;
                context.ActionArguments[o.Key] = options;
            }
            return;
        }
    }
}