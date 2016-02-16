using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;

namespace TestKI
{
    public class TestKI : BattleshipKI
    {
        private String name;

        private Boolean[,] shootField;
        private Boolean[,] hitField;
        private Boolean[,] shipField;
        private int fieldWidth;
        private int fieldHeight;

        public TestKI(int size) : base(size)
        {
            name = "TestKI";
            if (size != -1)
            {
                shootField = new Boolean[size, size];
                hitField = new Boolean[size, size];
                shipField = new Boolean[size, size];
            }            
		}

        public override void SetShips(List<Ship> ships)
        {
            Random random = new Random();

            foreach (Ship ship in ships)
            {
                ship.Dir = GetRandomDirection();

                if (ship.Dir == Direction.HORIZONTAL)
                {
                    while (!isShipSettedRight(ship))
                    {
                        ship.X = random.Next(0, fieldWidth - ship.Size - 1);
                        ship.Y = random.Next(0, fieldHeight - 1);
                    }
                }
                else
                {
                    while (!isShipSettedRight(ship))
                    {
                        ship.X = random.Next(0, fieldWidth - 1);
                        ship.Y = random.Next(0, fieldHeight - ship.Size - 1);
                    }
                }
            }
        }

        public override String GetName()
        {
            return name;
        }

        public override void Shoot(out int x, out int y)
        {
            Random random = new Random();

            x = random.Next(0, fieldWidth - 1);
            y = random.Next(0, fieldHeight - 1);

            while(shootField[x,y])
            {
                x = random.Next(0, fieldWidth - 1);
                y = random.Next(0, fieldHeight - 1);
            }

            shootField[x, y] = true;
        }

        public override void Notify(int x, int y, bool hit, bool deadly)
        {
            hitField[x, y] = hit;
        }

        private Direction GetRandomDirection()
        {
            Random random = new Random();

            if (random.Next(0, 1) == 0)
                return Direction.HORIZONTAL;
            else
                return Direction.VERTICAL;
        }

        private bool isShipSettedRight(Ship ship)
        {
            if (ship.Dir == Direction.HORIZONTAL)
            {
                for (int i = 0; i < ship.Size; i++)
                {
                    if (!shipField[ship.X + i, ship.Y])
                        return false;
                }
            }
            else
            {
                for (int i = 0; i < ship.Size - 1; i++)
                {
                    if (!shipField[ship.X, ship.Y + i])
                        return false;
                }
            }

            return true;
        }
    }
}
