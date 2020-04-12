using System;

namespace SI_3
{
    class Program
    {
        const int depth = 2, 
            rows = 6, 
            cols = 7;
        static void Main(string[] args)
        {
            MinMax minMax = new MinMax(depth);
            PlayerVsAI(minMax);
            Console.ReadKey();
        }

        private static void PlayerVsAI(MinMax minMax)
        {
            State state = new State(rows, cols);
            while (true)
            {
                try
                {
                    Console.WriteLine("\nYour move:");
                    int playerMove = int.Parse(Console.ReadLine());
                    state = state.NextState(false, playerMove - 1);
                    state.PrintState();
                    if (state.DidWin(false))
                    {
                        Console.WriteLine("Congratulations, you win");
                        return;
                    }

                    Console.WriteLine("\nAI move:");
                    int AImove = minMax.Move(state);
                    state = state.NextState(true, AImove);
                    state.PrintState();
                    if (state.DidWin(true))
                    {
                        Console.WriteLine("AI won");
                        return;
                    }
                }
                catch (FormatException fe)
                {
                    Console.Error.WriteLine("Invalid format \n" + fe.Message + "\n" + fe.StackTrace + "\n");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Unexpected exception\n" + e.Message + "\n" + e.ToString());
                }
            }
        }
    }
}
