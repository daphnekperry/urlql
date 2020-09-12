using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urlql.mergepatch.core.ModelBinding
{
    public class MergePatchModelBinder<T> : IModelBinder
    {
        /// <summary>
        /// Model Binding Context
        /// </summary>
        protected ModelBindingContext context { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public MergePatchModelBinder()
        { }

        /// <summary>
        /// Attempt to serialize the body JSON as a JObject, catch any exceptions from bad formatting and just fail the result.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            context = bindingContext;
            context.HttpContext.Request.EnableRewind();
            try
            {
                string objectJson = string.Empty;
                using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    objectJson = reader.ReadToEnd();
                }

                bindingContext.Result = ModelBindingResult.Success(new MergePatch<T>(objectJson));
            }
            catch
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
