using CSP;
using System;
using System.Collections.Generic;

namespace Sudoku
{
    class Program
    {
        const int sudokuNum = 43;
        const string filePath = @"C:\Users\User\Desktop\Ja\PeWueR\sem_6\Sztuczna inteligencja\lista2\Project\ai-lab2-2020-dane\Sudoku.csv";
        static void Main(string[] args)
        {
            Console.WriteLine($"id sudoku: {sudokuNum}");
            Sudoku sudoku = new Sudoku(sudokuNum, filePath);
            List<Solution<byte>> solutions = sudoku.Solve();
            Sudoku.PrintSolutions(solutions);
            Console.ReadKey();
        }
    }
}
