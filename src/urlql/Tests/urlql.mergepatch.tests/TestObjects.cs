using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.mergepatch.tests
{
    class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual string Description { get; set; }
        private int hidden { get; set; }
        protected int hiding { get; set; }
    }

    class Bar : Foo
    {
        public double Score { get; set; }
        public override string Description { get; set; }
    }
}
