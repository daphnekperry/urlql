using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.asp.core
{
    /// <summary>
    /// ModelBinder provider for QueryArgumentsModel
    /// </summary>
    public class QueryArgumentsBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// IModelBinderProvider implementation
        /// </summary>
        /// <param name="context">ModelBinderProvider context</param>
        /// <returns></returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(QueryArguments))
            {
                return new BinderTypeModelBinder(typeof(QueryArgumentsBinder));
            }

            return null;
        }
    }
}
