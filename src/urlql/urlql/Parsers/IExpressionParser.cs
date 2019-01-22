using System;
using System.Collections.Generic;
using System.Text;
using urlql.Expressions;

namespace urlql.Parsers
{
    public interface IExpressionParser<T> where T: IStatement
    {
        IList<T> ParseExpression(string expression);
    }
}
