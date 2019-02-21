using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using urlql.mergepatch.core.ModelBinding;

namespace urlql.mergepatch.core.tests.ModelBinding
{
    class MergePatchModelBinderProviderTests : Specification
    {
        protected ModelBinderProviderContext context;
        protected MergePatchModelBinderProvider provider;
        protected IModelBinder binder;

        protected override void Arrange()
        {
            var data = new EmptyModelMetadataProvider();
            var modelMetadata = data.GetMetadataForType(typeof(MergePatch<Foo>));

            context = Substitute.For<ModelBinderProviderContext>();
            context.Metadata.Returns(modelMetadata);

            provider = new MergePatchModelBinderProvider();
        }
    }

    class GetsModelBinder : MergePatchModelBinderProviderTests
    {
        protected override void Act()
        {
            binder = provider.GetBinder(context);
        }

        [Test]
        public void CreatedModelBinder()
        {
            Assert.IsNotNull(binder);
        }
    }

    class UsesCache : MergePatchModelBinderProviderTests
    {
        protected override void Arrange()
        {
            var data = new EmptyModelMetadataProvider();
            var modelMetadata = data.GetMetadataForType(typeof(MergePatch<Bar>));

            context = Substitute.For<ModelBinderProviderContext>();
            context.Metadata.Returns(modelMetadata);

            provider = new MergePatchModelBinderProvider();
        }

        protected override void Act()
        {
            binder = provider.GetBinder(context);
        }

        [Test]
        public void CachedActivator()
        {
            var objectActivator = TypeActivator.GetActivator(typeof(Bar));
            Assert.IsNotNull(objectActivator);
        }
    }
}

