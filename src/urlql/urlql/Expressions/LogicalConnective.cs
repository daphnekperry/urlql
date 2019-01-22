using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    public class LogicalConnective : IFilteringStatement
    {
        public LogicalOperation LogicalOperation { get; protected set; }

        public LogicalConnective(LogicalOperation operation)
        {
            LogicalOperation = operation;
        }

        public override string ToString()
        {
            return LogicalOperation.ToString();
        }

        public string ToString(QueryStatementFormatter formatter)
        {
            return this.ToString();
        }
    }
}
