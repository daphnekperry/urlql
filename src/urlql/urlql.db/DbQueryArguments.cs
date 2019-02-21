using System;
using System.Collections.Generic;
using System.Text;
using urlql.db.Expressions;

namespace urlql.db
{
    /// <summary>
    /// Query Arguments used against database provided IQueryables
    /// </summary>
    public class DbQueryArguments : QueryArguments
    {
        /// <summary>
        /// Does the object contain any statements.
        /// </summary>
        public override bool HasArguments => base.HasArguments;
    }
}
