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
            List<Ship> tmpShipsKI2 = new List<Ship>(tmpShipsKI1);


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

            if (firstToBeChecked) {

                foreach (Ship s in tmpShipsKI1) {
                    if (validatePos(s)) {
                        KI1valid = true;
                    } else {
                        KI1valid = false;
                        break;
                    }
                }

                foreach (Ship s in tmpShipsKI2) {
                    if (validatePos(s)) {
                        KI2valid = true;
                    } else {
                        KI2valid = false;
                        break;
                    }
                }
            } else {

                foreach (Ship s in tmpShipsKI2) {
                    if (validatePos(s)) {
                        KI2valid = true;
                    } else {
                        KI2valid = false;
                        break;
                    }
                }

                foreach (Ship s in tmpShipsKI1) {
                    if (validatePos(s)) {
                        KI1valid = true;
                    } else {
                        KI1valid = false;
                        break;
                    }
                }
            }


            if (!KI1valid && !KI2valid)
            {
                return null;
            }

            if (KI1valid && !KI2valid)
            {
                return first;
            }

            if (!KI1valid && KI2valid)
            {
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
                    return cur == KI1 ? first : second;
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

        private bool validatePos(Ship s)
        {
            bool[,] field = new bool[this.size, this.size];

            bool[,] coords = new bool[this.size, this.size];
            switch (s.Dir)
            {
                case Direction.HORIZONTAL:
                    try
                    {
                        for (int i = 0; i < s.Size; i++)
                        {
                            coords[s.X + i, s.Y] = true;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                    break;
                case Direction.VERTICAL:
                    try
                    {
                        coords[s.X, s.Y + 1] = true;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                    break;
            }

            return SetCoordinates(coords, field);
        }

        private bool SetCoordinates(bool[,] coords, bool[,] field)
        {
            for (int x = 0; x < this.size; x++)
            {
                for (int y = 0; y < this.size; y++)
                {
                    if (!coords[x, y])
                        continue;

                    if (field[x, y])
                        return false;

                    if (x > 0 && field[x - 1, y] && !coords[x - 1, y])
                    {
                        return false;
                    }

                    if (x < this.size - 1 && field[x + 1, y] && !coords[x + 1, y])
                    {
                        return false;
                    }

                    if (y > 0 && field[x, y - 1] && !coords[x, y - 1])
                    {
                        return false;
                    }

                    if (y < this.size - 1 && field[x, y + 1] && !coords[x, y + 1])
                    {
                        return false;
                    }

                    field[x, y] = true;
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

