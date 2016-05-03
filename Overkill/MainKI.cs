using System;
using BattleSDK;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Linq;

namespace Overkill{
	public class MainKI : BattleshipKI{
		private GameData _data;

		public MainKI (int size) : base(size){
			if (size == -1) //Just for naming purposes
				return;

			_data = new GameData (size);
		}
			
		public override void SetShips (List<Ship> ships){
		    _data.SetShipSizes(ships.Select(s => s.Size).ToList());

			int c = 0;
			foreach(Ship s in ships){
				s.X = c;
				s.Y = 0;
				s.Dir = Direction.VERTICAL;

				c += 2;
			}
		}

		public override string GetName ()
		{
		    return "Overkill";
		}

		public override void Shoot (out int x, out int y){
            _data.GetNextShot (out x, out y);
		}

		public override void Notify (int x, int y, bool hit, bool deadly){
			_data.Notify (x, y, hit, deadly);
		}
	}
}