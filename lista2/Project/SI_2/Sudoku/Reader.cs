using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CSP;

namespace Sudoku
{
    static class Reader
    {
        public static List<Variable<byte>> ReadSudoku(int sudokuNum, string filePath, List<Constraint<byte>> constraints, Dictionary<Variable<byte>, byte> invariables)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    //starts with number
                    if (Regex.IsMatch(line, @"^\d+"))
                    {
                        //id;difficulty;puzzle;solution
                        string[] splited = line.Split(';');
                        if (int.Parse(splited[0]) == sudokuNum)
                        {
                            return ReadVariables(splited[2], constraints, invariables);
                        }
                    }
                }
            }
            return null;
        }

        private static List<Variable<byte>> ReadVariables(string line, List<Constraint<byte>> constraints, Dictionary<Variable<byte>, byte> invariables)
        {
            Console.WriteLine(line);
            List<Variable<byte>> variables = new List<Variable<byte>>();
            for (int i = 0; i < Sudoku.sudokuDim * Sudoku.sudokuDim; i++)
            {
                string desc = GetDesc(i);
                Variable<byte> variable = new Variable<byte>(i, Sudoku.domain, desc);
                variables.Add(variable);

                if(line[i] != '.')
                {
                    invariables[variable] = byte.Parse(line[i].ToString());
                    // add constraint on number equal value
                    //constraints.Add(new Constraint<byte>(variable, byte.Parse(line[i].ToString()), Sudoku.EqualsNumber(byte.Parse(line[i].ToString()))));
                }
            }
            return variables;
        }

        private static string GetDesc(int i) => $"{i / 9 + 1}_{i % 9 + 1}";
    }
}
