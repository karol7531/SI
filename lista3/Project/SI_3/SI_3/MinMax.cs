using System;

namespace SI_3
{
    class MinMax
    {
        private int depth;

        public MinMax(int depth)
        {
            this.depth = depth;
        }

        private int FindMove(State state, bool player, int depth, ref int selectedCol)
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
                    int childEval = FindMove(state.NextState(player, c), !player, depth - 1, ref refNum);
                    if ((player && childEval > eval) || (!player && childEval < eval))
                    {
                        eval = childEval;
                        colSelection = c;
                        selectedCol = c;
                    }
                }
            }
            if (depth == this.depth)
            {
                selectedCol = colSelection;
            }
            return eval;
        }

        public int GetMove(State state, bool player) 
        {
            int selectedCol = 0;
            FindMove(state, player, this.depth, ref selectedCol);
            return selectedCol;
        }
    }
}
