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
            Random r = new Random();

			int c = 0;
			foreach(Ship s in ships){
                do {
                    s.X = r.Next(_data.fieldSize - s.Size);
                    s.Y = r.Next(_data.fieldSize - s.Size);

                    s.Dir = (r.Next(2) == 1 ? Direction.HORIZONTAL : Direction.VERTICAL);
                } while (!Val(ships, s));

                s.X = c;
				s.Y = 0;
				s.Dir = Direction.VERTICAL;

				c += 2;
			}
		}

        private bool Val(List<Ship> ships, Ship ship) {
            foreach(Ship s in ships) {
                if (s == ship)
                    continue;

                int cX = s.X;
                int cY = s.Y;
                    
                for(int i = 0; i < s.Size; i++) {
                    int oX = ship.X;
                    int oY = ship.Y;

                    for (int j = 0; j < ship.Size; j++) {
                        if (ship.Dir == Direction.HORIZONTAL)
                            oX++;
                        else
                            oY++;

                        if (cX == oX && cY == oY)
                            return false;
                    }

                    if (s.Dir == Direction.HORIZONTAL)
                        cX++;
                    else
                        cY++;
                }
            }

            return true;
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