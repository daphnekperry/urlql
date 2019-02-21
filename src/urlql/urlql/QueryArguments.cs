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
        public virtual bool HasArguments
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
        public bool HasFiltering { get { return (Filtering?.Any() ?? false); } }

        /// <summary>
        /// Ordering Properties
        /// </summary>
        public IList<IOrderingStatement> Ordering { get; set; }

        /// <summary>
        /// Has Ordering Arguments
        /// </summary>
        public bool HasOrdering { get { return (Ordering?.Any() ?? false); } }

        /// <summary>
        /// Grouping Properties
        /// </summary>
        public IList<IGroupingStatement> Grouping { get; set; }

        /// <summary>
        /// Has Grouping Arguments
        /// </summary>
        public bool HasGrouping { get { return (Grouping?.Any() ?? false); } }

        /// <summary>
        /// Selection Properties
        /// </summary>
        public IList<ISelectionStatement> Selections { get; set; }

        /// <summary>
        /// Has Selection Arguments
        /// </summary>
        public bool HasSelections { get { return (Selections?.Any() ?? false); } }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            if (HasSelections)
            {
                builder.Append(@"select ");
                builder.Append(string.Join(", ", Selections.Select(s => s.ToString())));
                builder.Append(@" ");
            }
            if (HasFiltering)
            {
                builder.Append(@"where ");
                builder.Append(string.Join(", ", Filtering.Select(s => s.ToString())));
                builder.Append(@" ");
            }
            if (HasGrouping)
            {
                builder.Append(@"group by ");
                builder.Append(string.Join(", ", Grouping.Select(s => s.ToString())));
                builder.Append(@" ");
            }
            if (HasOrdering)
            {
                builder.Append(@"order by ");
                builder.Append(string.Join(", ", Ordering.Select(s => s.ToString())));
                builder.Append(@" ");
            }
            if (HasPaging)
            {
                builder.Append($"skip {Paging.Skip} take {Paging.Take}");
            }
            return builder.ToString().TrimEnd();
        }
    }
}

