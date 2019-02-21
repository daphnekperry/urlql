using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace urlql.mergepatch
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

        public bool Validate()
        {
            List<string> memberNames = new List<string>();

            memberNames.AddRange(Type.GetProperties().Where(p => p.SetMethod != null).Select(p => p.Name));
            memberNames.AddRange(Type.GetFields().Where(f => f.IsPublic).Select(f => f.Name));

            return !this.Properties().Select(p => p.Name).Except(memberNames).Any();
        }

        public void Apply(T target)
        {
        }
    }
}
