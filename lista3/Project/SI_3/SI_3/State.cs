using System;
using System.Collections.Generic;
using System.Text;

namespace SI_3
{
    class State
    {
        private bool?[,] board;
        private int rows, cols;

        public State(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            board = new bool?[rows, cols];
        }

        internal State Clone() { }

        internal bool CanPlace(int col) { }

        internal State NextState(bool player, int col) { }

        public void PrintState() { }

        public bool DidWin(bool player) { }



    }
}
