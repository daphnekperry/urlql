using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.mergepatch.tests
{
    class TypeActivatorTests : Specification
    {
        ObjectActivator activator;

        protected override void Arrange()
        {
            TypeActivator.GetActivator(typeof(Foo));
        }

        protected override void Act()
        {
            TypeActivator.ActivatorCache.TryGetValue(typeof(Foo).GUID, out this.activator);
        }

        [Test]
        public void CachedFoo()
        {
            Assert.IsNotNull(activator);
        }
    }
}
