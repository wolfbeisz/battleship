using BattleSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class BoardBuilder
    {
        public static Board ConvertToBoard(IList<Ship> ships, int size)
        {
            //int[][] state

            return null;
        }

        public static Board BuildRandomBoard(Random random, IList<Ship> ships, int size)
        {
            int[][] state = new int[size][];
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                state[rowIndex] = new int[size];
            }

            IList<Ship> shipsSorted = new List<Ship>(ships.OrderBy(s => s.Size, Comparer<int>.Create((a, b) => b - a)));

            foreach (Ship ship in shipsSorted)
            {
                Direction dir = RandomDirection(random);
                try
                {
                    if (dir == Direction.HORIZONTAL)
                    {
                        setShipHorizontal(random, ship, state);
                    }
                    else
                    {
                        setShipVertical(random, ship, state);
                    }
                }
                catch (Exception e) // if an error occurs: try the other direction
                {
                    if (dir == Direction.HORIZONTAL)
                    {
                        setShipVertical(random, ship, state);
                    }
                    else
                    {
                        setShipHorizontal(random, ship, state);
                    }
                }
            }

            return new Board(ships, state);
        }

        private static void setShipVertical(Random random, Ship ship, int[][] state)
        {
            IList<int> columns = new List<int>(Enumerable.Range(0, state.Length));
            //columns = columns.OrderBy(e => random.Next()).ToList();

            foreach (int column in columns)
            {
                IList<int> rows = new List<int>(Enumerable.Range(0, state.Length));
                rows = rows.OrderBy(e => random.Next()).ToList();
                foreach (int rowIndex in rows)
                {
                    int startX = column;
                    int startY = rowIndex;
                    Direction direction = Direction.VERTICAL;
                    int size = ship.Size;
                    bool valid = canSetShip(state, startX, startY, direction, size);
                    if (valid)
                    {
                        for (int targetRow = startY; targetRow < startY + size; targetRow++)
                        {
                            state[targetRow][startX] = 1;
                        }
                        ship.X = column;
                        ship.Y = rowIndex;
                        ship.Dir = Direction.VERTICAL;
                        return;
                    }
                }
            }

            throw new Exception("unable to place the ship");

            // for each cell in a column
            // TODO: check that the required space exists, is empty and next to each other

            // TODO: check that ships do not touch

            // TODO set ship 

            // return after first match;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="startX">column</param>
        /// <param name="startY">row</param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        //private static bool canSetShip(int[][] state, int startX, int startY, Direction direction, int size)
        //{
        //    if (direction == Direction.VERTICAL) {
        //        for (int rowIndex = startY; rowIndex < startY + size; rowIndex++)
        //        {
        //            if (!existsField(state, startX, rowIndex) ||
        //                state[rowIndex][startX] != 0)
        //                return false;
        //        }
        //    }
        //    else if (direction == Direction.HORIZONTAL)
        //    {
        //        for (int colIndex = startX; colIndex < startX + size; colIndex++)
        //        {
        //            if (!existsField(state, colIndex, startY) ||
        //                state[startY][colIndex] != 0)
        //                return false;
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("invalid direction");
        //    }

        //    return true;
        //}

        private static bool canSetShip(int[][] state, int startX, int startY, Direction direction, int size)
        {
            var template = BoardUtil.BuildShipTemplate(size, direction, startX, startY, 0);
            return BoardUtil.TestBoard(state, template, startY - 1, startX - 1);
        }

        private static bool existsField(int[][] state, int x, int y)
        {
            return x >= 0 && y >= 0 && x < state.Length && y < state.Length;
        }

        private static void setShipHorizontal(Random random, Ship ship, int[][] state)
        {
            IList<int> rows = new List<int>(Enumerable.Range(0, state.Length));
            //rows = rows.OrderBy(e => random.Next()).ToList();

            foreach (int row in rows)
            {
                // TODO randomize start rows
                IList<int> columns = new List<int>(Enumerable.Range(0, state.Length));
                columns = columns.OrderBy(e => random.Next()).ToList();
                foreach (int colIndex in columns)
                {
                    int startX = colIndex;
                    int startY = row;
                    Direction direction = Direction.HORIZONTAL;
                    int size = ship.Size;
                    bool valid = canSetShip(state, startX, startY, direction, size);
                    if (valid)
                    {
                        for (int targetCol = startX; targetCol < startX + size; targetCol++)
                        {
                            state[startY][targetCol] = 1;
                        }
                        ship.X = colIndex;
                        ship.Y = row;
                        ship.Dir = Direction.HORIZONTAL;
                        return;
                    }
                }
            }

            throw new Exception("unable to place the ship");
        }

        private static Direction RandomDirection(Random random)
        {
            if (random.Next(0, 2) == 0)
                return Direction.HORIZONTAL;
            else
                return Direction.VERTICAL;
        }
    }
}
