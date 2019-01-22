using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using urlql.Internal;

namespace urlql.Expressions
{
    /// <summary>
    /// Aggregation Operation
    /// </summary>
    public class AggregationOperation : IOperation
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
        public readonly AggregationOperationType OperationType;

        /// <summary>
        /// Number of operands for the operation.
        /// </summary>
        public readonly int OperandCount;

        /// <summary>
        /// The type(s) valid for the operation.
        /// </summary>
        public readonly QueryablePropertyTypeCode PropertyType;

        /// <summary>
        /// Count
        /// </summary>
        public static readonly AggregationOperation cnt = new AggregationOperation("cnt", AggregationOperationType.Count, "it.Count() as {1}", 1, QueryablePropertyTypeCode.Any);
        /// <summary>
        /// Distinct Count
        /// </summary>
        public static readonly AggregationOperation dct = new AggregationOperation("dct", AggregationOperationType.DistinctCount, "it.Select({0}).Distinct().Count() as {1}", 2, QueryablePropertyTypeCode.Any);
        /// <summary>
        /// Minimum
        /// </summary>
        public static readonly AggregationOperation min = new AggregationOperation("min", AggregationOperationType.Minimum, "Min(it.{0}) as {1}", 2, QueryablePropertyTypeCode.Numeric);
        /// <summary>
        /// Maximum
        /// </summary>
        public static readonly AggregationOperation max = new AggregationOperation("max", AggregationOperationType.Maximum, "Max(it.{0}) as {1}", 2, QueryablePropertyTypeCode.Numeric);
        /// <summary>
        /// Average
        /// </summary>
        public static readonly AggregationOperation avg = new AggregationOperation("avg", AggregationOperationType.Average, "Average(it.{0}) as {1}", 2, QueryablePropertyTypeCode.Numeric);
        /// <summary>
        /// Summation
        /// </summary>
        public static readonly AggregationOperation sum = new AggregationOperation("sum", AggregationOperationType.Sum, "Sum(it.{0}) as {1}", 2, QueryablePropertyTypeCode.Numeric);

        /// <summary>
        /// Operation definitions for Expression/Statement parsing
        /// </summary>
        protected static readonly ReadOnlyDictionary<string, AggregationOperation> definitions = new ReadOnlyDictionary<string, AggregationOperation>(new Dictionary<string, AggregationOperation>()
        {
            {"min", min },
            {"max", max },
            {"sum", sum },
            {"avg", avg },
            {"cnt", cnt },
            {"dct", dct },
            {"count", cnt }, // Aliased keyword
            {"total", cnt }, // Aliased keyword
        });

        /// <summary>
        /// Defined operation keywords
        /// </summary>
        public static IEnumerable<string> Keywords => definitions.Select(d => d.Key);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="aggregation"></param>
        /// <param name="useProperty"></param>
        /// <param name="type"></param>
        /// <param name="operandCount"></param>
        /// <param name="operationName"></param>
        protected AggregationOperation(string keyword, AggregationOperationType type, string expression, int operandCount, QueryablePropertyTypeCode propertyType)
        {
            Keyword = keyword;
            OperationType = type;
            Expression = expression;
            Name = Enumerations.GetEnumDescription(type);

            OperandCount = operandCount;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Get an Aggregation Operation by it's <see cref="Name" /> property. Used for statement/expression parsing.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AggregationOperation GetAggregationByName(string name)
        {
            var comp = definitions.Where(d => d.Key == name.ToLowerInvariant().Trim()).SingleOrDefault();
            return comp.Value;
        }

        /// <summary>
        /// Get an <see cref="AggregationOperation"/> by it's <see cref="Value" /> property. Used for statement/expression parsing.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AggregationOperation GetAggregationByType(AggregationOperationType type)
        {
            var comp = definitions.Where(d => d.Value.OperationType == type).SingleOrDefault();
            return comp.Value;
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
