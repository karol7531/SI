using CSP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sudoku
{
    class Program
    {
        const int sudokuNum = 43;
        const int runs = 10;
        const ValueHeuristicType valueHeuristicType = ValueHeuristicType.Random;
        const VariableHeuristicType variableHeuristicType = VariableHeuristicType.Random;
        private const SearchType searchType = SearchType.ForwardChecking;
        const string filePath = @"C:\Users\User\Desktop\Ja\PeWueR\sem_6\Sztuczna inteligencja\lista2\Project\ai-lab2-2020-dane\Sudoku.csv";
        static void Main(string[] args)
        {
            Console.WriteLine($"id sudoku: {sudokuNum}");
            Parallel.For(0, runs, (i) => {
                Sudoku sudoku = new Sudoku(sudokuNum, filePath, searchType);
                List<Solution<byte>> solutions = sudoku.Solve(variableHeuristicType, valueHeuristicType);
                if (i == 0)
                {
                    //Sudoku.PrintSolutions(solutions);
                }
                Console.WriteLine();
            });
            //for (int i =0; i < runs; i++)
            //{
            //    Sudoku sudoku = new Sudoku(sudokuNum, filePath, searchType);
            //    List<Solution<byte>> solutions = sudoku.Solve(variableHeuristicType, valueHeuristicType);
            //    if (i == 0)
            //    {
            //        //Sudoku.PrintSolutions(solutions);
            //    }
            //    Console.WriteLine();
            //}
            Console.ReadKey();
        }
    }
}
