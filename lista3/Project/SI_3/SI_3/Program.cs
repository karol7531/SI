using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SI_3
{
    class Program
    {
        const int depth = 6, 
            rows = 6, 
            cols = 7;
        static void Main(string[] args)
        {
            MinMax minMax = new MinMax(depth);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //PlayerVsAi(minMax);
            AiVsAi(minMax, 7);

            stopwatch.Stop();
            Console.WriteLine($"Game time: {stopwatch.ElapsedMilliseconds}");
            Console.ReadKey();
        }

        private static void PlayerVsAi(MinMax minMax)
        {
            Stack<State> stack = new Stack<State>();
            State state = new State(rows, cols);
            while (state.CanPlace())
            {
                stack.Push(state.Clone());
                Console.WriteLine("\nYour move:");
                string line = Console.ReadLine();
                if(line == "u")
                {
                    stack.Pop(); stack.Pop();
                    state = stack.Pop();
                    Console.WriteLine(state);
                    continue;
                }
                int playerMove = int.Parse(line);
                state = state.NextState(false, playerMove - 1);
                Console.WriteLine(state);
                if (state.Points(false) >= State.pointsWin)
                {
                    Console.WriteLine("Congratulations, you won");
                    return;
                }

                stack.Push(state.Clone());
                Console.WriteLine("\nAI move:");
                AiMove(minMax, ref state, true);
                if (state.Points(true) >= State.pointsWin)
                {
                    Console.WriteLine("AI won");
                    return;
                }
            }
        }

        private static void AiVsAi(MinMax minMax, int start)
        {
            State state = new State(rows, cols);
            Console.WriteLine("\nAI_1 move:");
            state = state.NextState(false, start - 1);
            Console.WriteLine(state);
            while (state.CanPlace())
            {
                Console.WriteLine("\nAI_2 move:");
                AiMove(minMax, ref state, true);
                if (state.Points(true) >= State.pointsWin)
                {
                    Console.WriteLine("AI_2 won");
                    return;
                }

                Console.WriteLine("\nAI_1 move:");
                AiMove(minMax, ref state, false);
                if (state.Points(false) >= State.pointsWin)
                {
                    Console.WriteLine("AI_1 won");
                    return;
                }
            }
        }

        private static void AiMove(MinMax minMax, ref State state, bool player)
        {
            int aiMove = minMax.GetMove(state, player);
            state = state.NextState(player, aiMove);
            Console.WriteLine(state);
        }
    }
}
