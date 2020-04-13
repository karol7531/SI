using System;

namespace SI_3
{
    class State
    {
        private bool?[,] board;
        internal int rows, cols;

        public State(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            board = new bool?[rows, cols];
        }

        internal State Clone()
        {
            State newState = new State(rows, cols);
            newState.board = board.Clone() as bool?[,];
            return newState;
        }

        internal bool CanPlace(int col)
        {
            return board[0, col] == null;
        }

        internal State NextState(bool player, int col)
        {
            State newState = this.Clone();
            for (int r = rows - 1; r >= 0; r--)
            {
                if (newState.board[r, col] == null)
                {
                    newState.board[r, col] = player;
                    return newState;
                }
            }
            return newState;
        }

        public void PrintState()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Console.Write(board[r, c] == null ? "_ " : board[r, c] == true ? "X " : "O ");
                }
                Console.WriteLine();
            }
        }

        public int Evaluation()
        {
            return DidWin(true) ? 1 : DidWin(false) ? -1 : 0;
        }

        public bool DidWin(bool player)
        {
            return DidWinHorizontal(player) || DidWinVertical(player) || DidWinDiagonal(player);
        }

        private bool DidWinHorizontal(bool player)
        {
            for (int r = 0; r < rows; r++)
            {
                int inRow = 0;
                for (int c = 0; c < cols; c++)
                {
                    inRow = board[r, c] == player ? inRow + 1 : 0;
                    if (inRow == 4) return true;
                }
            }
            return false;
        }

        private bool DidWinVertical(bool player)
        {
            for (int c = 0; c < cols; c++)
            {
                int inRow = 0;
                for (int r = 0; r < rows; r++)
                {
                    inRow = board[r, c] == player ? inRow + 1 : 0;
                    if (inRow == 4) return true;
                }
            }
            return false;
        }

        private bool DidWinDiagonal(bool player)
        {
            return DidWinDiagonalForward(player) || DidWinDiagonalBack(player);
        }

        private bool DidWinDiagonalForward(bool player)
        {
            for (int r = 3; r < rows; r++)
            {
                int inRow = 0;
                int i = r;
                int j = 0;
                while (i >= 0 && j < cols)
                {
                    inRow = board[i, j] == player ? inRow + 1 : 0;
                    if (inRow == 4) return true;
                    i--;
                    j++;
                }
            }
            for (int c = 1; c < cols - 3; c++)
            {
                int inRow = 0;
                int i = rows - 1;
                int j = c;
                while (i >= 0 && j < cols)
                {
                    inRow = board[i, j] == player ? inRow + 1 : 0;
                    if (inRow == 4) return true;
                    i--;
                    j++;
                }
            }
            return false;
        }

        private bool DidWinDiagonalBack(bool player)
        {
            for (int r = 3; r < rows; r++)
            {
                int inRow = 0;
                int i = r;
                int j = cols - 1;
                while (i >= 0 && j >= 0)
                {
                    inRow = board[i, j] == player ? inRow+1 : 0;
                    if (inRow == 4) return true;
                    i--;
                    j--;
                }
            }
            for (int c = cols - 2; c > 2; c--)
            {
                int inRow = 0;
                int i = rows - 1;
                int j = c;
                while (i >= 0 && j >= 0)
                {
                    inRow = board[i, j] == player ? inRow+1 : 0;
                    if (inRow == 4) return true;
                    i--;
                    j--;
                }
            }
            return false;
        }
    }
}
