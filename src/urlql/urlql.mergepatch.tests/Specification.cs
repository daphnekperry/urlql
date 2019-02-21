using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace urlql.mergepatch.tests
{
    [TestFixture]
    public abstract class Specification
    {
        public Specification()
        {
        }

        [OneTimeSetUp]
        public virtual void Setup()
        {
            Arrange();
            Act();
        }

        protected virtual void Arrange()
        {
            return;
        }

        protected virtual void Act()
        {
            return;
        }

        [OneTimeTearDown]
        public virtual void TearDown()
        {
        }
    }
}
