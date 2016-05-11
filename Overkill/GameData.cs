// #define LOG
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata;

namespace Overkill{
	public class GameData{
		internal enum Data{
			UNKNOWN,
			EMPTY,
			HIT
		}
		internal struct Point{
			public int x;
			public int y;
		}

	    private Random _random = new Random();
		private Data[,] _fieldData; //Field that tries to mimic the enemy shield
	    private List<int> _shipSizes;
        public int fieldSize;

		public GameData (int size){
            fieldSize = size;
			_fieldData = new Data[size, size];

			for(int x = 0; x < size; x++){
				for(int y = 0; y < size; y++){
					_fieldData [x, y] = Data.UNKNOWN;
				}
			}
		}

	    public void SetShipSizes(List<int> sizes)
	    {
	        _shipSizes = sizes;
	    }

	    public void GetNextShot(out int x, out int y){
			int highest = -1;
            List<Point> points = new List<Point>();
            List<Point> shotsLeft = GetShotsLeft();
                                                   
            Dictionary<Point, int> shots = PriorizeShots (shotsLeft);

			foreach(Point key in shots.Keys){
				if(shots[key] > highest){
					highest = shots [key];
				    points.Clear();
				    points.Add(key);
				}
			}

	        if (points.Count == 0)
	        {
	            x = 0;
	            y = 0;
	        }
	        else
	        {
	            Point p = points[_random.Next(points.Count)];
	            x = p.x;
	            y = p.y;
	        }

            printStates();
    }

            
        public void printStates() {
#if LOG
            String s = "";
            for (int i = 0; i < _fieldData.GetLength(0); i++) {
                s += "\n";
                for (int j = 0; j < _fieldData.GetLength(1); j++) {
                    s += _fieldData[i, j].ToString().PadRight(5).Substring(0, 5) + " ";
                }
                Console.WriteLine(s);
                s = "";
            }
#endif
        }

        private List<Point> GetShotsLeft(){
			List<Point> points = new List<Point> ();

			for(int x = 0; x < _fieldData.GetLength (0); x++){
				for(int y = 0; y < _fieldData.GetLength(0); y++){
					if (_fieldData [x, y] == Data.UNKNOWN)
						points.Add (new Point (){ x = x, y = y });
				}
			}   

			return points;
		}
	
