using System;
using System.Collections.Generic;

namespace BattleSDK{
	public class Ship{
		public int X, Y;
        public int Size { get; private set; }
		public Direction Dir;
		public List<int> Hits = new List<int>();
	
        public Ship(int size){
            Size = size;
        }
    }
}

