using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// IQueryableExpression that allows for Aliasing of the IQueryable Properties
    /// </summary>
    public interface IAliasableStatement : IQueryableStatement
    {
        Alias Alias { get; }
    }
}
