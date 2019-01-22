using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Ordering
    /// </summary>
    public class Ordering : IQueryableStatement, IOrderingStatement
    {
        /// <summary>
        /// Ordering Operation
        /// </summary>
        public OrderingOperation OrderingOperation { get; set; }

        /// <summary>
        /// Property Name for Ordering
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="operation"></param>
        public Ordering(string propertyName, OrderingOperation operation = null)
        {
            PropertyName = propertyName.Trim();
            OrderingOperation = operation ?? OrderingOperation.asc;
        }

        /// <summary>
        /// Ordering as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ($"{PropertyName} {OrderingOperation}");
        }
    }
}
