using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Expression that operates on an IQueryable
    /// </summary>
    public interface IQueryableStatement : IStatement
    {
        string PropertyName { get; }
    }
}
