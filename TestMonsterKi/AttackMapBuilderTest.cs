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
            state[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[1] = new int[] { 0, 2, 2, 0, 0, 0, 0, 0, 0, 0 };
            state[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[4] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[5] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[6] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[7] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[8] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            state[9] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var detectedShips = AttackMapBuilder.DetectShips(state);
            Assert.AreEqual(1, detectedShips.Count);
        }
    }
}
