using System;

namespace Connect_4
{
    public enum MethodType { MinMax, AlphaBeta}

    public class AiEngine
    {
        private int depth;

        public AiEngine(int depth)
        {
            this.depth = depth;
        }

        private int MinMax(State state, bool player, int depth, ref int selectedCol, HeuristicType heuristicType, string path)
        {
            int colSelection = 0;
            int stateEval = state.Evaluation(player, heuristicType);
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
                    int childEval = MinMax(state.NextState(player, c), !player, depth - 1, ref refNum, heuristicType, path + $" {c}");
                    //Console.WriteLine($"depth: {depth}\tchildEval: {childEval}\tpath: {path + $" {c}"}");
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

        private int AlphaBeta2(State state, bool player, int depth, ref int selectedCol, int alpha, int beta, HeuristicType heuristicType, string path)
        {
            int colSelection = 0;
            int stateEval = state.Evaluation(player, heuristicType);
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
                    int childEval = AlphaBeta2(state.NextState(player, c), !player, depth - 1, ref refNum, alpha, beta, heuristicType, path + $" {c}");
                    //Console.WriteLine($"depth: {depth}\tchildEval: {childEval}\tpath: {path + $" {c}"}");
                    if ((player && childEval > eval) || (!player && childEval < eval))
                    {
                        eval = childEval;
                        colSelection = c;
                    }
                    if (player) alpha = Math.Max(alpha, childEval);
                    else beta = Math.Min(beta, childEval);
                    if (beta <= alpha) break;
                }
            }
            if (depth == this.depth)
            {
                selectedCol = colSelection;
            }
            return eval;
        }

        private int AlphaBeta(State state, bool player, int depth, ref int selectedCol, ref int alpha, ref int beta, HeuristicType heuristicType, string path)
        {
            int colSelection = 6;
            int stateEval = state.Evaluation(player, heuristicType);
            if (depth == 0 || stateEval == State.pointsWin || stateEval == -State.pointsWin)
            {
                return stateEval;
            }
            alpha = int.MinValue;
            beta = int.MaxValue;

            int eval = player ? int.MinValue + 1 : int.MaxValue - 1;
            for (int c = 0; c < state.cols; c++)
            {
                if (state.CanPlace(c))
                {
                    int refNum = 1;
                    int childAlpha = alpha;
                    int childBeta = beta;
                    int childEval = AlphaBeta(state.NextState(player, c), !player, depth - 1, ref refNum, ref childAlpha, ref childBeta, heuristicType, path + $" {c}");
                    Console.WriteLine($"depth: {depth}\tchildEval: {childEval}\tpath: {path + $" {c}"}");
                    if ((player && childEval > eval) || (!player && childEval < eval))
                    {
                        eval = childEval;
                        colSelection = c;
                    }
                    if (player) { alpha = Math.Max(childAlpha, eval); }
                    else { beta = Math.Min(childBeta, eval); }
                    if(beta <= alpha) {  break ;}
                }
            }
            if (depth == this.depth)
            {
                selectedCol = colSelection;
            }
            return eval;
        }

        public int GetMove(State state, bool player, MethodType methodType, HeuristicType heuristicType)
        {
            int selectedCol = 0;
            switch (methodType)
            {
                case MethodType.AlphaBeta:
                    {
                        int alpha = int.MinValue;
                        int beta = int.MaxValue;
                        //AlphaBeta(state, player, this.depth, ref selectedCol, ref alpha, ref beta, heuristicType, "");
                        AlphaBeta2(state, player, this.depth, ref selectedCol, alpha, beta, heuristicType, "");
                        break;
                    }
                case MethodType.MinMax:
                    {
                        MinMax(state, player, this.depth, ref selectedCol, heuristicType, "");
                        break;
                    }
            }
            return selectedCol;
        }
    }
}
