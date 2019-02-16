using System;
using System.Collections.Generic;
using System.Text;
using urlql.Expressions;

namespace urlql.ef.core.Expressions
{
    public class Include : IStatement
    {
        public string PropertyName { get; set; }

    }
}
