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
    public class GamerProfile
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public float ToxicityLevel { get; set; }
        public List<Nationality> Nationalities { get; set; }
        public List<Language> Languages { get; set; }
        public List<Game> Games { get; set; }
        public string Omschrijving { get; set; }

        public GamerProfile(int id, string displayName, int age, Gender gender, float toxicityLevel, List<Nationality> nationalities, List<Language> languages, List<Game> games, string omschrijving)
        {
            Id = id;
            DisplayName = displayName;
            Age = age;
            Gender = gender;
            ToxicityLevel = toxicityLevel;
            Nationalities = nationalities;
            Languages = languages;
            Games = games;
            Omschrijving = omschrijving;
        }

        /// <summary>
        /// Get profile information of other gamer to view the gamers profilepage
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>other gamerprofile</returns>
        public static GamerProfile getProfile(int ID)
        {
            var tempProfile = ProfileQuerys.GetProfile(ID);

            GamerProfile profile = new GamerProfile(tempProfile.Id, tempProfile.DisplayName, tempProfile.Age, tempProfile.Gender, tempProfile.ToxicityLevel, tempProfile.Nationalities, tempProfile.Languages, tempProfile.Games, tempProfile.Omschrijving);
            tempProfile = null;

            return profile;
        }

        /// <summary>
        /// when matching you kan like a gamer, a query is pushed to the database to like. if gamer is already liked they will become a match
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public bool LikeGamer(int ProfileId)
        {
            if (!MatchmakingQuerys.CheckLiked(ProfileId, Id))
            {
                if (MatchmakingQuerys.LikeGamer(ProfileId, Id))
                {
                    return true;
                }
            }
            else
            {
                if (MatchGamer(ProfileId, Id))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// if gamer is liked he can accept and become an match
        /// </summary>
        /// <param name="Profile"></param>
        /// <param name="GamerId"></param>
        /// <returns></returns>
        public static bool MatchGamer(int Profile, int GamerId)
        {
            if (MatchmakingQuerys.ApproveLike(Profile, GamerId))
            {
                return true;
            }

            return false;
        }
    }
}
