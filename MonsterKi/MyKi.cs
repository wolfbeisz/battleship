using BattleSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class MyKi : BattleshipKI
    {
        private Random _random = new Random();
        private Board _board;
        private int _size;

        public MyKi(int size) : base(size)
        {
            if (size > 0)
            {
                _size = size;

                int[][] map = new int[size][];
                for (int i = 0; i < size; i++) {
                    map[i] = Enumerable.Repeat(-1, size).ToArray();
                }
                _board = new Board(new List<Ship>(), map);
            }
		}

        public override void SetShips(List<Ship> ships)
        {
            // this modifies the specified ships
            Board b = BoardBuilder.BuildRandomBoard(_random, ships, _size);
        }

        public override string GetName()
        {
            return "MonsterKi";
        }

        public override void Shoot(out int x, out int y)
        {
            var coordinates = AttackMapBuilder.Build(_board.State);
            Coordinate target = coordinates.FirstOrDefault();
            
            if (target == null)
            {
                target = AttackMapBuilder.RandomCoordinate(_board.State, _random);
            }

            x = target.Col;
            y = target.Row;
        }

        public override void Notify(int x, int y, bool hit, bool deadly)
        {
            if (hit && deadly)
            {
                _board.SetField(y, x, (int)Field.DEADLY_HIT);
            }
            else if (hit)
            {
                _board.SetField(y, x, (int)Field.HIT);
            }
            else
            {
                _board.SetField(y, x, (int)Field.WATER);
            }
        }
    }
}
