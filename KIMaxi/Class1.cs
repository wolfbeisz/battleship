using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BattleSDK;

namespace KIMaxi
{
    public class Class1 : BattleshipKI
    {
        private String name;

        private bool[,] shootField;
        private bool[,] hitField;
        private bool[,] shipField;
        private int fieldWidth;
        private int fieldHeight;

        private List<Vector2D> listNotShooted;

        public Class1(int size) : base(size)
        {
            name = "RoiderKI";
            if (size != -1)
            {
                shootField = new bool[size, size];
                hitField = new bool[size, size];
                shipField = new bool[size, size];

                fieldHeight = size;
                fieldWidth = size;

                listNotShooted = new List<Vector2D>();
                initialiseListNotShooted();
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
                        ship.X = random.Next(0, fieldWidth - ship.Size - 1);
                        ship.Y = random.Next(0, fieldHeight - 1);
                }
                else
                {
                        ship.X = random.Next(0, fieldWidth - 1);
                        ship.Y = random.Next(0, fieldHeight - ship.Size - 1);
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
            int index = random.Next(0, listNotShooted.Count);

            x = listNotShooted[index].getX();
            y = listNotShooted[index].getY();

            listNotShooted.RemoveAt(index);
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

        private void initialiseListNotShooted()
        {
            for (int i = 0; i < fieldWidth; i++)
            {
                for (int k = 0; k < fieldHeight; k++)
                {
                    listNotShooted.Add(new Vector2D(i, k));
                }
            }
        }
    }

    public class Vector2D
    {
        private int x;
        private int y;

        public Vector2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }
    }
}
