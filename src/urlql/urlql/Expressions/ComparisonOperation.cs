using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using urlql.Internal;

namespace urlql.Expressions
{
    /// <summary>
    /// Comparison Operation
    /// </summary>
    public class ComparisonOperation : IOperation
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
        public readonly ComparisonOperationType OperationType;

        /// <summary>
        /// Is the operation or arguments are case sensitive;
        /// </summary>
        public readonly bool IsCaseSensitive;

        /// <summary>
        /// The type(s) valid for the operation.
        /// </summary>
        public readonly QueryablePropertyTypeCode PropertyType;

        /// <summary>
        /// Equals (==)
        /// </summary>
        [Description("Equals")]
        public static readonly ComparisonOperation eq = new ComparisonOperation("eq", ComparisonOperationType.Equal, "it.{0} == {1}", true, QueryablePropertyTypeCode.Any);

        /// <summary>
        /// Not Equals (!=)
        /// </summary>
        [Description("Not Equals")]
        public static readonly ComparisonOperation ne = new ComparisonOperation("ne", ComparisonOperationType.NotEqual, "it.{0} != {1}", true, QueryablePropertyTypeCode.Any);
        
        /// <summary>
        /// Less Than (&lt;)
        /// </summary>
        public static readonly ComparisonOperation lt = new ComparisonOperation("lt", ComparisonOperationType.LessThan, "it.{0} < {1}", true, QueryablePropertyTypeCode.Any);
        
        /// <summary>
        /// Greater Than (&gt;)
        /// </summary>
        public static readonly ComparisonOperation gt = new ComparisonOperation("gt", ComparisonOperationType.GreaterThan, "it.{0} > {1}", true, QueryablePropertyTypeCode.Any);
        
        /// <summary>
        /// Less Than or Equal To (&lt;=)
        /// </summary>
        public static readonly ComparisonOperation le = new ComparisonOperation("le", ComparisonOperationType.LessThanOrEqual, "it.{0} <= {1}", true, QueryablePropertyTypeCode.Any);
        
        /// <summary>
        /// Greater Than or Equal To (&gt;=)
        /// </summary>
        public static readonly ComparisonOperation ge = new ComparisonOperation("ge", ComparisonOperationType.GreaterThanOrEqual, "it.{0} >= {1}", true, QueryablePropertyTypeCode.Any);
        
        /// <summary>
        /// Contains Text
        /// </summary>
        public static readonly ComparisonOperation cn = new ComparisonOperation("cn", ComparisonOperationType.ContainsText, "it.{0}.Contains({1})", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Does Not Contain Text
        /// </summary>
        public static readonly ComparisonOperation nc = new ComparisonOperation("nc", ComparisonOperationType.DoesNotContainText, "!(it.{0}.Contains({1}))", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Starts With Text
        /// </summary>
        public static readonly ComparisonOperation st = new ComparisonOperation("st", ComparisonOperationType.StartsWithText, "it.{0}.StartsWith({1})", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Ends With Text
        /// </summary>
        public static readonly ComparisonOperation ed = new ComparisonOperation("ed", ComparisonOperationType.EndsWithText, "it.{0}.EndsWith({1})", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Equals Case Insensitive Text
        /// </summary>
        public static readonly ComparisonOperation ieq = new ComparisonOperation("ieq", ComparisonOperationType.EqualCI, "it.{0}.ToLower() == {1}", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Not Equals Case Insensitive Text
        /// </summary>
        public static readonly ComparisonOperation ine = new ComparisonOperation("ine", ComparisonOperationType.NotEqualCI, "it.{0}.ToLower() != {1}", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Contains Case Insensitive Text
        /// </summary>
        public static readonly ComparisonOperation icn = new ComparisonOperation("icn", ComparisonOperationType.ContainsTextCI, "it.{0}.ToLower().Contains({1})", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Does Not Contain Case Insensitive Text
        /// </summary>
        public static readonly ComparisonOperation inc = new ComparisonOperation("inc", ComparisonOperationType.DoesNotContainTextCI, "!(it.{0}.ToLower().Contains({1}))", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Starts With Case Insensitive Text
        /// </summary>
        public static readonly ComparisonOperation ist = new ComparisonOperation("ist", ComparisonOperationType.StartsWithTextCI, "it.{0}.ToLower().StartsWith({1})", true, QueryablePropertyTypeCode.Text);
        
        /// <summary>
        /// Ends With Case Insensitive Text
        /// </summary>
        public static readonly ComparisonOperation ied = new ComparisonOperation("ied", ComparisonOperationType.EndsWithTextCI, "it.{0}.ToLower().EndsWith({1})", true, QueryablePropertyTypeCode.Text);

        /// <summary>
        /// Operation definitions for Expression/Statement parsing
        /// </summary>
        protected static readonly ReadOnlyDictionary<string, ComparisonOperation> definitions = new ReadOnlyDictionary<string, ComparisonOperation>(new Dictionary<string, ComparisonOperation>()
        {
            {"eq", eq },
            {"ne", ne },
            {"lt", lt },
            {"gt", gt },
            {"le", le },
            {"ge", ge },
            {"cn", cn },
            {"nc", nc },
            {"st", st },
            {"ed", ed },
            {"ieq", ieq },
            {"ine", ine },
            {"icn", icn },
            {"inc", inc },
            {"ist", ist },
            {"ied", ied }
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
        /// <param name="isCaseSensitive"></param>
        /// <param name="comparison"></param>
        /// <param name="type"></param>
        /// <param name="operandCount"></param>
        /// <param name="operationName"></param>
        protected ComparisonOperation(string keyword, ComparisonOperationType type, string expression, bool isCaseSensitive, QueryablePropertyTypeCode propertyType)
        {
            Keyword = Keyword;
            OperationType = type;
            Expression = expression;
            Name = Enumerations.GetEnumDescription(type);

            IsCaseSensitive = isCaseSensitive;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Get a <see cref="ComparisonOperation"/> by it's <see cref="Name"/>. Used for expression/statement parsing.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ComparisonOperation GetComparisonByName(string name)
        {
            var operation = definitions.Where(d => d.Key == name.ToLowerInvariant().Trim()).SingleOrDefault();
            return operation.Value;
        }

        /// <summary>
        /// Get a <see cref="ComparisonOperation"/> by it's <see cref="Value"/> property. Used for statement/expression parsing.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ComparisonOperation GetComparisonByType(ComparisonOperationType type)
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
