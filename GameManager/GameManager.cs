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
        
        List<BattleshipKI> KIList = new List<BattleshipKI>();

        public Type PlayGame(Type first, Type second){

            //Liste mit Schiffen für die beiden KIs 
            List<Ship> tmpShipsKI1 = createShips();
            List<Ship> tmpShipsKI2 = new List<Ship>(tmpShipsKI1);

            //Die KIs zu einer Liste hinzufügen
            KIList.Add(KIs.NewKi(first, size));
            KIList.Add(KIs.NewKi(second, size));

            // Den KIs die Schiffe übergeben
            KIList[0].SetShips(tmpShipsKI1);
            KIList[1].SetShips(tmpShipsKI2);

            bool KI1valid = false; 
            bool KI2valid = false;

            foreach(Ship s in tmpShipsKI1){
                  if(validatePos(s)){
                     KI1valid = true;
                  } else {
                      KI1valid = false;
                      break;
                  }
            }

             foreach(Ship s in tmpShipsKI2){
                  if(validatePos(s)){
                     KI2valid = true;
                  } else {
                     KI2valid = false;
                     break;
                  }
            }
           
            if(!KI1valid && !KI2valid){
                return null;
            }

            if(KI1valid && !KI2valid){
                return first;
            }

            if(!KI1valid && KI2valid){
                return second;
            }

            //copy and clear


        }

        private bool validatePos(Ship s){
            bool[,] field = new bool[this.size, this.size];

            bool[,] coords = new bool[this.size, this.size];
            switch(s.Dir){
                case Direction.HORIZONTAL:
                    try{
                        for(int i = 0; i < s.Size; i++){
                            coords[s.X + i, s.Y] = true;
                        }                        
                    }catch(IndexOutOfRangeException){
                        return false;
                    }
                    break;
                case Direction.VERTICAL:
                    try{
                        coords[s.X, s.Y + 1] = true;
                    }catch(IndexOutOfRangeException){
                        return false;
                    }
                    break;
            }

            return SetCoordinates(coords, field);
        }

        private bool SetCoordinates(bool[,] coords, bool[,] field){
            for(int x = 0; x < this.size; x++){
                for(int y = 0; y < this.size; y++){
                    if(!coords[x, y])
                        continue;

                    if(field[x, y])
                        return false;

                    if(x > 0 && field[x - 1, y] && !coords[x - 1, y]){
                        return false;
                    }

                    if(x < this.size -1 && field[x + 1, y] && !coords[x + 1, y]){
                        return false;
                    }
                    
                    if(y > 0 && field[x, y - 1] && !coords[x, y - 1]){
                        return false;
                    }

                    if(y < this.size -1 && field[x, y + 1] && !coords[x, y + 1]){
                        return false;
                    }

                    field[x, y] = true;
                }
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

