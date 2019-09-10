using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.Expressions;

namespace urlql
{
    /// <summary>
    /// Query Results
    /// </summary>
    public class QueryResult : IReadOnlyList<object>
    {
        private IList<dynamic> resultList;

        /// <summary>
        /// Starting row index (0 based)
        /// </summary>
        public int? StartIndex { get; protected set; }

        /// <summary>
        /// Ending row index (0 based)
        /// </summary>
        public int? EndIndex { get; protected set; }

        /// <summary>
        /// Is this the last page of results
        /// </summary>
        public bool? IsLastPage { get; protected set; }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="results"></param>
        public QueryResult(IList<dynamic> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }
            resultList = results;
            StartIndex = null;
            EndIndex = null;
            IsLastPage = null;
        }

        /// <summary>
        /// Page Results Constructor
        /// </summary>
        /// <param name="results"></param>
        /// <param name="paging"></param>
        /// <param name="lastPage"></param>
        public QueryResult(IList<dynamic> results, Paging paging, bool lastPage)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            if (paging == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            resultList = results;
            IsLastPage = lastPage;

            StartIndex = paging.Skip;
            var endVal = paging.Skip + (results.Count - 1);
            EndIndex = paging.Skip > endVal ? paging.Skip : endVal;
        }

        /// <summary>
        /// Transform Constructor (for object modeling)
        /// </summary>
        /// <param name="original"></param>
        /// <param name="newData"></param>
        public QueryResult(QueryResult original, IList<dynamic> newData)
        {
            if (newData == null)
            {
                throw new ArgumentNullException(nameof(newData));
            }

            if (original.Count() != newData.Count())
            {
                throw new ArgumentNullException($"element count mismatch original [{original.Count()}] newData [{newData.Count()}]");
            }

            resultList = newData;
            StartIndex = original.StartIndex;
            EndIndex = original.EndIndex;
            IsLastPage = original.IsLastPage;
        }

        /// <summary>
        /// Address element by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object IReadOnlyList<object>.this[int index] => resultList[index];

        /// <summary>
        /// Count of results
        /// </summary>
        int IReadOnlyCollection<object>.Count => resultList.Count;

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.resultList.GetEnumerator();
        }

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return this.resultList.GetEnumerator();
        }
    }
}
