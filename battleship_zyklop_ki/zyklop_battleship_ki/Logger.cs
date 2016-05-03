using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;
using System.Diagnostics;

namespace battleship_zyklop_ki {
    class Logger {
        public const bool MUTE = true;

        public static void info(Object s) {
            if (!MUTE) {
                Console.WriteLine(s);
            }
        }

        public static String printStates(int size, CellState[,] shootField) {
            String s = "";
            for (int i = 0; i < size; i++) {
                s += "\n";
                for (int j = 0; j < size; j++) {
                    s += (shootField[i, j].ToString() + "   ").Substring(0, 5) + " ";
                }
            }
            return s;
        }

        public static String printBools(int size, SetShipState[,] shootField) {
            String s = "";
            for (int i = 0; i < size; i++) {
                s += "\n";
                for (int j = 0; j < size; j++) {
                    switch (shootField[i, j]) {
                        case SetShipState.WATER:
                            s += ".";
                            break;
                        case SetShipState.SHIP:
                            s += "#";
                            break;
                        case SetShipState.GAP:
                            s += "'";
                            break;
                    }
                }
            }
            return s;
        }
    }
}
