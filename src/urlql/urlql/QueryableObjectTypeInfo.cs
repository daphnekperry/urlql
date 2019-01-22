using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace urlql
{
    public class QueryableObjectTypeInfo
    {
        /// <summary>
        /// CLR Type
        /// </summary>
        public readonly Type ObjectType;

        /// <summary>
        /// Property Name and Query Property Type definitions
        /// </summary>
        protected IDictionary<string, QueryablePropertyType> typeDefinitions = new Dictionary<string, QueryablePropertyType>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectType"></param>
        public QueryableObjectTypeInfo(Type objectType)
        {
            ObjectType = objectType;
            var props = ObjectType.GetProperties();

            foreach (var p in props)
            {
                var propertyName = p.Name;
                if (VisibleProperty(p))
                {
                    var queryType = QueryablePropertyType.GetQueryType(p.PropertyType);
                    typeDefinitions.Add(propertyName, queryType);
                }
            }

            var fields = ObjectType.GetFields();
            foreach (var f in fields)
            {
                var propertyName = f.Name;
                if (VisibleProperty(f))
                {
                    var queryType = QueryablePropertyType.GetQueryType(f.DeclaringType);
                    typeDefinitions.Add(propertyName, queryType);
                }
            }
        }

        /// <summary>
        /// Get the Query Type for an Object's Property
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public QueryablePropertyType GetPropertyTypeInfo(string propertyName)
        {
            return typeDefinitions.Where(d=> d.Key.ToLowerInvariant() == propertyName.ToLowerInvariant()).SingleOrDefault().Value;
        }

        /// <summary>
        /// Test to see if a property should be visible in the output and available for comparison/selection
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected bool VisibleProperty(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes(false).Select(a => a.GetType().Name);
            // Filter out properties to ignore
            if (attributes.Where(a => a.ToLowerInvariant().StartsWith("jsonignore")).Any())
            {
                return false;
            }
            return true;
        }
    }
}