		private Dictionary<Point, int> PriorizeShots(List<Point> shotsLeft){
			Dictionary<Point, int> result = new Dictionary<Point, int> ();

            int bonus_not_border = 268;// 3;
            int bonus_next_single_hit = 400;// 5;
            int bonus_next_mult_hit = 1109;//  8;
            int bonus_in_larges_ship_area = 225;// 8;
            int bonus_surrounded_by_water = -30;// -1;

            foreach (Point shot in shotsLeft){
				result.Add (shot, 0);          
                  
			    //Priorize fields not at right most side
			    if (shot.x < _fieldData.GetLength(0) - 2)
			        result[shot] += bonus_not_border;

                //Priorize fields not at bottom most
                if (shot.y < _fieldData.GetLength(1) - 2)
                    result[shot] += bonus_not_border;

                //Priorize fields not surrounded by empty shots
                foreach(int size in _shipSizes) {
                    bool horX, verX;

                    ShipFits(shot.x, shot.y, size, out horX, out verX);

                    if (horX || verX)
                        result[shot] += bonus_surrounded_by_water;

                    if(horX && verX)
                        result[shot] += bonus_surrounded_by_water;
                }

                //Try to destroy ship when cur is hit
                //     0 = Single hit
                //     1 = Horizontal
                //     2 = Vertical
                { 
                    int type = 0;
			        if (shot.x > 0 && _fieldData[shot.x - 1, shot.y] == Data.HIT)
			        {
			            if (type == 1)
			            {
			                result[shot] += bonus_next_mult_hit;
			            }else if (type == 0)
			            {
			                if (shot.x > 1 && _fieldData[shot.x - 2, shot.y] == Data.HIT)
			                {
			                    type = 1;
			                    result[shot] += bonus_next_mult_hit;
						    }
			                else
			                    result[shot] += bonus_next_single_hit;
			            }
			        }
			        if (shot.x < _fieldData.GetLength(0) - 1 && _fieldData[shot.x + 1, shot.y] == Data.EMPTY)
				    {
					    if (type == 1)
					    {
						    result[shot] += bonus_next_mult_hit;
					    }else if (type == 0)
					    {
						    if (shot.x < _fieldData.GetLength(0) - 2 && _fieldData[shot.x + 2, shot.y] == Data.EMPTY)
						    {
							    type = 1;
							    result[shot] += bonus_next_mult_hit;
						    }
						    else
							    result[shot] += bonus_next_single_hit;
					    }
				    }
			        if (shot.y > 0 && _fieldData[shot.x, shot.y - 1] == Data.HIT)
				    {
					    if (type == 2)
					    {
						    result[shot] += bonus_next_mult_hit;
					    }else if (type == 0)
					    {
						    if (shot.y > 1 && _fieldData[shot.x, shot.y - 2] == Data.HIT)
						    {
							    type = 2;
							    result[shot] += bonus_next_mult_hit;
						    }
						    else
							    result[shot] += bonus_next_single_hit;
					    }
				    }
				    if (shot.y < _fieldData.GetLength(1) - 1 && _fieldData[shot.x, shot.y + 1] == Data.EMPTY)
				    {
					    if (type == 2)
					    {
						    result[shot] += bonus_next_mult_hit;
					    }else if (type == 0)
					    {
						    if (shot.y < _fieldData.GetLength(1) - 2 && _fieldData[shot.x, shot.y + 2] == Data.EMPTY)
						    {
							    type = 2;
							    result[shot] += bonus_next_mult_hit;
						    }
						    else
							    result[shot] += bonus_next_single_hit;
					    }
				    }
                }

                if (shot.x < _fieldData.GetLength(0) - 1 && _fieldData[shot.x + 1, shot.y] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 1.00f);

                if (shot.y < _fieldData.GetLength(1) - 1 && _fieldData[shot.x, shot.y + 1] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 1.00f);

                if (shot.x < _fieldData.GetLength(0) - 2 && _fieldData[shot.x + 2, shot.y] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 0.70f);

                if (shot.y < _fieldData.GetLength(1) - 2 && _fieldData[shot.x, shot.y + 2] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 0.70f);

                if (shot.x < _fieldData.GetLength(0) - 3 && _fieldData[shot.x + 3, shot.y] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 0.40f);

                if (shot.y < _fieldData.GetLength(1) - 3 && _fieldData[shot.x, shot.y + 3] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 0.40f);

                if (shot.x < _fieldData.GetLength(0) - 4 && _fieldData[shot.x + 4, shot.y] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 0.10f);

                if (shot.y < _fieldData.GetLength(1) - 4 && _fieldData[shot.x, shot.y + 4] == Data.HIT)
                    result[shot] -= (int)(bonus_not_border * 0.10f);

                //Priorize fields where the largest ship fits in
                bool hor, ver;
                ShipFits(shot.x, shot.y, _shipSizes.Max(), out hor, out ver);
                if (hor || ver)
			        result[shot] += bonus_in_larges_ship_area;
                if (hor && ver)
                    result[shot] += bonus_in_larges_ship_area;
            }

			return result;
		}

