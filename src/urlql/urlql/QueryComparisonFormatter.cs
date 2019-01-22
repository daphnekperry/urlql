﻿using System;
using System.Collections.Generic;
using System.Text;
using urlql.Expressions;

namespace urlql
{
    /// <summary>
    /// Formats IFilteringStatement arguments for Dynamic LINQ
    /// </summary>
    public class QueryComparisonFormatter
    {
        protected QueryOptions options {get;set;}

        protected QueryableObjectTypeInfo typeInfo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="typeInfo"></param>
        public QueryComparisonFormatter(QueryOptions options, QueryableObjectTypeInfo typeInfo)
        {
            this.options = options;
            this.typeInfo = typeInfo;
        }

        /// <summary>
        /// Formats text into the appropriate Dynamic LINQ expression for the type.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string FormatValue(IFilteringStatement statement)
        {
            if (statement is Comparison c)
            {
                var prop = typeInfo.GetPropertyTypeInfo(c.PropertyName);
                if (prop == null || (int)prop.PropertyType < (int)QueryablePropertyTypeCode.Undefined)
                {
                    return statement.ToString();
                }

                switch (c.ComparisonOperation.PropertyType)
                {
                    case QueryablePropertyTypeCode.DateTime:
                        System.DateTime.TryParseExact(c.RightOperand, options.DateTimeFormats, options.CultureInfo, options.DateTimeStyles, out var dateTime);
                        var operand = string.Format($"DateTime({dateTime.Ticks})");
                        if (dateTime.Kind != options.DateTimeKind)
                        {
                            switch (options.DateTimeKind)
                            {
                                case DateTimeKind.Utc:
                                    return string.Format($"DateTime({dateTime.Ticks}).ToUniversalTime()");
                                case DateTimeKind.Local:
                                    return string.Format($"DateTime({dateTime.Ticks}).ToLocalTime()");
                                case DateTimeKind.Unspecified:
                                default:
                                    return operand;
                            }
                        }
                        return operand;
                    case QueryablePropertyTypeCode.Guid:
                        return string.Format($"Guid.Parse(\"{c.RightOperand}\")");
                    default:
                        return c.RightOperand;
                }
            }
            return statement.ToString();
        }
    }
}