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
        private static ConcurrentDictionary<Guid, IDictionary<string, QueryablePropertyTypeInfo>> _typeCache = new ConcurrentDictionary<Guid, IDictionary<string, QueryablePropertyTypeInfo>>();

        protected ConcurrentDictionary<Guid, IDictionary<string, QueryablePropertyTypeInfo>> TypeCache = _typeCache;

        private static QueryableTypeOptions _typeOptions = new QueryableTypeOptions();

        public static QueryableTypeOptions TypeOptions
        {
            get { return _typeOptions; }
            set
            {
                var lockObj = new object();
                lock (lockObj)
                {
                    _typeCache = new ConcurrentDictionary<Guid, IDictionary<string, QueryablePropertyTypeInfo>>();
                    _typeOptions = value;
                }
            }
        }

        /// <summary>
        /// CLR Type
        /// </summary>
        public readonly Type ClrType;

        /// <summary>
        /// Property Name and Query Property Type definitions
        /// </summary>
        protected IDictionary<string, QueryablePropertyTypeInfo> propertyDefinitions = new Dictionary<string, QueryablePropertyTypeInfo>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectType"></param>
        public QueryableObjectTypeInfo(Type objectType)
        {
            ClrType = objectType;
            bool cached = TypeCache.TryGetValue(ClrType.GUID, out var propertyDefs);

            if (cached)
            {
                propertyDefinitions = propertyDefs;
            }
            else
            {
                var props = ClrType.GetProperties();
                foreach (var p in props)
                {
                    var propertyName = p.Name;
                    if (AccessableProperty(p, TypeOptions.ExcludedAttributeNames, TypeOptions.RequiredAttributeNames))
                    {
                        var queryType = QueryablePropertyTypeInfo.GetQueryType(p.PropertyType);
                        propertyDefinitions.Add(propertyName, queryType);
                    }
                }

                var fields = ClrType.GetFields();
                foreach (var f in fields)
                {
                    var propertyName = f.Name;
                    if (AccessableProperty(f, TypeOptions.ExcludedAttributeNames, TypeOptions.RequiredAttributeNames))
                    {
                        var queryType = QueryablePropertyTypeInfo.GetQueryType(f.DeclaringType);
                        propertyDefinitions.Add(propertyName, queryType);
                    }
                }

                TypeCache.TryAdd(ClrType.GUID, propertyDefinitions);
            }
        }

        /// <summary>
        /// Get the Query Type for an Object's Property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public QueryablePropertyTypeInfo GetPropertyTypeInfo(string propertyName)
        {
            return propertyDefinitions.Where(d=> d.Key.ToLowerInvariant() == propertyName.ToLowerInvariant()).SingleOrDefault().Value;
        }

        /// <summary>
        /// Test to see if a property should be visible in the output and available for comparison/selection
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected bool AccessableProperty(MemberInfo memberInfo, IEnumerable<string> ignoreAttributeNames, IEnumerable<string> requiredAttributeNames = null)
        {
            var attributeNames  = memberInfo.GetCustomAttributes(false).Select(a => a.GetType().Name).ToList();
            // Filter by properties
            if (attributeNames.Intersect(ignoreAttributeNames).Any() || attributeNames.Except(requiredAttributeNames ?? new List<string>().ToArray()).Count() != attributeNames.Count)
            {
                return false;
            }
            return true;
        }
    }
}
