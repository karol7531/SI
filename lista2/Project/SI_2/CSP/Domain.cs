using System.Collections.Generic;

namespace CSP
{
    public class Domain<T>
    {
        public List<T> values { get; private set; }
        public string desc { get; private set; }

        public Domain(List<T> values, string desc = "")
        {
            this.values = values;
            this.desc = desc;
        }

    }
}
