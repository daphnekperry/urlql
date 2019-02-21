using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace urlql
{
    public class QueryableObjectTypeInfo
    {
        protected static ConcurrentDictionary<Guid, IDictionary<string, QueryablePropertyTypeInfo>> TypeCache = new ConcurrentDictionary<Guid, IDictionary<string, QueryablePropertyTypeInfo>>();

        /// <summary>
        /// CLR Type
        /// </summary>
        public readonly Type ClrType;

        /// <summary>
        /// Property Name and Query Property Type definitions
        /// </summary>
        protected IDictionary<string, QueryablePropertyTypeInfo> typeDefinitions = new Dictionary<string, QueryablePropertyTypeInfo>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectType"></param>
        public QueryableObjectTypeInfo(Type objectType, QueryOptions options)
        {
            ClrType = objectType;
            bool cached = TypeCache.TryGetValue(ClrType.GUID, out var typeDef);

            if (cached)
            {
                typeDefinitions = typeDef;
            }
            else
            {
                var props = ClrType.GetProperties();
                foreach (var p in props)
                {
                    var propertyName = p.Name;
                    if (VisibleProperty(p, options.IgnoreAttributeNames))
                    {
                        var queryType = QueryablePropertyTypeInfo.GetQueryType(p.PropertyType);
                        typeDefinitions.Add(propertyName, queryType);
                    }
                }

                var fields = ClrType.GetFields();
                foreach (var f in fields)
                {
                    var propertyName = f.Name;
                    if (VisibleProperty(f, options.IgnoreAttributeNames))
                    {
                        var queryType = QueryablePropertyTypeInfo.GetQueryType(f.DeclaringType);
                        typeDefinitions.Add(propertyName, queryType);
                    }
                }

                TypeCache.TryAdd(ClrType.GUID, typeDefinitions);
            }
        }

        /// <summary>
        /// Get the Query Type for an Object's Property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public QueryablePropertyTypeInfo GetPropertyTypeInfo(string propertyName)
        {
            return typeDefinitions.Where(d=> d.Key.ToLowerInvariant() == propertyName.ToLowerInvariant()).SingleOrDefault().Value;
        }

        /// <summary>
        /// Test to see if a property should be visible in the output and available for comparison/selection
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected bool VisibleProperty(MemberInfo memberInfo, string[] ignoreAttributeNames)
        {
            var attributeNames  = memberInfo.GetCustomAttributes(false).Select(a => a.GetType().Name).ToList();
            // Filter out properties to ignore
            if (attributeNames.Intersect(ignoreAttributeNames).Any())
            {
                return false;
            }
            return true;
        }
    }
}
