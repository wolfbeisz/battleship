using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;

namespace MonsterKi
{
    public class AttackMapBuilder
    {

        public static Board Build(Board board)
        {
            int[][] state = (int[][])board.State.Clone();

            int size = board.State.Count();
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (int colIndex = 0; colIndex < size; colIndex++)
                {
                    int currentValue = state[rowIndex][colIndex];

                    if (currentValue != (int)Field.WATER) 
                    {
                        //var template = BoardUtil.BuildShipTemplate(1, Direction.HORIZONTAL, 0, 0, 0);
                        //BoardUtil.SetFields(state, template, rowIndex - 1, colIndex - 1, ApplyField);
                        
                    }
                }
            }
            return new Board(new List<Ship>(), state);
        }

        public static void ApplyField(int[][] state, int row, int column) 
        { 
        
        }

        public static IList<SimpleShip> DetectShips(int[][] state)
        {
            List<SimpleShip> detected = new List<SimpleShip>();
            

            for (int rowIndex = 0; rowIndex < state.Count(); rowIndex++)
            {
                for (int colIndex = 0; colIndex < state.Count(); colIndex++)
                {
                    int currentValue = state[rowIndex][colIndex];

                    if (currentValue == (int)Field.HIT ||
                        currentValue == (int)Field.DEADLY_HIT)
                    {
                        var neighbours = findNeighbours(state, rowIndex, colIndex);

                        foreach (Coordinate neighbour in neighbours)
                        {
                            var ships = detected.Where(ship => ship.Cells.Contains(new Coordinate(rowIndex, colIndex))).ToList();
                            
                            if (ships.Count() == 0)
                            {
                                List<Coordinate> coordinates = new List<Coordinate>();
                                coordinates.Add(neighbour);
                                ships.Add(new SimpleShip(coordinates, currentValue == (int)Field.DEADLY_HIT));
                            }
                            else if (ships.Count() == 1) 
                            {

                            } else {
                                throw new Exception();
                            }
                        }
                        
                    }
                    // if the current value is a hit => check the surrounding cells 
                    // if the cell exists, then check whether the cells belong to a ship (in detected)
                    // if it does not belong to a ship, then create a new ship
                }
            }

            return detected;
        }

        private static IEnumerable<Coordinate> findNeighbours(int[][] state, int row, int col)
        {
            IList<Coordinate> coordinates = new List<Coordinate>();
            
            if (BoardUtil.ExistsField(state, row - 1, col))
            {
                coordinates.Add(new Coordinate(row - 1, col));
            }

            if (BoardUtil.ExistsField(state, row + 1, col))
            {
                coordinates.Add(new Coordinate(row + 1, col));
            }

            if (BoardUtil.ExistsField(state, row, col - 1))
            {
                coordinates.Add(new Coordinate(row, col - 1));
            }

            if (BoardUtil.ExistsField(state, row, col + 1))
            {
                coordinates.Add(new Coordinate(row, col + 1));
            }

            return coordinates;
        }

        
    }
}
