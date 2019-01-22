using System;
using System.Collections.Generic;
using System.Text;

// Disable assignment warning for this test object
#pragma warning disable CS0649
namespace urlql.test
{
    /// <summary>
    /// Testing object
    /// </summary>
    public class Foo
    {
        public int Id { get; set; }

        public short CountShort;

        public long CountLong;

        public string Text;

        public double Number;

        public float SmallNumber;

        public decimal Dollars { get; set; }

        public bool Flag;

        public DateTime Timestamp;

        public Guid Key;

        public object Obj;

        public Type Type { get; set; } = typeof(Foo);

        public dynamic Dynamic;

        public List<int> List = new List<int>();

        public IEnumerable<double> Enumerable = new List<double>();

        public int[] Array = { 2, 4, 6, 8 };

        protected int hiddenInt;

        protected double hiddenDouble { get; set; }

        private Guid hiddenKey = Guid.NewGuid();

        public static IEnumerable<Foo> MakeFoo(int count = 10)
        {
            IList<Foo> newFoo = new List<Foo>();

            for (int i = 0; i < count; ++i)
            {
                Foo f = new Foo()
                {
                    Id = i,
                    Key = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddDays(-i)
                };
                newFoo.Add(f);
            }

            return newFoo;
        }
    }
}
#pragma warning restore CS0649
