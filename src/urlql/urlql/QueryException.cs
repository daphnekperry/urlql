﻿using System;
using System.Collections.Generic;
using System.Text;

namespace urlql
{
    /// <summary>
    /// Errors Encountered during the parsing, validation, or execution of urlql
    /// </summary>
    [System.Serializable]
    public class QueryException : Exception
    {
        public QueryException() { }
        public QueryException(string message) : base(message) { }
        public QueryException(string message, Exception inner) : base(message, inner) { }
        protected QueryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
