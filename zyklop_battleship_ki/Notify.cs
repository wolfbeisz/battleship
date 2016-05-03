using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship_zyklop_ki {
    class Notify {
        private readonly int size;
        private CellState[,] shootField;

        public Notify(int fieldSize, CellState[,] field) {
            size = fieldSize;
            shootField = field;
        }

        public CellState[,] notify(int x, int y, bool hit, bool deadly) {
            if (!hit) // -> WATER
            {
                shootField[x, y] = CellState.WATER;
            }
            else if (!deadly) // -> PART
            {
                shootField[x, y] = CellState.PART;

                foreach (Coord coord in CoordOffset.edges(x, y)) {
                    setCellToWater(x, y);
                }
            }
            else // -> DEAD
            {
                setCellToDead(x, y);

                foreach (Coord coord in CoordOffset.plusTemplate) {
                    for (int i = 1; i < size; i++) {
                        int tmpX = x + i * coord.x;
                        int tmpY = y + i * coord.y;
                        if (tmpX >= 0 && tmpY >= 0 && tmpX < size && tmpY < size && shootField[tmpX, tmpY] == CellState.PART) {
                            //Console.WriteLine(tmpX + " " + tmpY);
                            setCellToDead(tmpX, tmpY);
                        }
                        else break;
                    }
                }
            }
            closeTooSmall();
            Logger.info(Logger.printStates(size, shootField));
            return shootField;
        }

        private void closeTooSmall() {
            bool changes = true;
            while (changes) {
                changes = false;
                for (var i = 0; i < size; i++) {
                    for (var j = 0; j < size; j++) {
                        if (shootField[i, j] == CellState.NO) {
                            int count = 0;
                            foreach (Coord coord in CoordOffset.plus(i, j)) {
                                if (cellDontExistsOrIsFinel(coord.x, coord.y))
                                    count++;
                            }
                            if (count == 4) {
                                shootField[i, j] = CellState.WATER;
                            }
                        }
                    }
                }
            }
        }

        private bool cellDontExistsOrIsFinel(int x, int y) {
            if (!(x >= 0 && y >= 0 && x < size && y < size))
                return true;
            return (shootField[x, y] == CellState.DEAD || shootField[x, y] == CellState.WATER);
        }

        private void setCellToDead(int x, int y) {
            shootField[x, y] = CellState.DEAD;
            foreach (Coord coord in CoordOffset.around(x, y)) {
                setCellToWater(coord.x, coord.y);
            }
        }

        private void setCellToWater(int x, int y) {
            if ((x >= 0 && y >= 0 && x < size && y < size && shootField[x, y] == CellState.NO)) {
                shootField[x, y] = CellState.WATER;
            }
        }
    }
}
