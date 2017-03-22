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

        public static IList<Coordinate> Build(int[][] state)
        {
            IList<SimpleShip> ships = DetectShips(state);
            IList<Coordinate> targets = new List<Coordinate>();
            for (int rowIndex = 0; rowIndex < state.Count(); rowIndex++)
            {
                for (int colIndex = 0; colIndex < state.Count(); colIndex++)
                {
                    int currentValue = state[rowIndex][colIndex];
                    Coordinate currentCoordinate = new Coordinate(rowIndex, colIndex);

                    if (currentValue == (int)Field.HIT 
                        || currentValue == (int)Field.DEADLY_HIT) 
                    {
                        var ship = ships.Where(s => s.Cells.Contains(currentCoordinate)).ToList().Single();

                        if (ship.Cells.Count == 0)
                        {
                            throw new Exception("no matching ship found");
                        }
                        else if (ship.Cells.Count == 1)
                        {
                            foreach (var coordinate in findNeighbours(state, rowIndex, colIndex))
                            {
                                targets.Add(coordinate);
                            }
                        }
                        else
                        {
                            AddTargetForShip(state, targets, currentCoordinate, ship);
                        }

                        //var template = BoardUtil.BuildShipTemplate(1, Direction.HORIZONTAL, 0, 0, 0);
                        //BoardUtil.SetFields(state, template, rowIndex - 1, colIndex - 1, ApplyField);
                    }
                }
            }
            return targets;
        }

        private static void AddTargetForShip(int[][] state, IList<Coordinate> targets, Coordinate currentCoordinate, SimpleShip ship)
        {
            if (!ship.Dead)
            {
                Coordinate first = ship.Cells.First(); // relies on implementation details of DetectShips
                Coordinate last = ship.Cells.Last();

                if (first.Row == last.Row) // horizontal
                {
                    if (currentCoordinate.Equals(first))
                    {
                        if (BoardUtil.ExistsField(state, Left(currentCoordinate)))
                            targets.Add(Left(currentCoordinate));
                    }
                    else if (currentCoordinate.Equals(last))
                    {
                        if (BoardUtil.ExistsField(state, Right(currentCoordinate)))
                            targets.Add(Right(currentCoordinate));
                    }
                }
                else // vertical
                {
                    if (currentCoordinate.Equals(first))
                    {
                        if (BoardUtil.ExistsField(state, Upper(currentCoordinate)))
                            targets.Add(Upper(currentCoordinate));
                    }
                    else if (currentCoordinate.Equals(last))
                    {
                        if (BoardUtil.ExistsField(state, Lower(currentCoordinate)))
                            targets.Add(Lower(currentCoordinate));
                    }
                }
            }
        }

        public static void ApplyField(int[][] state, int row, int column) 
        { 
        
        }

        // for all fields:
        // if the current field is a hit => check the surrounding cells 
        // if the cell exists, then check whether the cells belong to a ship (in detected)
        // if it belongs to a ship, then update the ship (add the current field)
        // if it does not belong to a ship, then create a new ship
        public static IList<SimpleShip> DetectShips(int[][] state)
        {
            List<SimpleShip> detected = new List<SimpleShip>();

            for (int rowIndex = 0; rowIndex < state.Count(); rowIndex++)
            {
                for (int colIndex = 0; colIndex < state.Count(); colIndex++)
                {
                    Coordinate currentCoordinate = new Coordinate(rowIndex, colIndex);
                    int currentValue = state[rowIndex][colIndex];

                    if (currentValue == (int)Field.HIT ||
                        currentValue == (int)Field.DEADLY_HIT)
                    {
                        IList<Coordinate> neighbours = findNeighbours(state, rowIndex, colIndex);
                        bool match = false;

                        foreach (Coordinate neighbour in neighbours)
                        {
                            var ships = detected.Where(ship => ship.Cells.Contains(neighbour)).ToList();
                            
                            if (ships.Count() == 1)
                            {
                                SimpleShip current = ships.Single();
                                if (currentValue == (int)Field.DEADLY_HIT)
                                {
                                    detected.Remove(current);
                                    var coordinates = new List<Coordinate>(current.Cells);
                                    coordinates.Add(currentCoordinate);
                                    detected.Add(new SimpleShip(coordinates, currentValue == (int)Field.DEADLY_HIT));
                                }
                                else {
                                    current.Cells.Add(currentCoordinate);
                                }
                                match = true;
                                break;
                            } else if (ships.Count > 1) {
                                throw new Exception();
                            }
                        }

                        if (!match)
                        {
                            detected.Add(new SimpleShip(new List<Coordinate>(new Coordinate[]{ currentCoordinate }), currentValue == (int)Field.DEADLY_HIT));
                        }
                    }
                }
            }

            return detected;
        }

        private static IList<Coordinate> findNeighbours(int[][] state, int row, int col)
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

        private static Coordinate Upper(Coordinate c)
        {
            return new Coordinate(c.Row - 1, c.Col);
        }

        private static Coordinate Lower(Coordinate c)
        {
            return new Coordinate(c.Row + 1, c.Col);
        }

        private static Coordinate Left(Coordinate c)
        {
            return new Coordinate(c.Row, c.Col - 1);
        }

        private static Coordinate Right(Coordinate c)
        {
            return new Coordinate(c.Row, c.Col + 1);
        }
    }
}
