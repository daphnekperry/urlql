using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace urlql.asp.core
{
    public class QueryResultViewModel
    {
        public int? StartIndex => Result?.StartIndex;
        public int? EndIndex => Result?.EndIndex;
        public bool? IsLastPage => Result?.IsLastPage;

        public QueryResult Result { get; set; }

        public QueryResultViewModel(QueryResult result)
        {
            this.Result = result;
        }
    }
}
