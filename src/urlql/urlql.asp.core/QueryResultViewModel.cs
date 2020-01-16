using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace urlql.asp.core
{
    /// <summary>
    /// Query Result View Model (DTO)
    /// </summary>
    public class QueryResultViewModel
    {
        /// <summary>
        /// Start Index
        /// </summary>
        /// <remarks>0 based</remarks>
        public int? StartIndex => Result?.StartIndex;

        /// <summary>
        /// End Index
        /// </summary>
        /// <remarks>0 based</remarks>
        public int? EndIndex => Result?.EndIndex;

        /// <summary>
        /// Is Last Page
        /// </summary>
        public bool? IsLastPage => Result?.IsLastPage;

        /// <summary>
        /// Results
        /// </summary>
        public QueryResult Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result"></param>
        public QueryResultViewModel(QueryResult result)
        {
            this.Result = result;
        }
    }
}
