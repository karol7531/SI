using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSP
{
    class Program{static void Main(string[] args){}}
    public enum SearchType { Backtracking, ForwardChecking} 
    public enum VariableHeuristicType { DefinitionOrder, SmallestDomain, Random}
    public enum ValueHeuristicType { DefinitionOrder, Random }

    public class Problem<T>
    {
        private SearchType searchType;
        public List<Variable<T>> variables { get; private set; }
        public List<Domain<T>> domains { get; private set; }
        public List<Constraint<T>> constraints { get; private set; }
        private List<Solution<T>> solutions = new List<Solution<T>>();
        private Dictionary<Variable<T>, T> invariables;

        public Problem(List<Variable<T>> variables, List<Domain<T>> domains, List<Constraint<T>> constraints, Dictionary<Variable<T>, T> invariables, SearchType searchType)
        {
            this.variables = variables;
            this.domains = domains;
            this.constraints = constraints;
            this.invariables = invariables;
            this.searchType = searchType;
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
        public List<Solution<T>> Solve(VariableHeuristicType variableHeuristicType, ValueHeuristicType valueHeuristicType)
        {
            ResetValues();
            stopwatch.Start();
            Stopwatch stopwatchAll = new Stopwatch();
            stopwatchAll.Start();
            switch (searchType)
            {
                case SearchType.ForwardChecking:
                {
                    CloneDomains();
                    Solution<T> solution = new Solution<T>(invariables, variables);
                    //Wykorzystując ograniczenia odfiltruj dziedziny zmiennych bez wartości
                    solution.FilterOutDomains();
                    solution.ApplyValueHeuristic(valueHeuristicType);
                    solution.ApplyVariableHeuristic(variableHeuristicType);
                    ForwardChecking(solution, 0);
                    break;
                }
                case SearchType.Backtracking:
                {
                    Solution<T> solution = new Solution<T>(invariables, variables); 
                    solution.ApplyValueHeuristic(valueHeuristicType);
                    solution.ApplyVariableHeuristic(variableHeuristicType);
                    Backtracking(solution, 0);
                    break;
                }
            }
            stopwatchAll.Stop();
            Console.WriteLine("Total time: " + stopwatchAll.ElapsedMilliseconds);
            Console.WriteLine("Nodes visited till first solution: " + nodesVisitedFirst);
            Console.WriteLine("Nodes visited total: " + nodesVisited);
            Console.WriteLine("Backtracking count till first solution: " + backtrackingFirst);
            Console.WriteLine("Backtracking count total: " + backtracking);
            Console.WriteLine("Number of solutions: " + solutions.Count);
            return solutions;
        }

        private void ResetValues()
        {
            first = true;
            nodesVisitedFirst = 0;
            nodesVisited = 0;
            backtrackingFirst = 0;
            backtracking = 0;
            solutions.Clear();
            stopwatch.Restart();
        }

        private void CloneDomains()
        {
            foreach(var v in variables)
            {
                v.SetDomain(v.domain.Clone());
            }
        }

        private void ForwardChecking(Solution<T> solution, int variableNum)
        {
            //Wybierz kolejną zmienną do przypisania
            for (; variableNum < solution.variables.Count; variableNum++)
            {
                Variable<T> variable = solution.variables[variableNum];
                nodesVisited++;
                if (invariables.ContainsKey(variable))
                {
                    continue;
                }
                //Wybierz kolejną wartość z dziedziny aktualnej zmiennej
                for (int d = 0; d < variable.domain.values.Count; d++)
                {
                    T domainVal = variable.domain.values[d];
                    if(solution.DomainsNotEmpty()) // każda dziedzina nie jest pusta
                    {
                        var solutionCloned = solution.Clone(true);
                        //Przypisz wybraną wartość do aktualnej zmiennej
                        solutionCloned.Assign(variable, domainVal);
                        //Wykorzystując ograniczenia odfiltruj dziedziny zmiennych bez wartości
                        solutionCloned.FilterOutDomains(variable);
                        //LogSolution(solutionCloned);

                        ForwardChecking(solutionCloned, variableNum + 1);
                    }
                    backtracking++;
                }
                //brak kolejnej wartości -> Wróć do poprzedniej zmiennej
                backtracking++;
                return;
            }
            //brak kolejnej zmiennej -> Znaleziono rozwiązanie
            solutions.Add(solution.Clone(true));
            if (first)
            {
                stopwatch.Stop();
                Console.WriteLine("First solution time: " + stopwatch.ElapsedMilliseconds);
                first = false;
                nodesVisitedFirst = nodesVisited;
                backtrackingFirst = backtracking;
            }
        }

        private void LogSolution(Solution<T> solution)
        {
            foreach (KeyValuePair<Variable<T>, T> entry in solution.assignments)
            {
                Console.Write(entry.Key + ": " + entry.Value + "\t");
            }
            Console.WriteLine();
            foreach (var u in solution.unassignedVariables)
            {
                Console.Write(u + "" + u.domain + " ");
            }
            Console.WriteLine("\n");
        }

        bool first = true;
        int nodesVisitedFirst = 0;
        int nodesVisited = 0;
        int backtrackingFirst = 0;
        int backtracking = 0;
        private void Backtracking(Solution<T> solution, int variableNum)
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
                        Backtracking(solution.Clone(), variableNum + 1);
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
        }
    }
}