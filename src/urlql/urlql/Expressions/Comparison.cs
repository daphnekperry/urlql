using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Comparison
    /// </summary>
    public class Comparison : IComparison, IFilteringStatement
    {
        /// <summary>
        /// Comparison Operation
        /// </summary>
        public ComparisonOperation ComparisonOperation { get; protected set; }

        /// <summary>
        /// Property Name
        /// </summary>
        public string PropertyName => LeftOperand;

        /// <summary>
        /// Left Operand (Property Name)
        /// </summary>
        public string LeftOperand { get; protected set; }

        /// <summary>
        /// Right Operand (Value for comparison)
        /// </summary>
        public string RightOperand { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        public Comparison(ComparisonOperation operation, string leftOperand, string rightOperand)
        {
            ComparisonOperation = operation;
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        /// <summary>
        /// Comparison as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var argument = ComparisonOperation.IsCaseSensitive ? RightOperand : RightOperand.ToLower();
            return string.Format(ComparisonOperation.Expression, LeftOperand, argument);
        }

        /// <summary>
        /// Comparison as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        public virtual string ToString(QueryStatementFormatter formatter)
        {
            var argument = ComparisonOperation.IsCaseSensitive ? RightOperand : RightOperand.ToLower();
            argument = formatter.FormatValue(this);
            return string.Format(ComparisonOperation.Expression, LeftOperand, argument);
        }

        public string ToExpression()
        {
            return $"{this.LeftOperand} {this.ComparisonOperation.Keyword} {this.RightOperand}";
        }
    }
}
