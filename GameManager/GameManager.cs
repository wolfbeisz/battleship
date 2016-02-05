using System;
using BattleSDK;
using System.Collections.Generic;

namespace GameManager
{
	public class GameManager
	{

        int size = 10;
        int shipAmount = 5;
        
        List<BattleshipKI> KIList = new List<BattleshipKI>();

        public Type PlayGame(Type first, Type second){

            //Liste mit Schiffen für die beiden KIs 
            List<Ship> shipsKI1 = createShips();
            List<Ship> shipsKI2 = new List<Ship>(shipsKI1);

            //Die KIs zu einer Liste hinzufügen
            KIList.Add(KIs.NewKi(first, size));
            KIList.Add(KIs.NewKi(second, size));

            // Den KIs die Schiffe übergeben
            KIList[0].SetShips(shipsKI1);
            KIList[1].SetShips(shipsKI2);

            foreach(Ship s in shipsKI1){

            }

           



        }

        private bool validatePos(Ship s){
            bool[,] field = new bool[this.size, this.size];

            if(field[s.X,s.Y]){
                return false;
            }

            switch(s.Dir){
                case Direction.HORIZONTAL:
                    try{
                        for(int i = 1; i <= s.Size; i++){
                            bool test = field[s.X + i, s.Y];
                            
                            if(test){
                                return false;
                            }
                        }                        
                    }catch(IndexOutOfRangeException){
                        return false;
                    }
                    break;
                case Direction.VERTICAL:
                    try{
                        for(int i = 1; i <= s.Size; i++){
                            bool test = field[s.X, s.Y + i];
                        }
                    }catch(IndexOutOfRangeException){
                        return false;
                    }
                    break;
            }


            
            return true;
        }

        private List<Ship> createShips(){
            Random r = new Random();
            List<Ship> ships = new List<Ship>();
            for (int i = 0; i < shipAmount; i++){
                ships.Add(new Ship(r.Next(6) + 2) { X = 0, Y = 0 });
            }
            return ships;
		}       
}

