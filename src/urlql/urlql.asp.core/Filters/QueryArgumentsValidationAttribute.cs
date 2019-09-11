using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using urlql.asp.core.Internal;

namespace urlql.asp.core.Filters
{
    public class QueryArgumentsValidationAttribute : ActionFilterAttribute
    {
        public QueryArgumentsValidationAttribute()
        { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid && context.ModelState.Values.Any())
            {
                context.Result = new BadRequestObjectResult("bad request");
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
