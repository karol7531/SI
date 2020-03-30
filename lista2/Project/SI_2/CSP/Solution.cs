using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class Solution<T>
    {
        public Dictionary<Variable<T>, T> assignments { get; private set; }
        private List<Variable<T>> unassignedVariables;
        private List<Variable<T>> variables;

        public Solution(Dictionary<Variable<T>, T> assignments, List<Variable<T>> variables) : this(assignments, variables, UnassignedVariables(assignments, variables)) { }

        private Solution(Dictionary<Variable<T>, T> assignments, List<Variable<T>> variables, List<Variable<T>> unassignedVariables)
        {
            this.assignments = assignments;
            this.variables = variables;
            this.unassignedVariables = unassignedVariables;
        }

        private Dictionary<Variable<T>, T> AssignmentsClone => assignments.ToDictionary(entry => entry.Key, entry => entry.Value);

        internal Solution<T> Clone(bool deepCopy = false)
        {
            if (deepCopy)
            {
                var newVariables = variables.Select(v => v.Clone());
                return new Solution<T>(AssignmentsCloneDeep(newVariables), newVariables);
            }
            return new Solution<T>(AssignmentsClone, variables.ToList(), unassignedVariables.ToList());
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
            unassignedVariables.Remove(variable);
            assignments[variable] = value;
        }

        internal void FilterOutDomains(Variable<T> variable)
        {
            var value = assignments[variable];
            var unassignedInDomain = unassignedVariables.Where(v => v.domain.desc == variable.domain.desc);
            foreach(var ud in unassignedInDomain)
            {
                //chyba potrzene deepcopy variables a nie domains
                ud.domain.values.Remove(value);
            }
        }

        private static List<Variable<T>> UnassignedVariables(Dictionary<Variable<T>, T> assignments, List<Variable<T>> variables) 
            => variables.Where(v => !assignments.ContainsKey(v)).ToList();
    }
}
