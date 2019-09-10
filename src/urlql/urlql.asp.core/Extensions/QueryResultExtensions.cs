using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.asp.core
{
    public static class QueryResultExtensions
    {
        public static QueryResultViewModel AsViewModel(this QueryResult result)
        {
            return new QueryResultViewModel(result);
        }
    }
}