	    private void ShipFits(int baseX, int baseY, int size, out bool hor, out bool ver)
	    {
            hor = false;
            ver = false;

	        int horSize = 0;
	        int verSize = 0;

	        int x = baseX;
	        int y = baseY;
                   
	        //Hor left
	        while (_fieldData[x, y] == Data.UNKNOWN)
	        {                                           
	            horSize++;

                if (horSize >= size) {
                    hor = true;
                    break;
                }

	            if (x == 0)
	                break;

	            x--;

                if (x < 0) { 
                    hor = horSize >= size;
                    break;
                }
            }  
               
			x = baseX;
			y = baseY;
			//Hor right
			while (_fieldData[x, y] == Data.UNKNOWN)
			{                              
				horSize++;

                if (horSize >= size) { 
                    hor = true;
                    break;
                }

                if (x == _fieldData.GetLength(0))
					break;

				x++;

                if (x >= _fieldData.GetLength(0)) { 
                    hor =  horSize >= size;
                    break;
                }
            }

            x = baseX;
			y = baseY;
			//Ver up
			while (_fieldData[x, y] == Data.UNKNOWN)
			{
				verSize++;

                if (verSize >= size) {
                    ver = true;
                    break;
                }

                if (y == 0)
					break;

				y--;

                if (y < 0) {
                    ver = verSize >= size;
                    break;
                }
			}
	        
			x = baseX;
			y = baseY;
			//Ver down
			while (_fieldData[x, y] == Data.UNKNOWN)
			{
				verSize++;

                if (verSize >= size) {
                    ver = true;
                    break;
                }

                if (y == _fieldData.GetLength(1))
					break;

				y++;

                if (y >= _fieldData.GetLength(1)) {
                    ver = verSize >= size;
                    break;
                }
			}                              
	    }

		public void Notify(int x, int y, bool hit, bool deadly){
			_fieldData [x, y] = hit ? Data.HIT : Data.EMPTY;

			if (deadly)
				FindKilledShip (x, y);
		}

		private void FindKilledShip(int x, int y){
			Point[] fields = GetShipFields (x, y);

		    _shipSizes.Remove(fields.Length);

			//Mark surrounded as shot and empty
			foreach(Point p in fields){
				//Only shoot at a field if its not out of bounds and is unknown
				//Top
				if(p.y > 0 && _fieldData[p.x, p.y - 1] == Data.UNKNOWN)
					_fieldData[p.x, p.y - 1] = Data.EMPTY;

				//Bot
				if(p.y < _fieldData.GetLength (0) - 1 && _fieldData[p.x, p.y + 1] == Data.UNKNOWN)
					_fieldData[p.x, p.y + 1] = Data.EMPTY;

				//Left
				if(p.x > 0 && _fieldData[p.x - 1, p.y] == Data.UNKNOWN)
					_fieldData[p.x - 1, p.y] = Data.EMPTY;

				//Right
				if(p.x < _fieldData.GetLength (0) - 1 && _fieldData[p.x + 1, p.y] == Data.UNKNOWN)
					_fieldData[p.x + 1, p.y] = Data.EMPTY;
			}
		}

		private Point[] GetShipFields(int x, int y){
			List<Point> points = new List<Point>();

            //Left
            int cX = x;
            int cY = y;
            while (cX > 0 && _fieldData [cX - 1, cY] == Data.HIT){
				points.Add (new Point (){ x = cX - 1, y = cY });
                cX--;
            }

            //Right
            cX = x;
            cY = y;
			while(cX < _fieldData.GetLength (0) - 1 && _fieldData [cX + 1, cY] == Data.HIT) {
				points.Add (new Point (){ x = cX + 1, y = cY });
                cX++;
			}

            //Top
            cX = x;
            cY = y;
			while(cY > 0 && _fieldData [cX, cY - 1] == Data.HIT){
				points.Add (new Point (){ x = cX, y = cY - 1 });
                cY--;
			}

            //Bot
            cX = x;
            cY = y;
			while (cY < _fieldData.GetLength (0) - 1 && _fieldData [cX, cY + 1] == Data.HIT) {
				points.Add (new Point (){ x = x, y = y + 1 });
                cY++;
			}

			return points.ToArray ();
		}
	}
}

