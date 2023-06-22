using Model;
using Model.SQLData;
using NUnit.Framework;
using Model.Matchmaking;
using System.Collections.Generic;
using System;

namespace TestNUnit
{
    [TestFixture]
    class Matching_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void getLikes()
        {
            Assert.IsTrue(MatchmakingQuerys.getLikes(53).Count > 0);
            Assert.IsTrue(MatchmakingQuerys.getLikes(0).Count == 0);
        }

        [Test]
        public void checkLiked()
        {
            Assert.IsTrue(MatchmakingQuerys.CheckLiked(53, 76));
            Assert.IsTrue(MatchmakingQuerys.CheckLiked(76, 53));
            Assert.IsFalse(MatchmakingQuerys.CheckLiked(0,1));
        }
        [Test]
        public void userLiked()
        {
            Assert.IsTrue(MatchmakingQuerys.UserLiked(53, 76));
            Assert.IsFalse(MatchmakingQuerys.UserLiked(76, 53));
            Assert.IsFalse(MatchmakingQuerys.UserLiked(0, 1));
        }

        [Test]
        public void likeGamer()
        {
            GamerProfile profile = new GamerProfile(5, "UnitTests", 18, Gender.Man, 1, ProfileQuerys.GetNationalities(5), ProfileQuerys.GetLanguages(5), ProfileQuerys.getGames(5), "UnitTesten");

            ProfileQuerys.ProfileDeleteAllMatches(1);
            ProfileQuerys.ProfileDeleteAllMatches(20);
            Assert.IsTrue(MatchmakingQuerys.LikeGamer(1,2));
            Assert.IsTrue(MatchmakingQuerys.LikeGamer(20, 21));
            Assert.IsFalse(MatchmakingQuerys.LikeGamer(1, 2));
            Assert.IsFalse(MatchmakingQuerys.LikeGamer(0,0));
        }

        [Test]
        public void zapproveLike()
        {
            Assert.IsTrue(MatchmakingQuerys.ApproveLike(2, 1));
            Assert.IsTrue(GamerProfile.MatchGamer(21, 20));
            Assert.IsFalse(GamerProfile.MatchGamer(38, 20));
            Assert.IsFalse(MatchmakingQuerys.ApproveLike(1, 2));
            Assert.IsFalse(MatchmakingQuerys.ApproveLike(0, 0));
            Assert.IsTrue(ProfileQuerys.ProfileDeleteAllMatches(1));
            Assert.IsTrue(ProfileQuerys.ProfileDeleteAllMatches(20));
        }

        [Test]
        public void getMaches()
        {
            Assert.IsTrue(MatchmakingQuerys.getMatches(53).Count > 0);
            Assert.IsTrue(MatchmakingQuerys.getMatches(0).Count == 0);
        }

        [Test]
        public void checkMatched()
        {
            Assert.IsTrue(MatchmakingQuerys.CheckMatched(53, 51));
            Assert.IsTrue(MatchmakingQuerys.CheckMatched(51, 53));
            Assert.IsFalse(MatchmakingQuerys.CheckMatched(53, 76));
        }

        [Test]
        public void userBlocked()
        {
            Assert.IsTrue(MatchmakingQuerys.UserBlocked(53, 3));
            Assert.IsTrue(MatchmakingQuerys.UserBlocked(3, 53));
            Assert.IsFalse(MatchmakingQuerys.UserBlocked(0, 1));
        }

        [Test]
        public void getPotentialLikeabletest()
        {
            List<Game> selectedGames = new List<Game>();
            SearchCriteria criteria = new SearchCriteria { hasGames = selectedGames, InclusiveAgeRange = new Tuple<int, int>(18, 150), toxicityLevel = 3 };
            Assert.IsTrue(MatchmakingQuerys.getPotentialLikeable(53, criteria).Count > 1);
            criteria = new SearchCriteria { hasGames = selectedGames, InclusiveAgeRange = new Tuple<int, int>(150, 150), toxicityLevel = 0 };
            Assert.IsFalse(MatchmakingQuerys.getPotentialLikeable(53, criteria).Count > 1);
        }
    }
}