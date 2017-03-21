using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class Coordinate
    {
        public int Row { public get; private set; }
        public int Col { public get; private set; }

        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
