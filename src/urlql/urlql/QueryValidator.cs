using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using urlql.Expressions;
using urlql.Internal;

namespace urlql
{
    /// <summary>
    /// Validates Query Arguments against QueryableTypeInfo
    /// </summary>
    public class QueryValidator
    {
        /// <summary>
        /// Query Options
        /// </summary>
        protected QueryOptions options { get; set; }

        /// <summary>
        /// Query Options
        /// </summary>
        protected QueryableObjectTypeInfo typeInfo { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="typeInfo"></apram>
        public QueryValidator(QueryOptions options, QueryableObjectTypeInfo typeInfo)
        {
            this.options = options;
            this.typeInfo = typeInfo;
        }

        /// <summary>
        /// Validate an ISelectionStatement against the QueryValidator's QueryableObjectTypeInfo
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Validate(ISelectionStatement statement)
        {
            if (statement is IAliasableStatement a)
            {
                return Validate(a);
            }
            return true;
        }

        /// <summary>
        /// Validate an IAliasableStatement against the QueryValidator's QueryableObjectTypeInfo
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Validate(IAliasableStatement statement)
        {
            if (statement is Selection s)
            {
                var prop = typeInfo.GetPropertyTypeInfo(s.PropertyName);
                if (prop == null)
                {
                    return false;
                }
                return true;
            }

            if (statement is Aggregation a)
            {
                var prop = typeInfo.GetPropertyTypeInfo(a.PropertyName);
                if (prop == null)
                {
                    return false;
                }
                return this.Validate(a);
            }
            return false;
        }

        public bool Validate(IGroupingStatement statement)
        {
            var prop = typeInfo.GetPropertyTypeInfo(statement.PropertyName);
            if (prop == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate an IFilteringStatement against the QueryValidator's QueryableObjectTypeInfo
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Validate(IFilteringStatement statement)
        {
            if (statement is Comparison c)
            {
                var prop = typeInfo.GetPropertyTypeInfo(c.PropertyName);
                if (prop == null || (int)prop.PropertyType < (int)QueryablePropertyTypeCode.Undefined)
                {
                    return false;
                }

                switch (c.ComparisonOperation.PropertyType)
                {
                    case QueryablePropertyTypeCode.Any:
                        switch (prop.PropertyType)
                        {
                            case QueryablePropertyTypeCode.Text:
                                return IsValueOfTypeText(c.RightOperand);
                            case QueryablePropertyTypeCode.Numeric:
                                return IsValueOfTypeNumeric(c.RightOperand);
                            case QueryablePropertyTypeCode.DateTime:
                                return IsValueOfTypeDateTime(c.RightOperand);
                            case QueryablePropertyTypeCode.Boolean:
                                return IsValueOfTypeBoolean(c.RightOperand);
                            case QueryablePropertyTypeCode.Guid:
                                return IsValueOfTypeGuid(c.RightOperand);
                            default:
                                return false;
                        }
                    case QueryablePropertyTypeCode.Scalar:
                        switch (prop.PropertyType)
                        {
                            case QueryablePropertyTypeCode.Numeric:
                                return IsValueOfTypeNumeric(c.RightOperand);
                            case QueryablePropertyTypeCode.DateTime:
                                return IsValueOfTypeDateTime(c.RightOperand);
                            default:
                                return false;
                        }
                    case QueryablePropertyTypeCode.Numeric:
                        return (prop.PropertyType == QueryablePropertyTypeCode.Numeric) && IsValueOfTypeNumeric(c.RightOperand);
                    case QueryablePropertyTypeCode.Text:
                        return (prop.PropertyType == QueryablePropertyTypeCode.Text) && IsValueOfTypeText(c.RightOperand);
                    case QueryablePropertyTypeCode.Boolean:
                        return (prop.PropertyType == QueryablePropertyTypeCode.Boolean) && IsValueOfTypeBoolean(c.RightOperand);
                    case QueryablePropertyTypeCode.DateTime:
                        return (prop.PropertyType == QueryablePropertyTypeCode.DateTime) && IsValueOfTypeDateTime(c.RightOperand);
                    case QueryablePropertyTypeCode.Guid:
                        return (prop.PropertyType == QueryablePropertyTypeCode.Guid) && IsValueOfTypeGuid(c.RightOperand);
                    default:
                        return false;

                }
            }
            return true;
        }

        /// <summary>
        /// Validate an Aggregation against the QueryValidator's QueryableObjectTypeInfo and the Aggregation type
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Validate(Aggregation statement)
        {
            var prop = typeInfo.GetPropertyTypeInfo(statement.PropertyName);
            if (prop == null || (int)prop.PropertyType < (int)QueryablePropertyTypeCode.Undefined)
            {
                return false;
            }

            switch (statement.AggregationOperation.PropertyType)
            {
                case QueryablePropertyTypeCode.Any:
                    switch (prop.PropertyType)
                    {
                        case QueryablePropertyTypeCode.Text:
                        case QueryablePropertyTypeCode.Numeric:
                        case QueryablePropertyTypeCode.DateTime:
                        case QueryablePropertyTypeCode.Boolean:
                        case QueryablePropertyTypeCode.Guid:
                            return true;
                        default:
                            return false;
                    }
                case QueryablePropertyTypeCode.Scalar:
                    switch (prop.PropertyType)
                    {
                        case QueryablePropertyTypeCode.Numeric:
                        case QueryablePropertyTypeCode.DateTime:
                            return true;
                        default:
                            return false;
                    }
                case QueryablePropertyTypeCode.Numeric:
                    return prop.PropertyType == QueryablePropertyTypeCode.Numeric;
                case QueryablePropertyTypeCode.Text:
                    return prop.PropertyType == QueryablePropertyTypeCode.Text;
                case QueryablePropertyTypeCode.Boolean:
                    return prop.PropertyType == QueryablePropertyTypeCode.Boolean;
                case QueryablePropertyTypeCode.DateTime:
                    return prop.PropertyType == QueryablePropertyTypeCode.DateTime;
                case QueryablePropertyTypeCode.Guid:
                    return prop.PropertyType == QueryablePropertyTypeCode.Guid;
                default:
                    return false;
            }
        }


        /// <summary>
        /// Validates that the value provided is contains a Numeric Type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValueOfTypeNumeric(string value)
        {
            var isNumber = double.TryParse(value, options.NumberStyles, options.CultureInfo, out double number);
            var isMissingDecimal = value.EndsWith(".");
            return (isNumber && !isMissingDecimal);
        }

        /// <summary>
        /// Validates that the value provided is a Boolean type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValueOfTypeBoolean(string value)
        {
            if (value == "true" || value == "false")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates that the value provided is a Boolean type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValueOfTypeDateTime(string value)
        {
            bool isDateTime = System.DateTime.TryParseExact(value, options.DateTimeFormats, options.CultureInfo, options.DateTimeStyles, out var dateTime);
            return isDateTime;
        }

        /// <summary>
        /// Validates that the value provided is a Guid type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValueOfTypeGuid(string value)
        {
            var isGuid = System.Guid.TryParse(value, out var guid);
            return isGuid;
        }

        /// <summary>
        /// Validates that the value provided is a String Type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValueOfTypeText(string value)
        {
            var text = value.ToString().Trim();
            var textParsed = Regex.Split(text, StringExtensions.StringLiteralRegex);
            if (textParsed.Length != 1)
            {
                return false;
            }
            if (IsStringLiteral(textParsed.FirstOrDefault()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates that the value is a string literal
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        protected bool IsStringLiteral(string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return true;
            }
            return false;
        }


    }
}
