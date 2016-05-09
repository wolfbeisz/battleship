using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;
using System.Diagnostics;

/*
 _____      _    _             
/ _  /_   _| | _| | ___  _ __  
\// /| | | | |/ / |/ _ \| '_ \ 
 / //\ |_| |   <| | (_) | |_) |
/____/\__, |_|\_\_|\___/| .__/ 
      |___/  Battleship |_| KI 
*/

namespace battleship_zyklop_ki {
    partial class KI : BattleshipKI {
        private CellState[,] shootField;
        private readonly int size;
        public static Random random;
        public static List<int> shipSizes;

        public KI(int fieldSize) : base(fieldSize) {
            if (fieldSize < 1)
                return;

            random = new Random();
            size = fieldSize;
            shootField = new CellState[size, size];
        }

        public override void SetShips(List<Ship> ships) {
            shipSizes = ships.Select(ship => ship.Size).ToList();
            shipSizes.Sort();

            SetShips s = new SetShips(size);
            int quality = -1;
            // Es werde die Schiffspositionen genommen die, die beste quality haben
            for (var i = 0; i < 1; i++) {
                Tuple<List<Ship>, int> poss = s.Ships(new List<Ship>(ships));
                if (poss.Item2 > quality) {
                    quality = poss.Item2;
                    ships = poss.Item1;
                }
            }
        }

        public override String GetName() {
            return "Zyklop";
        }

        public override void Shoot(out int x, out int y) {
            Coord coord = new Shoot(size, shootField).shoot();
            x = coord.x;
            y = coord.y;
        }
    }
}