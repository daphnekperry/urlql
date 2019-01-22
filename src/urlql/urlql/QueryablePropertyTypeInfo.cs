using System;
using System.Collections.Generic;
using System.Text;

namespace urlql
{
    /// <summary>
    /// Query Property Type
    /// </summary>
    public class QueryablePropertyTypeInfo : IEquatable<QueryablePropertyTypeInfo>
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
        public readonly QueryablePropertyType PropertyType;

        /// <summary>
        /// Enumerable Values
        /// </summary>
        protected readonly int Value;

        /// <summary>
        /// Object Type
        /// </summary>
        public static readonly QueryablePropertyTypeInfo Object = new QueryablePropertyTypeInfo("Object", QueryablePropertyType.Reference, typeof(object));
        /// <summary>
        /// Numeric Type
        /// </summary>
        public static readonly QueryablePropertyTypeInfo Numeric = new QueryablePropertyTypeInfo("Numeric", QueryablePropertyType.Numeric, typeof(double));
        /// <summary>
        /// Text/String Type
        /// </summary>
        public static readonly QueryablePropertyTypeInfo Text = new QueryablePropertyTypeInfo("Text", QueryablePropertyType.Text, typeof(string));
        /// <summary>
        /// Boolean Type
        /// </summary>
        public static readonly QueryablePropertyTypeInfo Boolean = new QueryablePropertyTypeInfo("Boolean", QueryablePropertyType.Boolean, typeof(bool));
        /// <summary>
        /// DateTime Type
        /// </summary>
        public static readonly QueryablePropertyTypeInfo DateTime = new QueryablePropertyTypeInfo("DateTime", QueryablePropertyType.DateTime, typeof(DateTime));
        /// <summary>
        /// Guid Type
        /// </summary>
        public static readonly QueryablePropertyTypeInfo Guid = new QueryablePropertyTypeInfo("Guid", QueryablePropertyType.Guid, typeof(Guid));


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="queryType"></param>
        /// <param name="clrType"></param>
        protected QueryablePropertyTypeInfo(string typeName, QueryablePropertyType propType, Type clrType)
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
        public static QueryablePropertyTypeInfo GetQueryType(Type type)
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

        public bool Equals(QueryablePropertyTypeInfo other)
        {
            return (this.PropertyType == other.PropertyType && this.ClrType == other.ClrType);
        }
    }
}
