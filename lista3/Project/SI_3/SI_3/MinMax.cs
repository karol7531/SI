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
            int stateEval = state.Evaluation();
            if (depth == 0 || stateEval != 0)
            {
                return stateEval;
            }

            int eval = player ? -10 : 10;
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

        private int Max(State state, bool player, int depth, ref int selectedCol) 
        {
            int colSelection = 0;
            int stateEval = state.Evaluation();
            if (depth == 0 || stateEval != 0)
            {
                return stateEval;
            }
            int eval = -10;

            for(int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int childEval = Min(state.NextState(player, c), player, depth - 1);
                    if(childEval > eval)
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

        private int Min(State state, bool player, int depth) 
        {
            int stateEval = state.Evaluation();
            if (depth == 0 || stateEval != 0)
            {
                return stateEval;
            }
            int eval = 10;

            for (int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int refNum = 1;
                    int childEval = Max(state.NextState(!player, c), player, depth - 1, ref refNum);
                    eval = Math.Min(eval, childEval);
                }
            }
            return eval;
        }
    }
}
