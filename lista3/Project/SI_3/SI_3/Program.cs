using System;

namespace SI_3
{
    class Program
    {
        const int depth = 4, 
            rows = 6, 
            cols = 7;
        static void Main(string[] args)
        {
            MinMax minMax = new MinMax(depth);
            //PlayerVsAi(minMax);
            AiVsAi(minMax);
            Console.ReadKey();
        }

        private static void PlayerVsAi(MinMax minMax)
        {
            State state = new State(rows, cols);
            while (true)
            {
                Console.WriteLine("\nYour move:");
                UserMove(minMax, ref state, true);
                if (state.DidWin(true))
                {
                    Console.WriteLine("Congratulations, you won");
                    return;
                }

                Console.WriteLine("\nAI move:");
                AiMove(minMax, ref state, false);
                if (state.DidWin(false))
                {
                    Console.WriteLine("AI won");
                    return;
                }
            }
        }

        private static void AiVsAi(MinMax minMax)
        {
            State state = new State(rows, cols);
            while (true)
            {
                Console.WriteLine("\nAI_1 move:");
                AiMove(minMax, ref state, false);
                if (state.DidWin(false))
                {
                    Console.WriteLine("AI_1 won");
                    return;
                }

                Console.WriteLine("\nAI_2 move:");
                AiMove(minMax, ref state, true); 
                if (state.DidWin(true))
                {
                    Console.WriteLine("AI_2 won");
                    return;
                }
            }
        }

        private static void AiMove(MinMax minMax, ref State state, bool player)
        {
            int aiMove = minMax.GetMove(state, player);
            state = state.NextState(player, aiMove);
            state.PrintState();
        }

        private static void UserMove(MinMax minMax, ref State state, bool player)
        {
            int playerMove = int.Parse(Console.ReadLine());
            state = state.NextState(player, playerMove - 1);
            state.PrintState(); 
        }
    }
}
