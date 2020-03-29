using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class Solution<T>
    {
        public Dictionary<Variable<T>, T> assignments { get; private set; }

        public Solution(Dictionary<Variable<T>, T> assignments)
        {
            this.assignments = assignments;
        }

        internal Solution<T> Clone()
        {
            return new Solution<T>(assignments.ToDictionary(entry => entry.Key, entry => entry.Value));
        }

        internal bool Check(Variable<T> variable, T valToCheck)
        {
            foreach(var c in variable.constraints)
            {
                if (!c.satisfy(variable, c.restriction, this, valToCheck)) 
                {
                    return false;
                }
            }
            return true;
        }

        internal void Assign(Variable<T> variable, T value)
        {
            assignments[variable] = value;
        }
    }
}
