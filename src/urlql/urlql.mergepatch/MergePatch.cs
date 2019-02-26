using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace urlql
{
    public class MergePatch<T> : JObject
    {
        [Newtonsoft.Json.JsonIgnore]
        public new Type Type { get; protected set; }

        public JTokenType JObjectType => base.Type;

        public MergePatch(JObject baseObject)
            : base(baseObject)
        {
            Type = typeof(T);
        }

        public bool IsValid()
        {
            List<string> memberNames = new List<string>();

            memberNames.AddRange(Type.GetProperties().Where(p => p.SetMethod != null).Select(p => p.Name.ToLowerInvariant()));
            memberNames.AddRange(Type.GetFields().Where(f => f.IsPublic).Select(f => f.Name.ToLowerInvariant()));

            if (this.Properties().Select(p => p.Name.ToLowerInvariant()).Except(memberNames).Any())
            {
                return false;
            }

            foreach (var prop in this.Properties())
            {
                try
                {
                    // TODO: Cleanup this uglyness and cache type info.
                    // TODO: String handling is really permissive on input, we need to do hard checks for string literals and enforce that
                    var x = Type.GetProperties().Where(p => p.SetMethod != null && p.Name.ToLowerInvariant() == prop.Name.ToLowerInvariant()).FirstOrDefault()?.PropertyType ?? Type.GetFields().Where(f => f.IsPublic && f.Name.ToLowerInvariant() == prop.Name.ToLowerInvariant()).FirstOrDefault()?.FieldType;
                    System.Convert.ChangeType(prop.Value.ToString(), x);
                }
                catch
                {
                    return false;
                }
            }

            // TODO: Handle value types structs, ignore objects and foreign keys (this can screw up relational Id fields/navigation properties
            //       or start chucking lots of ugly exceptions at the data access layer for consumers)
            // TODO: Consider member properties such as JsonIgnore, IgnoreDataMember, and NonSerialized

            return true;
        }

        public void Apply(T target)
        {
            if (target.GetType() != typeof(T))
            {
                return; // TODO: Throw good exception here
            }

            if (!IsValid())
            {
                return;
            }

            foreach (var prop in this.Properties())
            {
                var clrProperty = Type.GetProperties().Where(p => p.SetMethod != null && p.Name.ToLowerInvariant() == prop.Name.ToLowerInvariant()).FirstOrDefault();
                if (clrProperty != null)
                {
                    clrProperty.SetValue(target, System.Convert.ChangeType(prop.Value.ToString(), clrProperty.PropertyType));
                    continue;
                }
                var clrField = Type.GetFields().Where(f => f.IsPublic && f.Name.ToLowerInvariant() == prop.Name.ToLowerInvariant()).FirstOrDefault();
                if (clrField != null)
                {
                    clrField.SetValue(target, System.Convert.ChangeType(prop.Value.ToString(), clrField.FieldType));
                }
            }
        }
    }
}
