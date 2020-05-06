using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Connect_4
{
    enum Gamemode { PlayerVsAi, AiVsAi }
    public class Program
    {
        const int runs = 10;
        const int depth = 6,
            rows = 6,
            cols = 7;
        const MethodType methodType1 = MethodType.AlphaBeta;
        const MethodType methodType2 = MethodType.MinMax;
        const HeuristicType heuristicType = HeuristicType.CornersRaw;
        Gamemode gamemode = Gamemode.PlayerVsAi;

        long ai1Time, ai2Time;
        int ai1Moves, ai2Moves;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.RunProgram();
        }


        private void RunProgram()
        {
            AiEngine minMax = new AiEngine(depth);
            ResetParams();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Random random = new Random();

            if (gamemode == Gamemode.PlayerVsAi)
            {
                PlayerVsAi(minMax);
            }
            else if (gamemode == Gamemode.AiVsAi)
            {
                for (int i = 0; i < runs; i++)
                {
                    AiVsAi(minMax, random.Next(7) + 1);
                }
            }
            PrintResults();
            ResetParams();

            stopwatch.Stop();
            Console.WriteLine($"Game time: {stopwatch.ElapsedMilliseconds}");
            Console.ReadKey();
        }

        private void PrintResults()
        {
            Console.WriteLine("Ai_1 Moves:\t" + ai1Moves);
            Console.WriteLine("Ai_2 Moves:\t" + ai2Moves);
            Console.WriteLine("Ai_1 Time:\t" + ai1Time);
            Console.WriteLine("Ai_2 Time:\t" + ai2Time);
            Console.WriteLine("Ai_1 AvgTime:\t" + ai1Time / ai1Moves);
            Console.WriteLine("Ai_2 AvgTime:\t" + ai2Time / ai2Moves);
        }

        private void ResetParams()
        {
            ai1Time = 0;
            ai2Time = 0;
            ai1Moves = 0;
            ai2Moves = 0;
        }

        private static void PlayerVsAi(AiEngine aiEngine)
        {
            Stack<State> stack = new Stack<State>();
            State state = new State(rows, cols);
            while (state.CanPlace())
            {
                stack.Push(state.Clone());
                Console.WriteLine("\nYour move:");
                string line = Console.ReadLine();
                if (line == "u")
                {
                    stack.Pop(); stack.Pop();
                    state = stack.Pop();
                    Console.WriteLine(state);
                    continue;
                }
                int playerMove = int.Parse(line);
                state = state.NextState(false, playerMove - 1);
                Console.WriteLine(state);
                if (state.Points(false, heuristicType) >= State.pointsWin)
                {
                    Console.WriteLine("Congratulations, you won");
                    return;
                }

                stack.Push(state.Clone());
                Console.WriteLine("\nAI move:");
                state = AiMove(aiEngine, state, true, methodType1);
                Console.WriteLine(state);
                if (state.Points(true, heuristicType) >= State.pointsWin)
                {
                    Console.WriteLine("AI won");
                    return;
                }
            }
        }

        private bool? AiVsAi(AiEngine aiEngine, int start)
        {
            State state = new State(rows, cols);
            Console.WriteLine("\nAI_1 move:");
            state = state.NextState(false, start - 1);
            Console.WriteLine(state);
            while (state.CanPlace())
            {
                Console.WriteLine("\nAI_2 move:");
                ai1Time += RunWithStopwatch(() =>
                {
                    state = AiMove(aiEngine, state, true, methodType1);
                });
                ai1Moves++;
                Console.WriteLine(state);
                if (state.Points(true, heuristicType) >= State.pointsWin)
                {
                    Console.WriteLine("AI_2 won");
                    return true;
                }

                Console.WriteLine("\nAI_1 move:");
                ai2Time += RunWithStopwatch(() => 
                { 
                    state = AiMove(aiEngine, state, false, methodType2); 
                });
                ai2Moves ++;
                Console.WriteLine(state);
                if (state.Points(false, heuristicType) >= State.pointsWin)
                {
                    Console.WriteLine("AI_1 won");
                    return false;
                }
            }
            return null;
        }

        public static State AiMove(AiEngine aiEngine, State state, bool player, MethodType methodType)
        {
            int aiMove = aiEngine.GetMove(state, player, methodType, heuristicType);
            return state.NextState(player, aiMove);
        }

        private static long RunWithStopwatch(Action action)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
