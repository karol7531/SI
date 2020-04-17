namespace Connect_4
{
    public class State
    {
        public const int pointsWin = 100000;
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

        public bool CanPlace(int col)
        {
            return board[0, col] == null;
        }

        public bool CanPlace()
        {
            for(int c = 0; c< cols; c++)
            {
                if (CanPlace(c)) return true;
            }
            return false;
        }

        public bool? GetValue(int row, int col)
        {
            return board[row, col];
        }

        public State NextState(bool player, int col)
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

        public override string ToString()
        {
            string output = "";
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    output += board[r, c] == null ? "_ " : board[r, c] == true ? "X " : "O ";
                }
                output += "\n";
            }
            return output;
        }

        public int Evaluation(bool player)
        {
            int truePoints = Points(true);
            if (truePoints >= pointsWin) return pointsWin;
            int falsePoints = Points(false);
            if (falsePoints >= pointsWin) return - pointsWin;
            return truePoints - falsePoints;
        }

        public int Points(bool player)
        {
            return PointsHorizontal(player) + PointsVertical(player) + PointsDiagonalForward(player) + PointsDiagonalBack(player);
        }

        private int PointsHorizontal(bool player)
        {
            int points = 0;
            for (int r = 0; r < rows; r++)
            {
                int inRow = 0;
                for (int c = 0; c < cols; c++)
                {
                    inRow = board[r, c] == player ? inRow + 1 : 0;
                    if (inRow == 4) return pointsWin;
                    points += inRow;
                }
            }
            return points;
        }

        private int PointsVertical(bool player)
        {
            int points = 0;
            for (int c = 0; c < cols; c++)
            {
                int inRow = 0;
                for (int r = 0; r < rows; r++)
                {
                    inRow = board[r, c] == player ? inRow + 1 : 0;
                    if (inRow == 4) return pointsWin;
                    points += inRow;
                }
            }
            return points;
        }

        private int PointsDiagonalForward(bool player)
        {
            int points = 0;
            for (int r = 3; r < rows; r++)
            {
                int inRow = 0;
                int i = r;
                int j = 0;
                while (i >= 0 && j < cols)
                {
                    inRow = board[i, j] == player ? inRow + 1 : 0;
                    if (inRow == 4) return pointsWin;
                    points += inRow;
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
                    if (inRow == 4) return pointsWin;
                    points += inRow;
                    i--;
                    j++;
                }
            }
            return points;
        }

        private int PointsDiagonalBack(bool player)
        {
            int points = 0;
            for (int r = 3; r < rows; r++)
            {
                int inRow = 0;
                int i = r;
                int j = cols - 1;
                while (i >= 0 && j >= 0)
                {
                    inRow = board[i, j] == player ? inRow + 1 : 0;
                    if (inRow == 4) return pointsWin;
                    points += inRow;
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
                    inRow = board[i, j] == player ? inRow + 1 : 0;
                    if (inRow == 4) return pointsWin;
                    points += inRow;
                    i--;
                    j--;
                }
            }
            return points;
        }
    }
}
