﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;

namespace TestKI
{
    public class TestKI : BattleshipKI
    {
        private String name;
        private Boolean[,] field;

        public TestKI(int size) : base(size)
        {
            name = "TestKI";

            if (size == -1) {
                return;
            }

            field = new Boolean[size, size];
		}

        public override void SetShips(List<Ship> ships)
        {

        }

        public override String GetName()
        {
            return name;
        }

        public override void Shoot(out int x, out int y)
        {
			x = 1;
			y = 1;
        }

        public override void Notify(int x, int y, bool hit, bool deadly)
        {

        }
    }
}
