using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using urlql.Internal;

namespace urlql.Expressions
{
    /// <summary>
    /// Association Operation
    /// </summary>
    public class AssociationOperation : IOperation
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
        public AssociationOperationType OperationType { get; protected set; }

        /// <summary>
        /// Open Parenthesis (
        /// </summary>
        public static readonly AssociationOperation OpenParenthesis = new AssociationOperation("(", AssociationOperationType.OpenParenthesis, "(");

        /// <summary>
        /// Close Parenthesis (
        /// </summary>
        public static readonly AssociationOperation CloseParenthesis = new AssociationOperation(")", AssociationOperationType.CloseParenthesis, ")");

        /// <summary>
        /// Operation definitions for Expression/Statement parsing
        /// </summary>
        protected static readonly ReadOnlyDictionary<string, AssociationOperation> definitions = new ReadOnlyDictionary<string, AssociationOperation>(new Dictionary<string, AssociationOperation>()
        {
            {"(", OpenParenthesis },
            {")", CloseParenthesis }
        });

        protected AssociationOperation(string keyword, AssociationOperationType type, string expression)
        {
            Keyword = keyword;
            OperationType = type;
            Expression = expression;
            Name = Enumerations.GetEnumDescription(type);
        }

        /// <summary>
        /// Get a <see cref="LogicalOperation"/> by it's <see cref="Name"/>. Used for expression/statement parsing.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AssociationOperation GetAssociationByName(string name)
        {
            var operation = definitions.Where(d => d.Key == name.ToLowerInvariant().Trim()).SingleOrDefault();
            return operation.Value;
        }

        /// <summary>
        /// Get a <see cref="AssociationOperation"/> by it's <see cref="OperationType"/> property. Used for statement/expression parsing.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AssociationOperation GetAssociationByType(AssociationOperationType type)
        {
            var operation = definitions.Where(d => d.Value.OperationType == type).SingleOrDefault();
            return operation.Value;
        }
    }
}
