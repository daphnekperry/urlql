using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// ISelectionExpressionStatement extensions
    /// </summary>
    static class ISelectionStatementExtensions
    {
        /// <summary>
        /// Does the list contain any Aggregation statements
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasAggregations(this IList<ISelectionStatement> list)
        {
            if (list.OfType<Aggregation>().Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieve all Aggregation statements from the list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Aggregation> GetAggregations(this IList<ISelectionStatement> list)
        {
            return list.OfType<Aggregation>();
        }

        /// <summary>
        /// Does the list contain any Aggregation statements
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasDistinct(this IList<ISelectionStatement> list)
        {
            if (list.OfType<Distinct>().Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Does the list contain any Property Selection statements
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasSelections(this IList<ISelectionStatement> list)
        {
            if (list.OfType<Selection>().Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieve all Property Selection statements from the list 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Selection> GetSelections(this IList<ISelectionStatement> list)
        {
            return list.OfType<Selection>();
        }
    }
}
