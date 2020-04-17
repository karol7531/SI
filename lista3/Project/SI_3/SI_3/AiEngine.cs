using System;
using System.Threading.Tasks;

namespace Connect_4
{
    public enum MethodType { MinMax, MinMaxBoosted, AlphaBeta}
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

        private int AlphaBeta(State state, bool player, int depth, ref int selectedCol, ref int alpha, ref int beta)
        {
            int colSelection = 0;
            alpha = int.MinValue;
            beta = int.MaxValue;
            int stateEval = state.Evaluation(player);
            if (depth == 0 || stateEval == State.pointsWin || stateEval == -State.pointsWin)
            {
                return stateEval;
            }

            int eval = player ? int.MinValue + 1 : int.MaxValue - 1;
            for (int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int refNum = 1;
                    int childAlpha = alpha;
                    int childBeta = beta;
                    int childEval = AlphaBeta(state.NextState(player, c), !player, depth - 1, ref refNum, ref childAlpha, ref childBeta);
                    if ((player && childEval > eval) || (!player && childEval < eval))
                    {
                        eval = childEval;
                        colSelection = c;
                    }
                    if (player) { alpha = Math.Max(childBeta, eval); }
                    else { beta = Math.Min(childAlpha, eval); }
                    if(beta <= alpha) {  break ;}
                }
            }
            if (depth == this.depth)
            {
                selectedCol = colSelection;
            }
            return eval;
        }

        public int GetMove(State state, bool player, MethodType methodType)
        {
            int selectedCol = 0;
            switch (methodType)
            {
                case MethodType.AlphaBeta:
                    {
                        int alpha = int.MinValue;
                        int beta = int.MaxValue;
                        AlphaBeta(state, player, this.depth, ref selectedCol, ref alpha, ref beta);
                        break;
                    }
                case MethodType.MinMax:
                    {
                        MinMax(state, player, this.depth, ref selectedCol);
                        break;
                    }
                case MethodType.MinMaxBoosted:
                    {
                        MinMaxBoosted(state, player, this.depth, ref selectedCol);
                        break;
                    }
            }
            return selectedCol;
        }
    }
}
