using CSP;
using System;
using System.Collections.Generic;

namespace Fill_In
{
    class Program
    {
        const int fillInNum = 1;
        const int runs = 10;
        const SearchType searchType = SearchType.ForwardChecking;
        const ValueHeuristicType valueHeuristicType= ValueHeuristicType.Random;
        const VariableHeuristicType variableHeuristicType= VariableHeuristicType.SmallestDomain;
        const string folderPath = @"C:\Users\User\Desktop\Ja\PeWueR\sem_6\Sztuczna inteligencja\lista2\Project\ai-lab2-2020-dane\Jolka\";
        static string puzzlePath = folderPath + "puzzle" + fillInNum;
        static string wordsPath = folderPath + "words" + fillInNum;

        static void Main(string[] args)
        {
            Console.WriteLine($"id fill_in: {fillInNum}");
            int width;
            Fill_In fill_In = new Fill_In(puzzlePath, wordsPath, out width, searchType);
            for (int i = 0; i < runs; i++)
            {
                List<Solution<string>> solutions = fill_In.Solve(variableHeuristicType, valueHeuristicType);
                if (i == 0)
                {
                    fill_In.PrintSolutions(solutions, width);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
