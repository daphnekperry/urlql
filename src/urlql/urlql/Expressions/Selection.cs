using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Selection
    /// </summary>
    public class Selection : IQueryableStatement, ISelectionStatement, IAliasableStatement
    {
        /// <summary>
        /// Property Name for the Object
        /// </summary>
        public string PropertyName { get; protected set; }

        /// <summary>
        /// Alias
        /// </summary>
        public Alias Alias { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="givenName"></param>
        public Selection(string propertyName, Alias alias = null)
        {
            PropertyName = propertyName;
            Alias = alias;
            if (Alias != null)
            {
                if (Alias.OriginalName != PropertyName)
                {
                    throw new ArgumentException($"Invalid Alias [{Alias}] for Selection PropertyName [{PropertyName}]");
                }
            }
        }

        /// <summary>
        /// Selection as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Alias?.ToString() ?? PropertyName;
        }
    }
}
