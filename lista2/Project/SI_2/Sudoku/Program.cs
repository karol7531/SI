using CSP;
using System;
using System.Collections.Generic;

namespace Sudoku
{
    class Program
    {
        const int sudokuNum = 46;
        const int runs = 10;
        const ValueHeuristicType valueHeuristicType = ValueHeuristicType.DefinitionOrder;
        const VariableHeuristicType variableHeuristicType = VariableHeuristicType.DefinitionOrder;
        private const SearchType searchType = SearchType.ForwardChecking;
        const string filePath = @"C:\Users\User\Desktop\Ja\PeWueR\sem_6\Sztuczna inteligencja\lista2\Project\ai-lab2-2020-dane\Sudoku.csv";
        static void Main(string[] args)
        {
            Console.WriteLine($"id sudoku: {sudokuNum}");
            for (int i = 0; i < runs; i++)
            {
                Sudoku sudoku = new Sudoku(sudokuNum, filePath, searchType, true);
                List<Solution<byte>> solutions = sudoku.Solve(variableHeuristicType, valueHeuristicType);
                if (i == 0)
                {
                    Sudoku.PrintSolutions(solutions);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
