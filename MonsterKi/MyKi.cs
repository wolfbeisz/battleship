using BattleSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterKi
{
    public class MyKi : BattleshipKI
    {
        private Random random = new Random();

        public MyKi(int size) : base(size)
        {

		}

        public override void SetShips(List<Ship> ships)
        {
            

            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override void Shoot(out int x, out int y)
        {
            throw new NotImplementedException();
        }

        public override void Notify(int x, int y, bool hit, bool deadly)
        {
            throw new NotImplementedException();
        }
    }
}
