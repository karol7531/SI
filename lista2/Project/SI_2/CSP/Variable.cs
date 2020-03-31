using System;
using System.Collections.Generic;
using System.Linq;

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

        public void SetDomain(Domain<T> domain) => this.domain = domain;

        internal Variable<T> Clone() 
        {
           return new Variable<T>(id, domain.Clone(), constraints, desc); 
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Variable<T> other = (Variable<T>)obj;
                return  this.ToString() == other.ToString();
            }
        }

        public override string ToString() => this.id + this.desc;
    }
}
