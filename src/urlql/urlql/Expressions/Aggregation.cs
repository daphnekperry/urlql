using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Aggregation
    /// </summary>
    public class Aggregation : IQueryableStatement, ISelectionStatement, IAliasableStatement
    {
        /// <summary>
        /// Aggregation Operation
        /// </summary>
        public AggregationOperation AggregationOperation { get; private set; }

        /// <summary>
        /// Property Name for aggregation.
        /// </summary>
        public string PropertyName { get; protected set; }

        /// <summary>
        /// Alias
        /// </summary>
        public Alias Alias { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="propertyName"></param>
        /// <param name="givenName"></param>
        public Aggregation(AggregationOperation operation, string propertyName, Alias alias)
        {
            AggregationOperation = operation;
            PropertyName = propertyName;
            Alias = alias;
            if (Alias.OriginalName != PropertyName)
            {
                throw new ArgumentException($"Invalid Alias [{Alias}] for Aggregation PropertyName [{PropertyName}]");
            }
        }

        /// <summary>
        /// Aggregation as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(AggregationOperation.Expression, PropertyName, Alias.NewName);
        }
    }
}
