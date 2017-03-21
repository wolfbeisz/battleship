using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class SimpleShip
    {
        public bool Dead { public get; private set; }
        public IList<Coordinate> Cells { public get; private set;}

        public SimpleShip(IList<Coordinate> cells, bool dead)
        {
            Dead = dead;
            Cells = cells;
        }
    }
}
