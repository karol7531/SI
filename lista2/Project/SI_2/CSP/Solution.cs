using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class Solution<T>
    {
        public Dictionary<Variable<T>, T> assignments { get; private set; }
        internal List<Variable<T>> unassignedVariables;
        internal List<Variable<T>> variables;

        public Solution(Dictionary<Variable<T>, T> assignments, List<Variable<T>> variables) : this(assignments, variables, UnassignedVariables(assignments, variables)) { }

        private Solution(Dictionary<Variable<T>, T> assignments, List<Variable<T>> variables, List<Variable<T>> unassignedVariables)
        {
            this.assignments = assignments;
            this.variables = variables;
            this.unassignedVariables = unassignedVariables;
        }

        private Dictionary<Variable<T>, T> AssignmentsClone => assignments.ToDictionary(entry => entry.Key, entry => entry.Value);

        public Solution<T> Clone(bool deepCopy = false)
        {
            if (deepCopy)
            {
                var newVariables = variables.Select(v => v.Clone()).ToList();
                return new Solution<T>(AssignmentsCloneDeep(newVariables), newVariables);
            }
            return new Solution<T>(AssignmentsClone, variables.ToList(), unassignedVariables.ToList());
        }

        private Dictionary<Variable<T>, T> AssignmentsCloneDeep(List<Variable<T>> newVariables)
        {
            Dictionary<Variable<T>, T> newAssignments = new Dictionary<Variable<T>, T>();
            foreach (KeyValuePair<Variable<T>, T> entry in assignments)
            {
                Variable<T> newVariable = newVariables.Where(v => v.Equals(entry.Key)).First();
                newAssignments[newVariable] = entry.Value;
            }
            return newAssignments;
        }

        internal bool Check(Variable<T> variable, T valToCheck)
        {
            foreach(var c in variable.constraints)
            {
                if (!c.satisfy(c.restriction, this, valToCheck)) 
                {
                    return false;
                }
            }
            return true;
        }

        internal void AssignInvariables()
        {
            foreach(var a in assignments)
            {

            }
        }

        internal void Assign(Variable<T> variable, T value)
        {
            unassignedVariables.Remove(variable);
            assignments[variable] = value;
        }

        //chcemy usunąć im z dzieziny pewną wartość wtedy kiedy wskazuje na to jakiś constraint
        internal void FilterOutDomains(Variable<T> variable)
        {
            //dla każdej nieprzypisanej zmiennej
            foreach(var uv in unassignedVariables)
            {
                // dla każdego jej ograniczenia
                foreach(var c in uv.constraints)
                {
                    //jeśli jej restrykcja skierowana jest na podaną zmienną
                    if(c.restriction is Variable<T> && variable.Equals(c.restriction))
                    {
                        //values that do not meet the restrictions
                        List<T> toDel = new List<T>();
                        //to przeszukaj dziedzinę tej nieprzypisanej zmiennej 
                        foreach (var val in uv.domain.values)
                        {
                            //jak jakaś wartość nie spełnia ograniczeń to ją usuń z dziedziny
                            if (!c.satisfy(variable, this, val))
                            {
                                toDel.Add(val);
                            }
                        }
                        foreach (var td in toDel)
                        {
                            uv.domain.values.Remove(td);
                        }
                        //for values also

                    }
                }
            }
        }

        internal void FilterOutDomains()
        {
            foreach(var a in assignments)
            {
                a.Key.SetDomain(new Domain<T>(new List<T>() { a.Value }, a.Key.domain.desc));
                FilterOutDomains(a.Key);
            }//assign all certain
        }

        internal bool DomainsNotEmpty()
        {
            foreach (var u in unassignedVariables)
            {
                if (u.domain.values.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<Variable<T>> UnassignedVariables(Dictionary<Variable<T>, T> assignments, List<Variable<T>> variables)
            => variables.Where(v => !assignments.ContainsKey(v)).ToList();

        internal void ApplyVariableHeuristic(VariableHeuristicType variableHeuristicType)
        {
            switch (variableHeuristicType)
            {
                case VariableHeuristicType.Random:
                    {
                        var r = new Random();
                        unassignedVariables = unassignedVariables.OrderBy(x => r.Next()).ToList();
                        break;
                    }
                case VariableHeuristicType.SmallestDomain:
                    {
                        unassignedVariables = unassignedVariables.OrderBy(v => v.domain.values.Count).ToList();
                        break;
                    }
            }
        }

        internal void ApplyValueHeuristic(ValueHeuristicType valueHeuristicType)
        {
            switch (valueHeuristicType)
            {
                case ValueHeuristicType.Random:
                    {
                        var r = new Random();
                        foreach (var uv in unassignedVariables)
                        {
                            uv.domain.values = uv.domain.values.OrderBy(x => r.Next()).ToList();
                        }
                        break;
                    }
            }
        }
    }
}
