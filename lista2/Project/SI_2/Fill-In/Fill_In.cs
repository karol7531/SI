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
        public Fill_In(string puzzlePath, string wordsPath)
        {
            List<Domain<string>> domains = Reader.ReadDomains(wordsPath);
            List<TileVariable> horizonatalTileVariables;
            List<TileVariable> verticalTileVariables;
            Reader.ReadVariables(puzzlePath, domains, out horizonatalTileVariables, out verticalTileVariables);
            List<Variable<string>> variables = horizonatalTileVariables.Select(h => h.variable)
                .Union(verticalTileVariables.Select(v => v.variable)).OrderBy(v => v.id).ToList();
            List<Constraint<string>> constraints = CreateConstraints(horizonatalTileVariables, verticalTileVariables);


            problem = new Problem<string>(variables, domains, constraints, new Dictionary<Variable<string>, string>());
            problem.AssignConstraints();
        }

        public List<Solution<string>> Solve()
        {
            return problem.Solve();
        }

        private List<Constraint<string>> CreateConstraints(List<TileVariable> horizonatalTileVariables, List<TileVariable> verticalTileVariables)
        {
            List<Constraint<string>> constraints = new List<Constraint<string>>();
            foreach(var h in horizonatalTileVariables)
            {
                foreach(var v in verticalTileVariables)
                {
                    List<Constraint<string>> con = GetConstraint(h, v);
                    constraints.AddRange(con);
                }
            }
            return constraints;
        }

        private List<Constraint<string>> GetConstraint(TileVariable h, TileVariable v)
        {
            List<Constraint<string>> constraints = new List<Constraint<string>>();
            for (int i = 0; i < h.tiles.Count; i++)
            {
                for(int j = 0; j < v.tiles.Count; j++)
                {
                    if(h.tiles[i] == v.tiles[j])
                    {
                        constraints.Add(new Constraint<string>(h.variable, v.variable, NotEqualLetter(i, j)));
                        constraints.Add(new Constraint<string>(v.variable, h.variable, NotEqualLetter(j, i)));
                        return constraints;
                    }
                }
            }
            return constraints;
        }

        public static Func<Variable<string>, object, Solution<string>, string, bool> NotEqualLetter(int variableLetterPos, int objLetterPos)
        {
            return (v, o, s, g) => s.assignments.ContainsKey((Variable<string>)o) ? g[variableLetterPos] != s.assignments[(Variable<string>)o][objLetterPos] : true;
        }

        internal void PrintSolutions(List<Solution<string>> solutions)
        {
            foreach(Solution<string> s in solutions)
            {
                PrintSolution(s);
                Console.WriteLine();
            }
        }

        private void PrintSolution(Solution<string> s)
        {
            foreach(var a in s.assignments.Where(entry => entry.Key.desc == "H"))
            {
                Console.WriteLine(a.Value);
            }
        }
    }
}
