using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship_zyklop_ki {
    partial class KI {

        public override void Notify(int x, int y, bool hit, bool deadly) {
            if (!hit) {
                NotifyWater(x, y);
            } else if (!deadly) {
                NotifyPart(x, y);
            } else {
                NotifyPart(x, y);
                NotifyDead(x, y);
            }
            closeTooSmall();
            Logger.info(Logger.printStates(size, shootField));
        }

        private void NotifyWater (int x, int y) {
            shootField[x, y] = CellState.WATER;
        }

        private void NotifyPart (int x, int y) {
            shootField[x, y] = CellState.PART;
            // Die Ecken um das Feld herum werden auf Wasser gesetzt
            foreach (Coord coord in CoordOffset.edges(x, y)) {
                setCellToWater(x, y);
            }
        }

        private void NotifyDead (int x, int y) {
            setCellToDead(x, y);
            int shipSize = 1;
            foreach (Coord coord in CoordOffset.plusTemplate) {
                for (int i = 1; i < size; i++) {
                    int tmpX = x + i * coord.x;
                    int tmpY = y + i * coord.y;
                    if (inRange(tmpX, tmpY) && shootField[tmpX, tmpY] == CellState.PART) {
                        setCellToDead(tmpX, tmpY);
                        shipSize++;
                    } else {
                        break;
                    }
                }
            }

            //bool e = shipSizes.Remove(shipSize);
            /*if (!e) {
                Logger.info("No Ship with size " + shipSize);
                Console.WriteLine(x + " " + y + ":");
                Console.WriteLine(Logger.printStates(size, shootField));
            }*/
            shipSizes.Sort();
        }

        // Alle Felder auf die das kleinste Schiff nicht mehr passt werden auf Wasser gesetzt
        private void closeTooSmall() {
            int smallest = smallestShip();
            bool changes = true;
            while (changes) {
                changes = false;
                for (var i = 0; i < size; i++) {
                    for (var j = 0; j < size; j++) {
                        if (shootField[i, j] == CellState.NO) {
                            bool smallEnouth = true;
                            foreach (Coord dir in CoordOffset.directions) {
                                if (!smallEnouth)
                                    break;
                                int space = 1;
                                foreach (int pos in new int[] { -1, 1 }) {
                                    for (int k = 1; k < size + 2; k++) {
                                        int tmpX = i + dir.x * pos * k;
                                        int tmpY = j + dir.y * pos * k;
                                        if (inRange(tmpX, tmpY) && shootField[tmpX, tmpY] != CellState.WATER) {
                                            space++;
                                        } else {
                                            break;
                                        }
                                    }
                                }
                                if (space >= smallest) {
                                    smallEnouth = false;
                                }
                            }
                            if (smallEnouth) {
                                //Console.WriteLine(Logger.printStates(size, shootField));
                                //Console.WriteLine("water " + i + " " +  j);
                                setCellToWater(i, j);
                                //Console.WriteLine(Logger.printStates(size, shootField));
                                //Console.WriteLine("-----------------------------------");
                                changes = true;
                            }
                        }
                    }
                }
            }
        }

        // Kann sich der Zustand nicht mehr ändern (Wasser, Getroffen)
        private bool cellDontExistsOrIsFinel(int x, int y) {
            if (!inRange(x, y))
                return true;
            return (shootField[x, y] == CellState.DEAD || shootField[x, y] == CellState.WATER);
        }

        private void setCellToDead(int x, int y) {
            shootField[x, y] = CellState.DEAD;
            foreach (Coord coord in CoordOffset.around(x, y)) {
                //setCellToWater(coord.x, coord.y);
            }
        }

        private void setCellToWater(int x, int y) {
            if ((inRange(x, y) && shootField[x, y] == CellState.NO)) {
                shootField[x, y] = CellState.WATER;
            }
        }

        private int smallestShip() {
            return shipSizes.First();
        }

        private int biggestShip() {
            return shipSizes.Last();
        }

        private bool inRange(int x, int y) {
            return x >= 0 && y >= 0 && x < size && y < size;
        }
    }
}
