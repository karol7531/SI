using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSP
{
    public class Problem<T>
    {
        public List<Variable<T>> variables { get; private set; }
        public List<Domain<T>> domains { get; private set; }
        public List<Constraint<T>> constraints { get; private set; }
        private List<Solution<T>> solutions = new List<Solution<T>>();
        private Dictionary<Variable<T>, T> invariables;

        public Problem(List<Variable<T>> variables, List<Domain<T>> domains, List<Constraint<T>> constraints, Dictionary<Variable<T>, T> invariables)
        {
            this.variables = variables;
            this.domains = domains;
            this.constraints = constraints;
            this.invariables = invariables;
        }

        /// <summary>
        /// Selects those constraints where variable is present and then assigns them to this variable
        /// </summary>
        public void AssignConstraints()
        {
            foreach (Variable<T> v in variables)
            {
                v.constraints = (from c in constraints where c.variable == v select c).ToList();
            }
        }

        Stopwatch stopwatch = new Stopwatch();
        public List<Solution<T>> Solve()
        {
            stopwatch.Start();
            Stopwatch stopwatchAll = new Stopwatch();
            stopwatchAll.Start();
            Solve(new Solution<T>(invariables), 0);
            stopwatchAll.Stop();
            Console.WriteLine("Total time: " + stopwatchAll.ElapsedMilliseconds);
            Console.WriteLine("Nodes visited till first solution: " + nodesVisitedFirst);
            Console.WriteLine("Nodes visited total: " + nodesVisited);
            Console.WriteLine("Backtracking count till first solution: " + backtrackingFirst);
            Console.WriteLine("Backtracking count total: " + backtracking);
            Console.WriteLine("Number of solutions: " + solutions.Count);
            return solutions;
        }

        bool first = true;
        int nodesVisitedFirst = 0;
        int nodesVisited = 0;
        int backtrackingFirst = 0;
        int backtracking = 0;
        private void Solve(Solution<T> solution, int variableNum)
        {
            //Wybierz kolejną zmienną do przypisania
           for (; variableNum < variables.Count; variableNum++)
            {
                Variable<T> variable = variables[variableNum];
                nodesVisited++;
                if (invariables.ContainsKey(variable))
                {
                    continue;
                }
                //Wybierz kolejną wartość z dziedziny aktualnej zmiennej
                for (int d = 0; d < variable.domain.values.Count; d++)
                {
                    T domainVal = variable.domain.values[d];
                    if (solution.Check(variable, domainVal)) //wszystkie ograniczenia są spełnione
                    {
                        //Przypisz wybraną wartość do aktualnej zmiennej
                        solution.Assign(variable, domainVal);
                        Solve(solution.Clone(), variableNum + 1);
                    }
                    backtracking++;
                }
                //brak kolejnej wartości -> Wróć do poprzedniej zmiennej
                backtracking++;
                return;
            }
            //brak kolejnej zmiennej -> Znaleziono rozwiązanie
            solutions.Add(solution.Clone());
            if (first)
            {
                stopwatch.Stop();
                Console.WriteLine("First solution time: " + stopwatch.ElapsedMilliseconds);
                first = false;
                nodesVisitedFirst = nodesVisited;
                backtrackingFirst = backtracking;
            }
            return;

        }
    }
}