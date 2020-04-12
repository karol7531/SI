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

        public int Move(State state) 
        {
            int selectedCol = 0;
            Max(state, this.depth, ref selectedCol);
            return selectedCol;
        }

        private int Max(State state, int depth, ref int selectedCol) 
        {
            int colSelection = 0;
            if(depth == 0)
            {
                return state.Evaluation();
            }
            int eval = -10;

            for(int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int childEval = Min(state.NextState(true, c), depth - 1);
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

        private int Min(State state, int depth) 
        {
            if (depth == 0)
            {
                return state.Evaluation();
            }
            int eval = 10;

            for (int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int refNum = 1;
                    int childEval = Max(state.NextState(false, c), depth - 1, ref refNum);
                    eval = Math.Min(eval, childEval);
                }
            }
            return eval;
        }
    }
}
