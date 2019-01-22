using System;
using System.Collections.Generic;
using System.Text;

namespace urlql
{
    /// <summary>
    /// Query Property Type
    /// </summary>
    public class QueryablePropertyType
    {
        /// <summary>
        ///  Type Name
        /// </summary>
        public readonly string TypeName;

        /// <summary>
        /// Corresponding CLR Type
        /// </summary>
        protected readonly Type ClrType;

        /// <summary>
        /// Operation Type(s)
        /// </summary>
        public readonly QueryablePropertyTypeCode PropertyType;

        /// <summary>
        /// Enumerable Values
        /// </summary>
        protected readonly int Value;


        /// <summary>
        /// Object Type
        /// </summary>
        public static readonly QueryablePropertyType Object = new QueryablePropertyType("Object", QueryablePropertyTypeCode.Reference, typeof(object));
        /// <summary>
        /// Numeric Type
        /// </summary>
        public static readonly QueryablePropertyType Numeric = new QueryablePropertyType("Numeric", QueryablePropertyTypeCode.Numeric, typeof(double));
        /// <summary>
        /// Text/String Type
        /// </summary>
        public static readonly QueryablePropertyType Text = new QueryablePropertyType("Text", QueryablePropertyTypeCode.Text, typeof(string));
        /// <summary>
        /// Boolean Type
        /// </summary>
        public static readonly QueryablePropertyType Boolean = new QueryablePropertyType("Boolean", QueryablePropertyTypeCode.Boolean, typeof(bool));
        /// <summary>
        /// DateTime Type
        /// </summary>
        public static readonly QueryablePropertyType DateTime = new QueryablePropertyType("DateTime", QueryablePropertyTypeCode.DateTime, typeof(DateTime));
        /// <summary>
        /// Guid Type
        /// </summary>
        public static readonly QueryablePropertyType Guid = new QueryablePropertyType("Guid", QueryablePropertyTypeCode.Guid, typeof(Guid));


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="queryType"></param>
        /// <param name="clrType"></param>
        protected QueryablePropertyType(string typeName, QueryablePropertyTypeCode propType, Type clrType)
        {
            TypeName = typeName;
            ClrType = clrType;
            PropertyType = propType;
            Value = (int)propType;
        }

        /// <summary>
        /// Get Query Type for Corresponding CLR Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static QueryablePropertyType GetQueryType(Type type)
        {
            var typeToCheck = Nullable.GetUnderlyingType(type) ?? type;
            switch (Type.GetTypeCode(typeToCheck))
            {
                case TypeCode.String:
                    return Text;
                case TypeCode.Boolean:
                    return Boolean;
                case TypeCode.DateTime:
                    return DateTime;
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Numeric;
                case TypeCode.Object:
                default:
                    if (typeToCheck == typeof(System.Guid))
                    {
                        return Guid;
                    }
                    return Object;
            }
        }
    }
}
