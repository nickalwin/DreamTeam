using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using NUnit.Framework;

namespace TestNUnit
{
    [TestFixture]
    class ProfileQuerys_Test
    {
        [Test]
        public void ProfileQuerys_GetProfile()
        {
            Profile profile = ProfileQuerys.GetProfile(57);

            Assert.IsTrue(profile.Id.Equals(57));
        }

        [Test]
        public void ProfileQuerys_ProfileGetId()
        {
            int profile = ProfileQuerys.ProfileGetId("test@test.nl");

            Assert.IsTrue(profile.Equals(57));
        }

        [Test]
        public void ProfileQuerys_GetNationalityId()
        {
            int nationalId = ProfileQuerys.GetNationalityId("Afghan");

            Assert.IsTrue(nationalId.Equals(3));
        }

        [Test]
        public void GetNationalities()
        {
            List<Nationality> profile = ProfileQuerys.GetNationalities(57);

            Assert.IsTrue(profile[0].Equals(Nationality.Palauan));
        }

        [Test]
        public void getGames()
        {
            List<Game> games = ProfileQuerys.getGames(57);

            Assert.IsTrue(games[0].Id.Equals(2));
            Assert.IsTrue(games[1].Id.Equals(4));
        }

        [Test]
        public void getgamelist()
        {
            List<Game> games = ProfileQuerys.getgamelist(57);

            Assert.IsTrue(games[0].Name.Equals("League of Legends"));
            Assert.IsTrue(games[0].Rank.NaamRank.Equals("Silver"));
            Assert.IsTrue(games[1].Name.Equals("Dota 2"));
        }

        [Test]
        public void GetLanguages()
        {
            List<Language> Languages = ProfileQuerys.GetLanguages(57);

            Assert.IsTrue(Languages[0].Equals(Language.Manx));
        }

        [Test]
        public void ProfileCheckGames()
        {
            bool first = ProfileQuerys.ProfileCheckGames(57,2);
            bool second = ProfileQuerys.ProfileCheckGames(57, -45);
            Assert.IsTrue(first);
            Assert.IsTrue(!second);
        }

        [Test]
        public void GetRanksOfGame()
        {
            List<Rank> ranks = ProfileQuerys.GetRanksOfGame(1);

            Assert.IsTrue(ranks[0].RankId.Equals(0));
            Assert.IsTrue(ranks[1].RankId.Equals(18));
        }

        [Test]
        public void GetAllUsernamesOfAGame()
        {
            List<string> names = ProfileQuerys.GetAllUsernamesOfAGame(22, 51);

            Assert.IsTrue(names[0].Equals("nickalwin"));
            Assert.IsTrue(names[1].Equals("Poggers"));
            Assert.IsFalse(names[0].Equals("Gamer1234"));
        }
    }
}
