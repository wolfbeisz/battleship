using System;
using BattleSDK;
using System.Collections.Generic;

namespace GameManager
{
	public class GameManager
	{

        int size = 10;
        int shipAmount = 5;
        List<BattleshipKI> KIs = new List<BattleshipKI>();

        public Type PlayGame(Type first, Type second){

            List<Ship> ships = createShips();
            KIs.Add(KIs.NewKi(first, size));
            KIs.Add(KIs.NewKi(first, size));



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

