using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    public interface IFilteringStatement : IStatement
    {
        string ToString(QueryComparisonFormatter formatter);
    }
}
