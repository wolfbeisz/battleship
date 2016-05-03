using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;
using System.Diagnostics;

namespace battleship_zyklop_ki {
    class SetShips {
        private int size;
        private SetShipState[,] field;

        public SetShips(int fieldSize) {
            size = fieldSize;
        }

        private int quality() {
            int q = 1;
            foreach (SetShipState val in field) {
                if (val == SetShipState.WATER) {
                    q++;
                }
            }
            return q;
        }

        public Tuple<List<Ship>, int> Ships(List<Ship> ships) {
            Random random = KI.random;

            while (true) {
                field = new SetShipState[size, size];
                bool valid = true;

                foreach (Ship ship in ships) {
                    if (!valid) {
                        break;
                    }
                    int x = 0;
                    int y = 0;
                    Direction dir = Direction.HORIZONTAL;
                    int s = ship.Size;

                    valid = false;
                    for (int i = 0; i < 1000; i++) {
                        x = random.Next(0, size);
                        y = random.Next(0, size);
                        dir = random.NextDouble() >= 0.5 ? Direction.HORIZONTAL : Direction.VERTICAL;

                        if (infield(x, y, s, dir)) {
                            valid = true;
                            break;
                        }
                    }

                    ship.X = x;
                    ship.Y = y;
                    ship.Dir = dir;
                }

                if (valid) {
                    Logger.info(Logger.printBools(size, field));
                    return Tuple.Create(ships, quality());
                }
            }
        }

        private void setShipField(int x, int y) {
            field[x, y] = SetShipState.SHIP;
            foreach (Coord coord in CoordOffset.around(x, y)) {
                //Console.WriteLine(coord.x +  " - " +  coord.y);
                setGapField(coord.x, coord.y);
            }
        }

        private void setGapField(int x, int y) {
            if (x >= 0 && y >= 0 && x < size && y < size && field[x, y] == SetShipState.WATER) {
                field[x, y] = SetShipState.GAP;
            }
        }

        private bool infield(int x, int y, int s, Direction d) {
            if (d == Direction.HORIZONTAL) {
                for (int i = 0; i < s; i++) {
                    if (x + i > size - 1 || field[x + i, y] != SetShipState.WATER)
                        return false;
                }
                for (int i = 0; i < s; i++) {
                    setShipField(x + i, y);
                }
            }
            else {
                for (int i = 0; i < s; i++) {
                    if (y + i > size - 1 || field[x, y + i] != SetShipState.WATER)
                        return false;
                }
                for (int i = 0; i < s; i++) {
                    setShipField(x, y + 1);
                }
            }
            return true;
        }
    }
}
