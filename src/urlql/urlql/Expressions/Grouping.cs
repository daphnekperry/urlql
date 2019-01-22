using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Grouping
    /// </summary>
    public class Grouping : IQueryableStatement, IGroupingStatement
    {
        /// <summary>
        /// Property Name for grouping
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Property Name to group on
        /// </summary>
        /// <param name="property"></param>
        public Grouping(string property)
        {
            PropertyName = property;
        }

        /// <summary>
        /// Property Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return PropertyName;
        }
    }
}
