using System;
using System.Collections.Generic;
using System.Text;

namespace urlql.Expressions
{
    /// <summary>
    /// Interface for any object that represents a Dynamic Linq statement
    /// </summary>
    public interface IStatement
    {
        /// <summary>
        /// Expression as a Dynamic Linq statement
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
