using CSP;
using System;
using System.Collections.Generic;

namespace Fill_In
{
    class Program
    {
        const int fillInNum = 4;
        const int runs = 1;
        const SearchType searchType = SearchType.ForwardChecking;
        const ValueHeuristicType valueHeuristicType= ValueHeuristicType.DefinitionOrder;
        const VariableHeuristicType variableHeuristicType= VariableHeuristicType.ShortestWord;
        const string folderPath = @"C:\Users\User\Desktop\Ja\PeWueR\sem_6\Sztuczna inteligencja\lista2\Project\ai-lab2-2020-dane\Jolka\";
        static string puzzlePath = folderPath + "puzzle" + fillInNum;
        static string wordsPath = folderPath + "words" + fillInNum;

        static void Main(string[] args)
        {
            Console.WriteLine($"id fill_in: {fillInNum}");
            for (int i = 0; i < runs; i++)
            {
                int width;
                Fill_In fill_In = new Fill_In(puzzlePath, wordsPath, out width, searchType);
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
