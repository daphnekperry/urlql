using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    public interface IGroupingStatement : IStatement
    {
        string PropertyName { get; }
    }
}
