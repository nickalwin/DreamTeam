using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Matchmaking
{
    public class Users
    {
        public GamerProfile MatchedProfile { get; set; }
        public DateTime MatchDateTime { get; set; }
        public bool IsMatched { get; set; }

        public Users(GamerProfile matchedProfile, DateTime matchDateTime, bool isMatched)
        {
            MatchedProfile = matchedProfile;
            MatchDateTime = matchDateTime;
            IsMatched = isMatched;
        }

        /// <summary>
        /// returns all possible gamers to be liked, this means not matched and the user desired criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static Queue<GamerProfile> getPotentials(SearchCriteria criteria)
        {
            return MatchmakingQuerys.getPotentialLikeable(StaticProfile.UserProfile.Id, criteria);
        }
    }
}
