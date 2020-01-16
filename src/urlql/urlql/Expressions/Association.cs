using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    public class Association : IFilteringStatement
    {
        public AssociationOperation AssociationOperation { get; protected set; }

        public Association(AssociationOperation operation)
        {
            AssociationOperation = operation;
        }

        public override string ToString()
        {
            return AssociationOperation.ToString();
        }

        public string ToString(QueryStatementFormatter formatter)
        {
            return this.ToString();
        }

        public string ToExpression()
        {
            return AssociationOperation.Keyword;
        }
    }
}
