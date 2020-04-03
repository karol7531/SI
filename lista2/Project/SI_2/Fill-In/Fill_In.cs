using CSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fill_In
{
    class Fill_In
    {
        private Problem<string> problem;
        public Fill_In(string puzzlePath, string wordsPath, out int width, SearchType searchType)
        {
            List<Domain<string>> domains = Reader.ReadDomains(wordsPath);
            List<TileVariable> horizonatalTileVariables;
            List<TileVariable> verticalTileVariables;
            Reader.ReadVariables(puzzlePath, domains, out horizonatalTileVariables, out verticalTileVariables, out width);
            List<Variable<string>> variables = horizonatalTileVariables.Select(h => h.variable)
                .Union(verticalTileVariables.Select(v => v.variable)).ToList();
            List<Constraint<string>> constraints = CreateWordConstraints(variables, domains);
            constraints = constraints.Union(CreateLetterConstraints(horizonatalTileVariables, verticalTileVariables)).ToList();


            problem = new Problem<string>(variables, domains, constraints, new Dictionary<Variable<string>, string>(), searchType);
            problem.AssignConstraints();
        }

        public List<Solution<string>> Solve(VariableHeuristicType variableHeuristicType, ValueHeuristicType valueHeuristicType)
        {
            return problem.Solve(variableHeuristicType, valueHeuristicType);
        }

        private List<Constraint<string>> CreateWordConstraints(List<Variable<string>> variables, List<Domain<string>> domains)
        {
            List<Constraint<string>> constraints = new List<Constraint<string>>();
            foreach (var d in domains)
            {
                var variablesWithDomain = variables.Where((v) => v.domain == d);
                foreach(var v in variablesWithDomain)
                {
                    foreach(var o in variablesWithDomain)
                    {
                        if(v!= o)
                        {
                            constraints.Add(new Constraint<string>(v, o, NotEqualWords));
                        }
                    }
                }
            }
            return constraints;
        }

        private List<Constraint<string>> CreateLetterConstraints(List<TileVariable> horizonatalTileVariables, List<TileVariable> verticalTileVariables)
        {
            List<Constraint<string>> constraints = new List<Constraint<string>>();
            foreach (var h in horizonatalTileVariables)
            {
                foreach (var v in verticalTileVariables)
                {
                    List<Constraint<string>> con = GetLetterConstraint(h, v);
                    constraints.AddRange(con);
                }
            }
            return constraints;
        }

        private List<Constraint<string>> GetLetterConstraint(TileVariable h, TileVariable v)
        {
            List<Constraint<string>> constraints = new List<Constraint<string>>();
            for (int i = 0; i < h.tiles.Count; i++)
            {
                for(int j = 0; j < v.tiles.Count; j++)
                {
                    if(h.tiles[i] == v.tiles[j])
                    {
                        constraints.Add(new Constraint<string>(h.variable, v.variable, EqualLetter(i, j)));
                        constraints.Add(new Constraint<string>(v.variable, h.variable, EqualLetter(j, i)));
                        return constraints;
                    }
                }
            }
            return constraints;
        }

        /// <summary>
        /// Cross letter on the same position in horizontal and vertical word must be the same
        /// </summary>
        /// <param name="variableLetterPos"></param>
        /// <param name="objLetterPos"></param>
        /// <returns></returns>
        public static Func< object, Solution<string>, string, bool> EqualLetter(int variableLetterPos, int objLetterPos)
        {
            return ( o, s, g) => s.assignments.ContainsKey((Variable<string>)o) ? g[variableLetterPos] == s.assignments[(Variable<string>)o][objLetterPos] : true;
        }

        /// <summary>
        /// Word assigned to variable != word of already assigned variable (in the smae domain)
        /// </summary>
        public static Func< object, Solution<string>, string, bool> NotEqualWords =
         ( o, s, g) => s.assignments.ContainsKey((Variable<string>)o) ? g != s.assignments[(Variable<string>)o] : true;
        


        internal void PrintSolutions(List<Solution<string>> solutions, int width)
        {
            foreach(Solution<string> s in solutions)
            {
                Console.WriteLine();
                PrintSolution(s, width);
                Console.WriteLine();
            }
        }

        private void PrintSolution(Solution<string> s, int width)
        {
            int currRow = 1;
            List<Variable<string>> keysSorted = s.assignments.Where(entry => entry.Key.desc == "H").Select(entry => entry.Key).OrderBy(el => int.Parse(el.id.ToString())).ToList();
            for (int i = 0; i < keysSorted.Count; i++)
            {
                int position = int.Parse(keysSorted[i].id.ToString());
                if (i == 0)
                {
                    Console.Write(new string(' ', position));
                }
                if (int.Parse(keysSorted[i].id.ToString()) < currRow * width)
                {
                    Console.Write(s.assignments[keysSorted[i]] + " ");
                }
                else
                {
                    Console.Write("\n" + new string(' ', position - currRow * width) + s.assignments[keysSorted[i]] + " ");
                    currRow++;
                }
            }
        }
    }
}
