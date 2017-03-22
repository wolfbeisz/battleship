using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonsterKi;
using BattleSDK;
using System.Collections.Generic;

namespace TestMonsterKi
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Random r = new Random(1000);
            IList<Ship> ships = new List<Ship>(new Ship[] { new Ship(2) });
            Board b = BoardBuilder.BuildRandomBoard(r, ships, 10);
            ;
        }

        [TestMethod]
        public void TestPlaceTwoShips()
        {
            Random r = new Random(1000);
            IList<Ship> ships = new List<Ship>(new Ship[] { new Ship(2), new Ship(2) });
            Board b = BoardBuilder.BuildRandomBoard(r, ships, 10);
            ;
        }

        [TestMethod]
        public void TestPlaceSeveralShips()
        {
            Random r = new Random(1000);
            IList<Ship> ships = new List<Ship>(new Ship[] { new Ship(6), new Ship(6), new Ship(7), new Ship(5), new Ship(2) });
            Board b = BoardBuilder.BuildRandomBoard(r, ships, 10);
            ;
        }
    }
}
