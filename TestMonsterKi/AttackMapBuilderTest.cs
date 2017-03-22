using BattleSDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonsterKi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonsterKi
{
    [TestClass]
    public class AttackMapBuilderTest
    {
        [TestMethod]
        public void TestShipDetection()
        {
            int[][] state = new int[10][];
            state[0] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[1] = new int[] { -1,  2,  2, -1, -1, -1, -1, -1, -1, -1 };
            state[2] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[3] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[4] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[5] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[6] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[7] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[8] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[9] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            var detectedShips = AttackMapBuilder.DetectShips(state);
            Assert.AreEqual(1, detectedShips.Count);
        }

        [TestMethod]
        public void TestFindTargetsForHorizontalShip()
        {
            int[][] state = new int[10][];
            state[0] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[1] = new int[] { -1,  2,  2, -1, -1, -1, -1, -1, -1, -1 };
            state[2] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[3] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[4] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[5] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[6] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[7] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[8] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[9] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            IList<Coordinate> coords = AttackMapBuilder.Build(state);
            Assert.AreEqual(2, coords.Count);
        }

        [TestMethod]
        public void TestFindTargetsForVerticalShip()
        {
            int[][] state = new int[10][];
            state[0] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[1] = new int[] { -1,  2, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[2] = new int[] { -1,  2, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[3] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[4] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[5] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[6] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[7] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[8] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[9] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            IList<Coordinate> coords = AttackMapBuilder.Build(state);
            Assert.AreEqual(2, coords.Count);
        }

        [TestMethod]
        public void TestFindTargetsForSingleHit()
        {
            int[][] state = new int[10][];
            state[0] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[1] = new int[] { -1,  2, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[2] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[3] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[4] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[5] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[6] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[7] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[8] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[9] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            IList<Coordinate> coords = AttackMapBuilder.Build(state);
            Assert.AreEqual(4, coords.Count);
        }

        [TestMethod]
        public void TestFindTargetsForSingleHitInCorner()
        {
            int[][] state = new int[10][];
            state[0] = new int[] {  2, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[1] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[2] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[3] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[4] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[5] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[6] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[7] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[8] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[9] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            IList<Coordinate> coords = AttackMapBuilder.Build(state);
            Assert.AreEqual(2, coords.Count);
        }

        [TestMethod]
        public void TestExcludeFieldsIfStateIsKnown()
        {
            int[][] state = new int[10][];
            state[0] = new int[] { -1,  0, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[1] = new int[] { -1,  2, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[2] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[3] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[4] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[5] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[6] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[7] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[8] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            state[9] = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            IList<Coordinate> coords = AttackMapBuilder.Build(state);
            Assert.AreEqual(3, coords.Count);
        }
    }
}
