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

        // quality wird berechnet, je nachdem wie viele Zellen mit Wasser belegt sind
        private int quality() {
            int quality = 0;
            foreach (SetShipState val in field) {
                if (val == SetShipState.WATER) {
                    quality++;
                }
            }
            return quality;
        }

        // Zufällige valide Schiffspositionen
        public Tuple<List<Ship>, int> Ships(List<Ship> ships) {
            Random random = KI.random;

            while (true) {
                field = new SetShipState[size, size];
                bool valid = true;

                foreach (Ship ship in ships) {
                    if (!valid) {
                        break;
                    }

                    valid = false;
                    for (int i = 0; i < 1000; i++) {
                        ship.Dir = random.NextDouble() >= 0.5 ? Direction.HORIZONTAL : Direction.VERTICAL;
                        ship.X = random.Next(size);
                        ship.Y = random.Next(size);

                        if (infield(ship.X, ship.Y, ship.Size, ship.Dir)) {
                            valid = true;
                            break;
                        }
                    }
                }

                if (valid) {
                    //Logger.info(Logger.printBools(size, field));
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

        // Schiff setzen, wenn es auf das Feld passt
        private bool infield(int x, int y, int s, Direction d) {
            Coord dirOff = d == Direction.HORIZONTAL ? new Coord(1, 0) : new Coord(0, 1);

            for (int i = 0; i < s; i++) {
                if ((x + i) * dirOff.x + (y + i) * dirOff.y > size - 1 || field[x + i * dirOff.x, y + i * dirOff.y] != SetShipState.WATER)
                    return false;
            }
            for (int i = 0; i < s; i++) {
                setShipField(x + i * dirOff.x, y + i * dirOff.y);
            }

            return true;
        }
    }
}
