using CSP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    class Sudoku
    {
        public const int sudokuDim = 9;
        private static readonly byte[] domainValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static readonly List<int> squareOrder = new List<int>() {
            0,1,2, 9,10,11, 18,19,20, 3,4,5, 12,13,14, 21,22,23, 6,7,8, 15,16,17, 24,25,26,
            27,28,29, 36,37,38, 45,46,47, 30,31,32, 39,40,41, 48,49,50, 33,34,35, 42,43,44, 51,52,53,
            54,55,56, 63,64,65, 72,73,74, 57,58,59, 66,67,68, 75,76,77, 60,61,62, 69,70,71, 78,79,80};
        public static readonly Domain<byte> domain = new Domain<byte>(domainValues.ToList());

        private Problem<byte> problem;

        public Sudoku(int sudokuNumber, string filePath, SearchType searchType, bool doSquareOrder = false)
        {
            List<Domain<byte>> domains = new List<Domain<byte>>() { domain };
            List<Constraint<byte>> constraints = new List<Constraint<byte>>();
            Dictionary<Variable<byte>, byte> invariables = new Dictionary<Variable<byte>, byte>();
            List<Variable<byte>> variables = Reader.ReadSudoku(sudokuNumber, filePath, constraints, invariables);
            AddConstraints(constraints, variables);

            problem = doSquareOrder 
                ? new Problem<byte>(variables, domains, constraints, invariables, searchType, squareOrder)
                : new Problem<byte>(variables, domains, constraints, invariables, searchType);
            problem.AssignConstraints();
        }

        public List<Solution<byte>> Solve(VariableHeuristicType variableHeuristicType, ValueHeuristicType valueHeuristicType)
        {
            return problem.Solve(variableHeuristicType, valueHeuristicType);
        }

        /// <summary>
        /// Adds sudoku constraints if variable does'nt have any already.
        /// </summary>
        /// <param name="constraints"></param>
        /// <param name="variables"></param>
        private void AddConstraints(List<Constraint<byte>> constraints, List<Variable<byte>> variables)
        {
            List<Variable<byte>> constrained = constraints.Select(c => c.variable).ToList();

            foreach(var v in variables)
            {
                if (!constrained.Contains(v))
                {
                    constraints.AddRange(CreateConstraints(variables, v));
                }
            }
        }

        private List<Constraint<byte>> CreateConstraints(List<Variable<byte>> variables, Variable<byte> variable)
        {
            List<Constraint<byte>> constraints = new List<Constraint<byte>>();
            int rowNum = int.Parse(variable.desc[0].ToString()) - 1;
            int colNum = int.Parse(variable.desc[2].ToString()) - 1;
            HashSet<int> indexes = RowIndexes(rowNum, colNum);
            indexes.UnionWith(ColIndexes(rowNum, colNum));
            indexes.UnionWith(BoxIndexes(rowNum, colNum));
            foreach(int i in indexes)
            {
                constraints.Add(new Constraint<byte>(variable, variables.ElementAt(i), NotEqualVariable));
            }
            return constraints;
        }

        private HashSet<int> RowIndexes(int rowNum, int colNum)
        {
            HashSet<int> rowIndexes = new HashSet<int>(8);
            for (int i = 0; i < sudokuDim; i++)
            {
                if(i != colNum)
                {
                    rowIndexes.Add(rowNum * sudokuDim + i);
                }
            }
            return rowIndexes;
        }

        private HashSet<int> ColIndexes(int rowNum, int colNum)
        {
            HashSet<int> colIndexes = new HashSet<int>(8);
            for (int i = 0; i < sudokuDim; i++)
            {
                if (i * sudokuDim != rowNum)
                {
                    colIndexes.Add(i * sudokuDim + colNum);
                }
            }
            return colIndexes;
        }

        private HashSet<int> BoxIndexes(int rowNum, int colNum)
        {
            HashSet<int> boxIndexes = new HashSet<int>(8);
            int boxRow = rowNum / 3;
            int boxCol = colNum / 3;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    int row = boxRow * 3 + i;
                    int col = boxCol * 3 + j;
                    if (rowNum != row || colNum != col)
                    {
                        boxIndexes.Add(row * sudokuDim + col);
                    }
                }
            }
            return boxIndexes;
        }


        /// <summary>
        /// Predicate function for given initial values
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if value is the same as initial, else false</returns>
        public static Func< object, Solution<byte>, byte, bool> EqualsNumber(byte value)
        {
           return ( o, s, g) => g == value; 
        }

        /// <summary>
        /// Predicate function. Value cannot be the same as value in the other Variable.
        /// </summary>
        /// <returns>true if value not the same as in other variable, else false</returns>
        public static Func< object, Solution<byte>, byte, bool> NotEqualVariable =
            ( o, s, g) => s.assignments.ContainsKey((Variable<byte>)o) ? s.assignments[(Variable<byte>)o] != g : true;

        public static void PrintSolutions(List<Solution<byte>> solutions)
        {
            foreach(var s in solutions)
            {
                PrintSolution(s);
            }
        }

        private static void PrintSolution(Solution<byte> solution)
        {
            List<byte> answer = solution.assignments.OrderBy(entry => entry.Key.id).Select(entry => entry.Value).ToList();
            foreach(byte a in answer)
            {
                Console.Write(a);
            }
            Console.WriteLine();
        }
    }
}
