using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using urlql.Expressions;

namespace urlql
{
    /// <summary>
    /// Query Arguments
    /// </summary>
    public class QueryArguments
    {
        /// <summary>
        /// Does the object contain any statements.
        /// </summary>
        public bool HasArguments
        {
            get
            {
                return HasPaging || HasFiltering || HasOrdering || HasGrouping || HasSelections;
            }
        }

        /// <summary>
        /// Paging arguments
        /// </summary>
        public Paging Paging { get; set; }

        /// <summary>
        /// Has Paging Arguments
        /// </summary>
        public bool HasPaging { get { return Paging != null; } }

        /// <summary>
        /// Filters
        /// </summary>
        public IList<IFilteringStatement> Filtering { get; set; }

        /// <summary>
        /// Has Filtering Arguments
        /// </summary>
        public bool HasFiltering { get { return (Filtering != null && Filtering.Any()); } }

        /// <summary>
        /// Ordering Properties
        /// </summary>
        public IList<IOrderingStatement> Ordering { get; set; }

        /// <summary>
        /// Has Ordering Arguments
        /// </summary>
        public bool HasOrdering { get { return (Ordering != null && Ordering.Any()); } }

        /// <summary>
        /// Grouping Properties
        /// </summary>
        public IList<IGroupingStatement> Grouping { get; set; }

        /// <summary>
        /// Has Grouping Arguments
        /// </summary>
        public bool HasGrouping { get { return (Grouping != null && Grouping.Any()); } }

        /// <summary>
        /// Selection Properties
        /// </summary>
        public IList<ISelectionStatement> Selections { get; set; }

        /// <summary>
        /// Has Selection Arguments
        /// </summary>
        public bool HasSelections { get { return (Selections != null && Selections.Any()); } }

    }
}

