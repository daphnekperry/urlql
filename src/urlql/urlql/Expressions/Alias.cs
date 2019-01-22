using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Alias
    /// </summary>
    public class Alias : IStatement
    {
        public string OriginalName { get; protected set; }

        /// <summary>
        /// The name of the aggregation in the result set.
        /// </summary>
        public string NewName { get; protected set; }

        public AliasOperation AliasOperation => AliasOperation.Alias;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        public Alias(string original, string alias)
        {
            OriginalName = original;
            NewName = alias;
        }

        public override string ToString()
        {
            return string.Format(AliasOperation.Expression, OriginalName, NewName);
        }
    }
}
