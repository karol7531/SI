using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class Domain<T>
    {
        public List<T> values { get; internal set; }
        public string desc { get; private set; }

        public Domain(List<T> values, string desc = "")
        {
            this.values = values;
            this.desc = desc;
        }

        public Domain<T> Clone() => new Domain<T>(values.ToList(), desc);

        public override string ToString()
        {
            string result = "{";
            foreach(var val in values)
            {
                result += val + ", ";
            }
            return result + "}";
        }
    }
}
