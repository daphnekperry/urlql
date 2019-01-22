using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    public class Distinct : ISelectionStatement
    {
        public DistinctOperation DistinctOperation => DistinctOperation.Distinct;

        public override string ToString()
        {
            return DistinctOperation.Distinct.ToString();
        }
    }
}
