using System;
using System.Collections.Generic;

namespace BattleSDK{
	public abstract class BattleshipKI{
		public BattleshipKI (int size){
		}

		public abstract void SetShips(List<Ship> ships);
		public abstract String GetName();
		public abstract void Shoot(out int x, out int y);
		public abstract void Notify(int x, int y, bool hit, bool deadly);
	}
}

