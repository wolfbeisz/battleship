using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship_zyklop_ki {
    class Shoot {
        private readonly CellState[,] field;
        private int[,] c;
        private readonly int size;

        public Shoot(int fieldSize, CellState[,] shootField) {
            size = fieldSize;
            field = shootField;
            c = new int[size, size];
        }

        public Coord shoot() {
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    if (field[i, j] == CellState.NO) {
                        c[i, j] = 1 + countUnsetAround(i, j, 1);
                    }
                    else if (field[i, j] == CellState.PART) {
                        foreach (Coord coord in CoordOffset.plus(i, j)) {
                            incrementField(coord.x, coord.y, 5);
                        }
                    }
                }
            }

            return getRandomBest();
        }

        private Coord getRandomBest() {
            int maxVal = c.Cast<int>().Max();
            if (maxVal == 0) {
                string msg = "Kein Feld mehr offen";
                //Debug.WriteLine(msg);
                //Console.WriteLine(msg);
            }
            List<Coord> coords = new List<Coord>();
            for (var i = 0; i < size; i++) {
                for (var j = 0; j < size; j++) {
                    if (c[i, j] == maxVal) {
                        coords.Add(new Coord(i, j));
                    }
                }
            }
            int elem = KI.random.Next(coords.Count);
            return coords.ElementAt(elem);
        }

        private void setCellsAround(int x, int y, int to, int by) {
            foreach (Coord coord in CoordOffset.around(x, y, by)) {
                setCell(coord.x, coord.y, to);
            }
        }

        private void setCell(int x, int y, int to) {
            if (fieldExistsAndIsEmpty(x, y)) {
                c[x, y] = to;
            }
        }

        private void incrementField(int x, int y, int by) {
            if (fieldExistsAndIsEmpty(x, y)) {
                c[x, y] += by;
            }
        }

        private bool fieldExistsAndIsEmpty(int x, int y) {
            return (x >= 0 && y >= 0 && x < size && y < size && field[x, y] == CellState.NO);
        }

        private int countUnsetAround(int x, int y, int by) {
            int count = 0;
            foreach (Coord coord in CoordOffset.plus(x, y)) {
                if (fieldExistsAndIsEmpty(coord.x, coord.y))
                    count++;
            }
            return count;
        }
    }
}
