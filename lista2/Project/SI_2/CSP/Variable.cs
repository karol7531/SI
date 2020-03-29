using System.Collections.Generic;

namespace CSP
{
    public class Variable<T>
    {
        public string desc { get; private set; }
        public int id { get; private set; }
        public Domain<T> domain { get; private set; }
        public List<Constraint<T>> constraints { get; internal set; }

        public Variable(int id, Domain<T> domain, List<Constraint<T>> constraints, string desc = "")
        {
            this.id = id;
            this.desc = desc;
            this.domain = domain;
            this.constraints = constraints;
        }

        public Variable(int id, Domain<T> domain, string desc = "") : this(id, domain, new List<Constraint<T>>(), desc) { }
    }
}
