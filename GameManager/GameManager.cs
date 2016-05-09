using System;
using BattleSDK;
using System.Collections.Generic;

namespace GameManager
{
    public class GameManager
    {

        private int size = 10;
        private int shipAmount = 5;
        private List<Ship> shipsKI1;
        private List<Ship> shipsKI2;

        BattleshipKI KI1;
        BattleshipKI KI2;

        public Type PlayGame(Type first, Type second)
        {
            this.KI1 = KIs.NewKi(first, size);
            this.KI2 = KIs.NewKi(second, size);


            //Liste mit Schiffen für die beiden KIs 
            List<Ship> tmpShipsKI1 = createShips();
            List<Ship> tmpShipsKI2 = new List<Ship>();

            for (int i = 0; i < tmpShipsKI1.Count; i++)
                tmpShipsKI2.Add(tmpShipsKI1[i].Copy());

            bool firstToBeChecked = new Random().Next(2) == 0;

            if (firstToBeChecked) {
                try {
                    KI1.SetShips(tmpShipsKI1);
                } catch (Exception e){
                    Console.WriteLine("Error setting ships of " + KI1.GetName());
                    return second;
                }
                try {
                    KI2.SetShips(tmpShipsKI2);
                } catch (Exception e) {
                    Console.WriteLine("Error setting ships of " + KI2.GetName());
                    return first;
                }
            } else {
                try {
                    KI2.SetShips(tmpShipsKI2);
                } catch (Exception e) {
                    Console.WriteLine("Error setting ships of " + KI2.GetName());
                    return first;
                }
                try {
                    KI1.SetShips(tmpShipsKI1);
                } catch (Exception e) {
                    Console.WriteLine("Error setting ships of " + KI1.GetName());
                    return second;
                }
            }
            

            bool KI1valid = false;
            bool KI2valid = false;

            KI1valid = shipsAreValid(tmpShipsKI1);
            KI2valid = shipsAreValid(tmpShipsKI2);

            if (!KI1valid && !KI2valid)
            {
                Console.WriteLine("Error setting ships of both");
                return null;
            }

            if (KI1valid && !KI2valid)
            {
                Console.WriteLine("Error setting ships of " + KI2.GetName());
                return first;
            }

            if (!KI1valid && KI2valid)
            {
                Console.WriteLine("Error setting ships of " + KI1.GetName());
                return second;
            }

            //copy and clear

            shipsKI1 = new List<Ship>();
            shipsKI2 = new List<Ship>();

            foreach (Ship s in tmpShipsKI1)
            {
                Ship copy = s.Copy();
                copy.Hits.Clear();
                shipsKI1.Add(copy);
            }

            foreach (Ship s in tmpShipsKI2)
            {
                Ship copy = s.Copy();
                copy.Hits.Clear();
                shipsKI2.Add(copy);
            }

            bool ki1won = false;
            bool ki2won = false;
            bool firstTurn = new Random().Next(2) == 0;



            while (!(ki1won || ki2won)){

                bool hit = false;
                bool deadly = false;
                int x = -1;
                int y = -1;

                BattleshipKI cur = firstTurn ? KI1 : KI2;
                try{
                    cur.Shoot(out x, out y);
                }
                catch (Exception e){
                    Console.WriteLine(cur.GetName() + " threw Exception and lost the game\n" + e.Message + "\n");
                    return cur == KI1 ? second : first;
                }

                if (x < 0 || y < 0 || x > 9 || y > 9)
                {
                    if (cur == KI1)
                    {
                        return second;
                    }
                    else
                    {
                        return first;
                    }
                }


                List<Ship> curKIShips = cur == KI1 ? shipsKI2 : shipsKI1;



                foreach (Ship s in curKIShips)
                {
                    for (int i = 0; i < s.Size; i++)
                    {
                        if (s.Dir == Direction.HORIZONTAL)
                        {
                            int sY = s.Y;
                            int sX = s.X + i;

                            if (sX == x && sY == y)
                            {
                                hit = true;
                                if (!s.Hits.Contains(i))
                                {
                                    s.Hits.Add(i);
                                    if (s.Hits.Count == s.Size)
                                    {
                                        deadly = true;
                                        if (checkGameOver(curKIShips))
                                        {
                                            if (cur == KI1)
                                            {
                                                return first;
                                            }
                                            else
                                            {
                                                return second;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            int sY = s.Y + i;
                            int sX = s.X;

                            if (sX == x && sY == y)
                            {
                                hit = true;
                                if (!s.Hits.Contains(i))
                                {
                                    s.Hits.Add(i);
                                    if (s.Hits.Count == s.Size)
                                    {
                                        deadly = true;
                                        if (checkGameOver(curKIShips))
                                        {
                                            if (cur == KI1)
                                            {
                                                return first;
                                            }
                                            else
                                            {
                                                return second;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                cur.Notify(x, y, hit, deadly);
                firstTurn = !firstTurn;
            }

            return null;

        }

        private enum SetShipState {
            WATER,
            SHIP,
            GAP
        }

        private bool shipsAreValid(List<Ship> ships) {

            SetShipState[,] field = new SetShipState[size, size];
            foreach (Ship ship in ships) {
                if (!shipIsValid(ref field, ship.X, ship.Y, ship.Size, ship.Dir)) {
                    return false;
                }
            }
            return true;
        }

        public struct Coord {
            public int x;
            public int y;

            public Coord(int coordX, int coordY) {
                x = coordX;
                y = coordY;
            }
        }

        private bool shipIsValid(ref SetShipState[,] field, int x, int y, int s, Direction d) {
            Coord dirOff = d == Direction.HORIZONTAL ? new Coord(1, 0) : new Coord(0, 1);

            for (int i = 0; i < s; i++) {
                if ((x + i) * dirOff.x + (y + i) * dirOff.y > size - 1 || field[x + i * dirOff.x, y + i * dirOff.y] != SetShipState.WATER)
                    return false;
            }
            for (int i = 0; i < s; i++) {
                field[x, y] = SetShipState.SHIP;
                foreach (Coord coord in new Coord[] {
                    new Coord(-1, -1), new Coord(-1, 0), new Coord(-1, 1),
            new Coord(0, -1),                    new Coord(0, 1),
            new Coord(1, -1), new Coord(1, 0), new Coord(1, 1)
        }) {
                    if (x >= 0 && y >= 0 && x < size && y < size && field[x, y] == SetShipState.WATER) {
                        field[coord.x + x, coord.y + y] = SetShipState.GAP;
                    }
                }
            }

            return true;
        }



        private bool checkGameOver(List<Ship> curKIShips)
        {

            foreach (Ship s in curKIShips)
            {
                if (!(s.Hits.Count == s.Size))
                {
                    return false;
                }
            }
            return true;
        }

        private List<Ship> createShips()
        {
            Random r = new Random();
            List<Ship> ships = new List<Ship>();
            for (int i = 0; i < shipAmount; i++)
            {
                ships.Add(new Ship(r.Next(6) + 2) { X = 0, Y = 0 });
            }
            return ships;
        }
    }
}

