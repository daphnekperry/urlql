using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.mergepatch.tests
{
    class MergePatchApply : Specification
    {
        protected JObject obj;
        protected MergePatch<Foo> patch;
        protected Foo testFoo;

        protected override void Arrange()
        {
            testFoo = new Foo();
            var patchFoo = new Foo() { Id = 42 };
            obj = JObject.Parse(JsonConvert.SerializeObject(patchFoo));
            patch = new MergePatch<Foo>(obj);
        }

        protected override void Act()
        {
            patch.Apply(testFoo);
        }

        [Test]
        public void ChangedValue()
        {
            Assert.AreEqual(testFoo.Id, 42);
        }
    }

    class MergePatchWrongType : Specification
    {
        protected JObject obj;
        protected MergePatch<Foo> patch;
        protected bool isValid;

        protected override void Arrange()
        {
            obj = JObject.Parse(JsonConvert.SerializeObject(new Bar()));
            patch = new MergePatch<Foo>(obj);
        }

        protected override void Act()
        {
            isValid = patch.IsValid();
        }

        [Test]
        public void IsNotValid()
        {
            Assert.IsFalse(isValid);
        }
    }

    class MergePatchMatchingType : Specification
    {
        protected JObject obj;
        protected MergePatch<Foo> patch;
        protected bool isValid;

        protected override void Arrange()
        {
            obj = JObject.Parse(JsonConvert.SerializeObject(new Foo()));
            patch = new MergePatch<Foo>(obj);
        }

        protected override void Act()
        {
            isValid = patch.IsValid();
        }

        [Test]
        public void IsValid()
        {
            Assert.IsTrue(isValid);
        }
    }

    class MergePatchWrongMemberType : Specification
    {
        protected JObject obj;
        protected MergePatch<Foo> patch;
        protected bool isValid;

        protected override void Arrange()
        {
            obj = JObject.Parse(@"{""Id"":""asdf""}");
            patch = new MergePatch<Foo>(obj);
        }

        protected override void Act()
        {
            isValid = patch.IsValid();
        }

        [Test]
        public void IsNotValid()
        {
            Assert.IsFalse(isValid);
        }
    }

    class MergePatchNoPrivateMembers : Specification
    {
        protected JObject obj;
        protected MergePatch<Foo> patch;
        protected bool isValid;

        protected override void Arrange()
        {
            obj = JObject.Parse(@"{""hidden"":1}");
            patch = new MergePatch<Foo>(obj);
        }

        protected override void Act()
        {
            isValid = patch.IsValid();
        }

        [Test]
        public void IsNotValid()
        {
            Assert.IsFalse(isValid);
        }
    }

    class MergePatchNoProtectedMembers : Specification
    {
        protected JObject obj;
        protected MergePatch<Foo> patch;
        protected bool isValid;

        protected override void Arrange()
        {
            obj = JObject.Parse(@"{""hiding"":2}");
            patch = new MergePatch<Foo>(obj);
        }

        protected override void Act()
        {
            isValid = patch.IsValid();
        }

        [Test]
        public void IsNotValid()
        {
            Assert.IsFalse(isValid);
        }
    }
}
