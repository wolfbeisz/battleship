using BattleSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class Board
    {
        public IList<Ship> Ships { get; private set; }
        public int[][] State { get; private set; }

        public Board(IList<Ship> ships, int[][] state)
        {
            State = state;
            Ships = ships;
        }

        public void SetField(int row, int column, int value)
        {
            State[row][column] = value;
        }
    }
}
