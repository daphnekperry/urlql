using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using urlql.mergepatch.core.ModelBinding;


namespace urlql.mergepatch.core.tests.ModelBinding
{
    class MergePatchModelBinderTests : Specification
    {
        protected ModelBindingContext context;
        protected IModelBinder binder;
        protected Stream requestBody;

        protected void SetRequestBody(string bodyContent)
        {
            requestBody = new MemoryStream(Encoding.UTF8.GetBytes(bodyContent));
            context.HttpContext.Request.Body = requestBody;
        }

        protected override void Arrange()
        {
            context = Substitute.For<DefaultModelBindingContext>();
            context.HttpContext.Request.Returns(new DefaultHttpRequest(new DefaultHttpContext()));
        }

        public override void TearDown()
        {
            base.TearDown();
            if (requestBody != null)
            {
                requestBody.Dispose();
            }
        }
    }

    class BindsFoo : MergePatchModelBinderTests
    {
        protected Foo foo = new Foo() { Id = 453 };

        protected override void Arrange()
        {
            base.Arrange();
            binder = new MergePatchModelBinder<Foo>();
            SetRequestBody(JsonConvert.SerializeObject(foo));
        }

        protected override void Act()
        {
            binder.BindModelAsync(context);
        }

        [Test]
        public void CreatedPatchFoo()
        {
            Assert.AreEqual(context.Result.IsModelSet, true);
            var result = context.Result.Model as MergePatch<Foo>;
            Assert.IsNotNull(result);
        }
    }
}
