using Model.Matchmaking;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SQLData
{
    public static class MatchmakingQuerys
    {
        /// <summary>
        /// Get all liked users
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Users> getLikes(int AccountID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            string query =
                "SELECT `ProfileID1`,`MatchDateTime` FROM `Matches` WHERE `ProfileID2` = @AccountID AND `isMatched` = 0";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            List<Users> matches = new List<Users>();

            while (data.Read())
            {
                GamerProfile matchProfile = GamerProfile.getProfile((int)data[0]);

                matches.Add(new Users(matchProfile, (DateTime)data[1], false));
            }
            SQL.CloseConnection(conn);
            return matches;
        }

        /// <summary>
        /// Gets all matches of a user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Users> getMatches(int AccountID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            string query =
                "SELECT DISTINCT `ProfileID1`,`MatchDateTime` FROM `Matches` WHERE `ProfileID2` = @AccountID AND `isMatched` = 1 GROUP BY `ProfileID1` UNION ALL SELECT DISTINCT `ProfileID2`,`MatchDateTime` FROM `Matches` WHERE `ProfileID1` = @AccountID AND `isMatched` = 1 GROUP BY `ProfileID2`";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            List<Users> matches = new List<Users>();

            while (data.Read())
            {
                GamerProfile matchProfile = GamerProfile.getProfile((int)data[0]);

                matches.Add(new Users(matchProfile, (DateTime)data[1], true));
            }
            SQL.CloseConnection(conn);
            return matches;
        }

        /// <summary>
        /// Approves a user who liked user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static bool ApproveLike(int AccountID, int OtherID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "UPDATE `Matches` SET `isMatched`= 1 WHERE `ProfileID1` = @OtherID AND `ProfileID2` = @AccountID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@OtherID", OtherID.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all potentially likeable users.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="SearchCriteria"></param>
        /// <returns></returns>
        public static Queue<GamerProfile> getPotentialLikeable(int AccountID, SearchCriteria SearchCriteria)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@ToxicityLevel", SearchCriteria.toxicityLevel.ToString());
            parameters.Add("@CeilAge", SearchCriteria.InclusiveAgeRange.Item2.ToString());
            parameters.Add("@FloorAge", SearchCriteria.InclusiveAgeRange.Item1.ToString());

            // Select
            string query = "SELECT ID, Leeftijd FROM `Profiel` ";

            // Joins

            // Where
            // ID Can't be the same as user
            query += "WHERE Profiel.ID != @AccountID ";
            // ToxicityLevel should be lower than given in SearchCriteria, with a default of 0
            query += "AND Profiel.ToxicityLevel < @ToxicityLevel ";
            

            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            Queue<GamerProfile> potentialLikeable = new Queue<GamerProfile>();

            while (data.Read())
            {
                int potentialId = (int)data[0];
                DateTime ageDateTime = (DateTime) data[1];
                int potentialage = DateTime.Now.Year - ageDateTime.Year;

                if (potentialage >= SearchCriteria.InclusiveAgeRange.Item1 &&
                    potentialage <= SearchCriteria.InclusiveAgeRange.Item2)
                {

                    List<Game> potentialGames = ProfileQuerys.getGames(potentialId);

                    bool hasMatch = potentialGames.Any(x => SearchCriteria.hasGames.Any(y => y.Name == x.Name));

                    if (hasMatch || SearchCriteria.hasGames.Count() == 0 && !CheckMatched(AccountID, potentialId) &&
                        !UserLiked(AccountID, potentialId) && !UserBlocked(AccountID, potentialId))
                    {
                        // Has all the games
                        potentialLikeable.Enqueue(GamerProfile.getProfile(potentialId));
                    }
                }
            }
            conn.Close();
            return potentialLikeable;
        }

        /// <summary>
        /// Likes a gamer.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static bool LikeGamer(int AccountID, int OtherID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `Matches` (`ProfileID1`, `ProfileID2`, `isMatched`) VALUES(@AccountID, @OtherID, 0)";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@OtherID", OtherID.ToString());
            if (AccountID == OtherID)
            {
                return false;
            }
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                SQL.CloseConnection(connection);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if user liked another user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static bool CheckLiked(int AccountID, int OtherID)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@OtherID", OtherID.ToString());


            string query =
                "SELECT COUNT(*) FROM Matches WHERE ((ProfileID1 = @AccountID AND ProfileID2 = @OtherID) OR (ProfileID1 = @OtherID AND ProfileID2 = @AccountID)) AND isMatched = 0";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }

        /// <summary>
        /// Check if user matched another user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static bool CheckMatched(int AccountID, int OtherID)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@OtherID", OtherID.ToString());


            string query =
                "SELECT COUNT(*) FROM Matches WHERE ((ProfileID1 = @AccountID AND ProfileID2 = @OtherID) OR (ProfileID1 = @OtherID AND ProfileID2 = @AccountID)) AND isMatched = 1";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }

        /// <summary>
        /// Select if user liked another user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static bool UserLiked(int AccountID, int OtherID)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@OtherID", OtherID.ToString());


            string query =
                "SELECT COUNT(*) FROM Matches WHERE (ProfileID1 = @AccountID AND ProfileID2 = @OtherID) AND isMatched = 0";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }

        /// <summary>
        /// Get all blocked users.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static bool UserBlocked(int AccountID, int OtherID)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@OtherID", OtherID.ToString());


            string query =
                "SELECT COUNT(*) FROM Matches WHERE ((ProfileID1 = @AccountID AND ProfileID2 = @OtherID) OR (ProfileID1 = @OtherID AND ProfileID2 = @AccountID)) AND isMatched = -1";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }
    }
}
