using System.Threading.Tasks;

namespace SI_3
{
    class AiEngine
    {
        private int depth;

        public AiEngine(int depth)
        {
            this.depth = depth;
        }

        private int MinMax(State state, bool player, int depth, ref int selectedCol)
        {
            int colSelection = 0;
            int stateEval = state.Evaluation(player);
            if (depth == 0 || stateEval == State.pointsWin || stateEval == -State.pointsWin)
            {
                return stateEval;
            }

            int eval = player ? int.MinValue : int.MaxValue;
            for (int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int refNum = 1;
                    int childEval = MinMax(state.NextState(player, c), !player, depth - 1, ref refNum);
                    if ((player && childEval > eval) || (!player && childEval < eval))
                    {
                        eval = childEval;
                        colSelection = c;
                    }
                }
            }
            if (depth == this.depth)
            {
                selectedCol = colSelection;
            }
            return eval;
        }

        private int MinMaxBoosted(State state, bool player, int depth, ref int selectedCol)
        {
            int colSelection = 0;
            object accessLock = new object();
            int stateEval = state.Evaluation(player);
            if (depth == 0 || stateEval == State.pointsWin || stateEval == -State.pointsWin)
            {
                return stateEval;
            }

            int eval = player ? int.MinValue : int.MaxValue;
            Parallel.For(0, state.cols, (c) =>
            {
                if (state.CanPlace(c))
                {
                    int refNum = 1;
                    int childEval = MinMax(state.NextState(player, c), !player, depth - 1, ref refNum);
                    lock (accessLock)
                    {
                        if ((player && childEval > eval) || (!player && childEval < eval))
                        {
                            eval = childEval;
                            colSelection = c;
                        }
                    }
                }
            });
            selectedCol = colSelection;
            return eval;
        }

        public int GetMove(State state, bool player)
        {
            int selectedCol = 0;
            //MinMax(state, player, this.depth, ref selectedCol);
            MinMaxBoosted(state, player, this.depth, ref selectedCol);
            return selectedCol;
        }
    }
}
