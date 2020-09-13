using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using urlql.mergepatch.Internal;

namespace urlql
{
    public class MergePatch<T> : JObject
    {
        [Newtonsoft.Json.JsonIgnore]
        public new Type Type { get; protected set; } = typeof(T);

        public JTokenType JObjectType => base.Type;

        public MergePatch(JObject baseObject)
            : base(baseObject)
        {
        }

        public MergePatch(string json)
            : base(JsonConvert.DeserializeObject<JObject>(json))
        {
        }

        public bool IsValid()
        {
            // Get all assignable members
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
                    // TODO: Cleanup this uglyness and cache type info.*
                    // TODO: String handling is really permissive on input, we need to do hard checks for string literals and enforce that
                    var x =
                    Type.GetProperties()
                        .Where(p => p.SetMethod != null && p.Name.CompareCaseInsensitive(prop.Name))
                        ?.FirstOrDefault()?.PropertyType
                    ??
                    Type.GetFields()
                        .Where(f => f.IsPublic && f.Name.CompareCaseInsensitive(prop.Name))
                        ?.FirstOrDefault()?.FieldType;
                    System.Convert.ChangeType(prop.Value.ToString(), x);
                }
                catch
                {
                    return false;
                }
            }

            // TODO: Handle value types structs, ignore objects and foreign keys as this can really
            //       screw up relational Id fields/navigation properties or at least start chucking
            //       lots of ugly exceptions at the data access layer for consumers.
            //       Also consider making optional/opt-in.
            // TODO: Consider member properties such as JsonIgnore, IgnoreDataMember, and NonSerialized

            return true;
        }

        public void Apply(T target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (!target.GetType().IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"Cannot apply MergePatch<{typeof(T)}> to target of type {target.GetType()}");
            }

            if (!IsValid())
            {
                return;
            }

            foreach (var prop in this.Properties())
            {
                var clrProperty = Type.GetProperties().Where(p => p.SetMethod != null && p.Name.CompareCaseInsensitive(prop.Name))?.FirstOrDefault();
                if (clrProperty != null)
                {
                    clrProperty.SetValue(target, System.Convert.ChangeType(prop.Value.ToString(), clrProperty.PropertyType));
                    continue;
                }
                var clrField = Type.GetFields().Where(f => f.IsPublic && f.Name.CompareCaseInsensitive(prop.Name))?.FirstOrDefault();
                if (clrField != null)
                {
                    clrField.SetValue(target, System.Convert.ChangeType(prop.Value.ToString(), clrField.FieldType));
                }
            }
        }
    }
}
