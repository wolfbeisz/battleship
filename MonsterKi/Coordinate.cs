using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class Coordinate
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Coordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Coordinate)
            {
                Coordinate other = (Coordinate)obj;
                return Row == other.Row && Col == other.Col;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 13 * Row;
            hash = 13 * hash + Col;
            return hash;
        }
    }
}
