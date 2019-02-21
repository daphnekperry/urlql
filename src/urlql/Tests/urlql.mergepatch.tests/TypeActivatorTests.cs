using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.mergepatch.tests
{
    class CreatesTypeActivator : Specification
    {
        ObjectActivator activator;
        Foo foo;

        protected override void Arrange()
        {
            activator = TypeActivator.GetActivator(typeof(Foo));
        }

        protected override void Act()
        {
            foo = activator() as Foo;
        }

        [Test]
        public void CreatedFooInstance()
        {
            Assert.IsNotNull(foo);
        }
    }

    class UsesCache : Specification
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
