using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class Coordinate : IEqualityComparer<Coordinate>
    {
        public int Row { public get; private set; }
        public int Col { public get; private set; }

        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }


        public bool Equals(Coordinate x, Coordinate y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            return x.Row == y.Row && x.Col == y.Col;
        }

        public int GetHashCode(SimpleShip obj)
        {
            int hash = 13 * Row;
            hash = 13 * hash + Col;
            return hash;
        }
    }
}
