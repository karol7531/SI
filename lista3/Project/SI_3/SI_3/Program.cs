using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Connect_4
{
    public class Program
    {
        const int depth = 7,
            rows = 6,
            cols = 7;
        const MethodType methodType = MethodType.AlphaBeta;

        static void Main(string[] args)
        {
            AiEngine minMax = new AiEngine(depth);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            PlayerVsAi(minMax);
            //AiVsAi(minMax, 7);

            stopwatch.Stop();
            Console.WriteLine($"Game time: {stopwatch.ElapsedMilliseconds}");
            Console.ReadKey();
        }

        private static void PlayerVsAi(AiEngine minMax)
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
                state = AiMove(minMax, state, true, methodType);
                Console.WriteLine(state);
                if (state.Points(true) >= State.pointsWin)
                {
                    Console.WriteLine("AI won");
                    return;
                }
            }
        }

        private static bool? AiVsAi(AiEngine minMax, int start)
        {
            State state = new State(rows, cols);
            Console.WriteLine("\nAI_1 move:");
            state = state.NextState(false, start - 1);
            Console.WriteLine(state);
            while (state.CanPlace())
            {
                Console.WriteLine("\nAI_2 move:");
                state = AiMove(minMax, state, true, methodType);
                Console.WriteLine(state);
                if (state.Points(true) >= State.pointsWin)
                {
                    Console.WriteLine("AI_2 won");
                    return true;
                }

                Console.WriteLine("\nAI_1 move:");
                state = AiMove(minMax, state, false, methodType);
                Console.WriteLine(state);
                if (state.Points(false) >= State.pointsWin)
                {
                    Console.WriteLine("AI_1 won");
                    return false;
                }
            }
            return null;
        }

        public static State AiMove(AiEngine minMax, State state, bool player, MethodType methodType)
        {
            int aiMove = minMax.GetMove(state, player, methodType);
            return state.NextState(player, aiMove);
        }
    }
}
