using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.mergepatch.core.ModelBinding
{
    public class MergePatchModelBinderProvider : IModelBinderProvider
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

            if (this.TryGetGenericType(context.Metadata.ModelType) == typeof(MergePatch<>))
            {
                var types = context.Metadata.ModelType.GetGenericArguments();
                Type o = typeof(MergePatchModelBinder<>).MakeGenericType(types);
                var activator = TypeActivator.GetActivator(o);
                var obj = activator();
                return obj as IModelBinder;
            }

            return null;
        }

        protected Type TryGetGenericType(Type type)
        {
            try
            {
                if (!type.IsGenericType)
                {
                    return null;
                }
                return type.GetGenericTypeDefinition();
            }
            catch
            { }
            return null;
        }
    }
}
