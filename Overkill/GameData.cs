﻿using System;
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

		public GameData (int size){
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

			foreach(Point shot in shotsLeft){
				result.Add (shot, 0);

			    //Priorize fields not at border
				if (shot.x > 0 && shot.x < _fieldData.GetLength (0) - 1)
					result [shot]++;

				if (shot.y > 0 && shot.y < _fieldData.GetLength (0) - 1)
					result [shot]++;

			    //Priorize fields not at right most side
			    if (shot.x < _fieldData.GetLength(0) - 2)
			        result[shot]++;

                //Priorize fields not at bottom most
                //if (shot.y < _fieldData.GetLength(1) - 2)
                //    result[shot]++;

                //Priorize fields where the largest ship fits in
			    if (ShipFits(shot.x, shot.y, _shipSizes.Max()))
			        result[shot] += 2;
			}

			return result;
		}

	    private bool ShipFits(int baseX, int baseY, int size)
	    {
	        int horSize = 0;
	        int verSize = 0;

	        int x = baseX;
	        int y = baseY;
                   
	        //Hor left
	        while (_fieldData[x, y] == Data.UNKNOWN)
	        {                                           
	            horSize++;

                if (horSize >= size)
                    return true;

	            if (x == 0)
	                break;

	            x--;

                if (x < 0)
                    return horSize >= size;
            }  
               
			x = baseX;
			y = baseY;
			//Hor right
			while (_fieldData[x, y] == Data.UNKNOWN)
			{                              
				horSize++;

                if (horSize >= size)
                    return true;

                if (x == _fieldData.GetLength(0))
					break;

				x++;

                if (x >= _fieldData.GetLength(0))
                    return horSize >= size;
			}

            x = baseX;
			y = baseY;
			//Ver up
			while (_fieldData[x, y] == Data.UNKNOWN)
			{
				verSize++;

                if (verSize >= size)
                    return true;

                if (y == 0)
					break;

				y--;

                if (y < 0)
                    return verSize >= size;
			}
	        
			x = baseX;
			y = baseY;
			//Ver down
			while (_fieldData[x, y] == Data.UNKNOWN)
			{
				verSize++;

                if (verSize >= size)
                    return true;

                if (y == _fieldData.GetLength(1))
					break;

				y++;

                if (y >= _fieldData.GetLength(1))
                    return verSize >= size;
			}

            return horSize >= size || verSize >= size;                                    
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
