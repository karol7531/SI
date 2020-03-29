using CSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fill_In
{
    class TileVariable
    {
        internal Variable<string> variable;
        internal List<int> tiles;

        public TileVariable(Variable<string> variable, List<int> tiles)
        {
            this.variable = variable;
            this.tiles = tiles;
        }
    }
}
