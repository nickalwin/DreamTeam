using Model;
using Model.Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestNUnit
{
    public class Game_Tests
    {

        [Test]
        public void gameName()
        {
            Game game = new Game(1);
            Assert.AreEqual("Fortnite", game.Name);
            Game game2 = new Game(0, "unkown");
            Assert.AreEqual("GameName: unkown, Rank: , Speelstijl: GeenVoorkeur", game2.ToString());
        }

        [Test]
        public void rankPrioTest()
        {
            List<Rank> ranks = ProfileQuerys.GetRanksOfGame(1);
            Assert.AreEqual(ranks[0].Priority, 99);
            Assert.AreEqual(ranks[ranks.Count -1].Priority, 1);

            Assert.AreEqual(ranks[0].ToString(), ranks[0].NaamRank);
        }
    }
}