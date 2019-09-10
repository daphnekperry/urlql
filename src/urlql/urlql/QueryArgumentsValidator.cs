using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class QueryArgumentsValidator
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
        public QueryArgumentsValidator(QueryOptions options, QueryableObjectTypeInfo typeInfo)
        {
            this.options = options;
            this.typeInfo = typeInfo;
        }

        public bool Validate(Paging statement)
        {
            if (statement.Take > options.MaximumPageSize)
            {
                return false;
            }
            return true;
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
            if (!IsValidIdentifier(statement.Alias?.NewName ?? @"true"))
            {
                return false;
            }

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

        /// <summary>
        /// Validate an Aggregation against the QueryValidator's QueryableObjectTypeInfo and the Aggregation type
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Validate(Aggregation aggregation)
        {
            var prop = typeInfo.GetPropertyTypeInfo(aggregation.PropertyName);
            if (prop == null || (int)prop.PropertyType < (int)QueryablePropertyType.Any)
            {
                return false;
            }

            return IsOperationValidForType(prop.PropertyType, aggregation.AggregationOperation.PropertyType);
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
                if (prop == null || (int)prop.PropertyType < (int)QueryablePropertyType.Undefined)
                {
                    return false;
                }

                return IsOperationValidForType(prop.PropertyType, c.ComparisonOperation.PropertyType) && IsValueValidForType(prop.PropertyType, c.RightOperand);
            }
            return true;
        }

        /// <summary>
        /// Validate an IGroupingStatement against the QueryValidator's QueryableObjectTypeInfo
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public bool Validate(IGroupingStatement statement)
        {
            var prop = typeInfo.GetPropertyTypeInfo(statement.PropertyName);
            if (prop == null)
            {
                return false;
            }
            return true;
        }

        protected bool IsOperationValidForType(QueryablePropertyType propertyType, QueryablePropertyType operationType)
        {
            switch (operationType)
            {
                case QueryablePropertyType.Any:
                    switch (propertyType)
                    {
                        case QueryablePropertyType.Text:
                        case QueryablePropertyType.Numeric:
                        case QueryablePropertyType.DateTime:
                        case QueryablePropertyType.Boolean:
                        case QueryablePropertyType.Guid:
                            return true;
                        default:
                            return false;
                    }
                case QueryablePropertyType.Scalar:
                    switch (propertyType)
                    {
                        case QueryablePropertyType.Numeric:
                        case QueryablePropertyType.DateTime:
                            return true;
                        default:
                            return false;
                    }
                case QueryablePropertyType.Undefined:
                case QueryablePropertyType.Object:
                    return false;
                default:
                    return operationType == propertyType;
            }
        }

        protected bool IsValueValidForType(QueryablePropertyType typeCode, string value)
        {
            switch (typeCode)
            {
                case QueryablePropertyType.Text:
                    return IsValueOfTypeText(value);
                case QueryablePropertyType.Numeric:
                    return IsValueOfTypeNumeric(value);
                case QueryablePropertyType.DateTime:
                    return IsValueOfTypeDateTime(value);
                case QueryablePropertyType.Boolean:
                    return IsValueOfTypeBoolean(value);
                case QueryablePropertyType.Guid:
                    return IsValueOfTypeGuid(value);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Is Valid Identifier
        /// </summary>
        /// <param name="identifierName"></param>
        ///<remarks>Lovingly borrowed from https://stackoverflow.com/a/51181944 </remarks>
        protected bool IsValidIdentifier(string identifierName)
        {
            if (string.IsNullOrEmpty(identifierName))
            {
                throw new ArgumentNullException(nameof(identifierName));
            }

            // Technically the input must be in normal form C. Implementations aren't required
            // to verify that though, so you could remove this check if your runtime doesn't
            // mind.
            if (!identifierName.IsNormalized())
            {
                return false;
            }

            // Convert escape sequences to the characters they represent. The only allowed escape
            // sequences are of form \u0000 or \U0000, where 0 is a hex digit.
            MatchEvaluator replacer = (Match match) =>
            {
                string hex = match.Groups[1].Value;
                var codepoint = int.Parse(hex, NumberStyles.HexNumber);
                return new string((char)codepoint, 1);
            };
            var escapeSequencePattern = @"\\[Uu]([\dA-Fa-f]{4})";
            var withoutEscapes = Regex.Replace(identifierName, escapeSequencePattern, replacer, RegexOptions.CultureInvariant);

            // Now do the real check.
            var isIdentifier = @"^@?[\p{L}\p{Nl}_][\p{Cf}\p{L}\p{Mc}\p{Mn}\p{Nd}\p{Nl}\p{Pc}]*$";
            return Regex.IsMatch(withoutEscapes, isIdentifier, RegexOptions.CultureInvariant);
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
        /// Validates that the value provided is a String type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValueOfTypeText(string value)
        {
            var text = value.ToString().Trim();
            var textParsed = text.Tokenize();
            if (textParsed.Count() != 1)
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
