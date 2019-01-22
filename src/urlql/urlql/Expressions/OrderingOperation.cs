using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using urlql.Internal;

namespace urlql.Expressions
{
    /// <summary>
    /// Ordering Operation
    /// </summary>
    public class OrderingOperation : IOperation
    {
        /// <summary>
        /// Keyword in an expression
        /// </summary>
        public string Keyword { get; protected set; }

        /// <summary>
        /// Descriptive Name for the operation.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The operation Dynamic Linq statement template.
        /// </summary>
        public string Expression { get; protected set; }

        /// <summary>
        /// Enumerable value for the operation.
        /// </summary>
        public readonly OrderingOperationType OperationType;

        /// <summary>
        /// Ascending
        /// </summary>
        public static readonly OrderingOperation asc = new OrderingOperation("asc", OrderingOperationType.Ascending, "{0} ascending ");

        /// <summary>
        /// Descending
        /// </summary>
        public static readonly OrderingOperation desc = new OrderingOperation("desc", OrderingOperationType.Descending, "{0} descending ");

        /// <summary>
        /// Operation definitions for Expression/Statement parsing
        /// </summary>
        protected static readonly ReadOnlyDictionary<string, OrderingOperation> definitions = new ReadOnlyDictionary<string, OrderingOperation>(new Dictionary<string, OrderingOperation>()
        {
            {"asc", asc },
            {"desc", desc }
        });

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="value"></param>
        /// <param name="sortOrder"></param>
        protected OrderingOperation(string keyword, OrderingOperationType type, string expression)
        {
            Keyword = keyword;
            OperationType = type;
            Expression = expression;
            Name = Enumerations.GetEnumDescription(type);
        }

        /// <summary>
        /// Get an Ordering Operation by it's name. Used for statement/expression parsing.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static OrderingOperation GetOrderingOperationByName(string name)
        {
            var operation = definitions.Where(d => d.Key == name.ToLowerInvariant().Trim()).SingleOrDefault();
            return operation.Value;
        }

        /// <summary>
        /// Get an <see cref="OrderingOperation"/> by it's <see cref="OperationType"/> property. Used for statement/expression parsing.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static OrderingOperation GetOrderingOperationByType(OrderingOperationType type)
        {
            var operation = definitions.Where(d => d.Value.OperationType == type).SingleOrDefault();
            return operation.Value;
        }

        /// <summary>
        /// Operation Dynamic Linq statement.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Expression;
        }
    }
}
