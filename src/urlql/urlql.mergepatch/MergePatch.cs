using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
