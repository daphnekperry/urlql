using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlql.asp.core.Internal
{
    internal static class ActionExecutingContextExtensions
    {
        public static void ValidateParameter(this ActionExecutingContext context, string parameterName, Type parameterType)
        {
            // Validate that the provided argument name is correct
            var param = context.ActionDescriptor.Parameters.SingleOrDefault(x => x.Name == parameterName);
            if (param == null || param.ParameterType != parameterType)
            {
                throw new InvalidOperationException($"{parameterName} is not of type {parameterType.Name} or is not an argument for this action.");
            }
        }
    }
}
