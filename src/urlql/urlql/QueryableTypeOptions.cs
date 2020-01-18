using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace urlql
{
    public class QueryableTypeOptions
    {
        private static string[] _excludedAttributeNames = {
            "System.Runtime.Serialization.IgnoreDataMember",
            "System.Runtime.Serialization.NonSerialized",
            "System.Text.Json.Serialization.JsonIgnoreAttribute",
            "Newtonsoft.Json.JsonIgnoreAttribute",
        };


        /// <summary>
        /// Global Ignore Properties with these Attributes
        /// </summary>
        public static ICollection<string> DefaultExcludedAttributeNames = _excludedAttributeNames;

        /// <summary>
        /// Ignore Properties with these Attributes
        /// </summary>
        public IReadOnlyCollection<string> ExcludedAttributeNames;
        

        /// <summary>
        /// Global Ignore Properties without these Attributes
        /// </summary>
        public static ICollection<string> DefaultRequiredAttributeNames = new string[0];
               
        /// <summary>
        /// Ignore Properties without these Attributes
        /// </summary>
        public IReadOnlyCollection<string> RequiredAttributeNames;


        public QueryableTypeOptions(ICollection<string> excludedAttributeNames = null, ICollection<string> requiredAttributeNames = null)
        {
            ExcludedAttributeNames = (excludedAttributeNames ?? DefaultExcludedAttributeNames).ToList().AsReadOnly();
            RequiredAttributeNames = (requiredAttributeNames ?? DefaultRequiredAttributeNames).ToList().AsReadOnly();
        }
    }
}
