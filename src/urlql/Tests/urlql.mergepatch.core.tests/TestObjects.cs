using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.mergepatch.core.tests
{
    class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Bar : Foo
    {
        public double Score { get; set; }
    }
}
