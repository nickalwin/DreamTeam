using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model.Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata.Images;
using MySqlConnector;

namespace Model.SQLData
{
    public static class ProfileQuerys
    {
        static ProfileQuerys()
        {
        }


        /// <summary>
        /// Registers user in the database. Assumes all checks have been performed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="age"></param>
        /// <param name="gender"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool ProfileRegister(string name, string password, string email, string age, Gender gender, string salt)
        {
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `Profiel` (`DisplayNaam`, `Wachtwoord`, `Email`, `Leeftijd`, `Gender`, `Salt`) VALUES(@name, @password, @email, @age, @gender, @salt)";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@name", name);
            parameters.Add("@password", password);
            parameters.Add("@email", email);
            parameters.Add("@age", age.ToString());
            parameters.Add("@gender", Enum.GetName(gender));
            parameters.Add("@salt", salt);
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                SQL.CloseConnection(connection);
                StaticProfile.UserProfile = GetProfile(ProfileGetId(email));
                StaticProfile.UserProfile.DisplayName = name;
                DateTime realage = Convert.ToDateTime(age);
                StaticProfile.UserProfile.Age = DateTime.Now.Year - realage.Year;
                StaticProfile.UserProfile.Gender = gender;
                StaticProfile.UserProfile.Email = email;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes a profile from the database. Also deletes all the data.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool DeleteProfile(int ID)
        {
            bool deleted = false;
            var connection = SQL.OpenConnection();
            string query =
                "DELETE FROM `Profiel` WHERE `Profiel`.`ID` = @id"; //profiel zelf verwijderen
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", ID.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                deleted = true;
                //delete chats
                ChatQuerys.DeleteMessagesOfProfile(ID);
                //delete matches
                ProfileDeleteAllMatches(ID);
                //delete profielgames
                ProfileDeleteAllGames(ID);
                //delete profiel nationaliteit
                ProfileDeleteAllNationality(ID);
                //delete profieltaal
                ProfileDeleteAllLanguage(ID);
                //delete profile img
                Media.RemoveProfilePictures(ID);
            }

            return deleted;
        }

        /// <summary>
        /// Gets profile ID based on email, which is unique.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static int ProfileGetId(string email)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            string query = "SELECT `ID`FROM `Profiel` WHERE `Email` = @email";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            int id = 0;
            while (data.Read())
            {
                id = (int)data[0];
            }
            connection.Close();
            return id;
        }

        /// <summary>
        /// Gets the ID based on the profile name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int ProfileGetIdName(string name)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@name", name);
            string query = "SELECT `ID`FROM `Profiel` WHERE `DisplayNaam` = @name";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            int id = 0;
            while (data.Read())
            {
                id = (int)data[0];
            }
            connection.Close();
            return id;
        }

        /// <summary>
        /// Gets the profile object based on the ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Profile GetProfile(int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", id.ToString());

            string query =
                "SELECT `ID`,`DisplayNaam`,`Email`,`Leeftijd`,`Gender`,`Omschrijving`,`ToxicityLevel` FROM `Profiel`  WHERE `ID` = @id";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            int idProfile = 0;
            string name = "";
            string email = "";
            Gender gender = Gender.Agender;
            int age = 0;
            string omschrijving = "";
            float toxic = 0;
            while (data.Read())
            {
                idProfile = (int) data[0];
                name = (string) data[1];
                email = (string) data[2];
                DateTime age3 = (DateTime) data[3];
                age = DateTime.Now.Year - age3.Year;
                gender = (Gender) Enum.Parse(typeof(Gender), (string) data[4], true);
                omschrijving = "";
                if (data[5].GetType() != typeof(DBNull))
                {
                    omschrijving = (string) data[5];
                }

                toxic = (float) data[6];
            }
            connection.Close();


            List<Nationality> nationalities = GetNationalities(id);
            List<Game> games = getGames(id);
            List<Language> languages = GetLanguages(id);
            List<Connection> clutches = new List<Connection>();

            return new Profile(idProfile, name, email, age, gender, omschrijving, toxic, languages, games, clutches, nationalities, omschrijving);
        }

        /// <summary>
        /// Gets the nationality ID based on the nationality
        /// </summary>
        /// <param name="nationality"></param>
        /// <returns></returns>
        public static int GetNationalityId(string nationality)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@Nationaliteit", nationality.ToString());

            string query =
                "SELECT NationaliteitID FROM Nationaliteit WHERE Nationaliteit = @Nationaliteit";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            int NationaliteitID = 0;
            while (data.Read())
            {
                NationaliteitID = (int)data[0];
            }
            
            connection.Close();

            return NationaliteitID;

        }

        /// <summary>
        /// Gets all nationalities from an user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Nationality> GetNationalities(int AccountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", AccountID.ToString());

            MySqlConnection connection = SQL.OpenConnection();
            List<Nationality> nationalities = new List<Nationality>();
            string query =
                "SELECT `Nationaliteit` FROM `Nationaliteit` INNER JOIN ProfielNationaliteit ON Nationaliteit.NationaliteitID = ProfielNationaliteit.NationaliteitID WHERE ProfielNationaliteit.ProfileID = @id";
            var data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                nationalities.Add((Nationality)Enum.Parse(typeof(Nationality), (string)data[0], true));
            }
            connection.Close();

            return nationalities;
        }

        /// <summary>
        /// Gets all games based on AccountID.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Game> getGames(int AccountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", AccountID.ToString());

            MySqlConnection connection = SQL.OpenConnection();
            List<Game> games = new List<Game>();
            string query =
                "SELECT Games.Naam, Ranks.RankID, Ranks.Rank, Ranks.RankPriority, ProfielGames.Speelstijl, Games.GameID FROM `ProfielGames` JOIN Games ON ProfielGames.GameID = Games.GameID JOIN Ranks ON ProfielGames.RankID = Ranks.RankID WHERE ProfielGames.ProfileID = @id";
            var data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                string nameGame = (string)data[0];
                int rankid = (int)data[1];
                string rankname = (string)data[2];
                int rankprio = (int)data[3];
                Playstyle playstyle = (Playstyle)Enum.Parse(typeof(Playstyle), (string)data[4], true);
                int gameId = (int)data[5];
                games.Add(new Game(gameId, nameGame, new Rank(rankid, rankname, rankprio), playstyle));
            }
            connection.Close();

            return games;
        }

        /// <summary>
        /// Gets the game list based on AccountID.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Game> getgamelist(int AccountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", AccountID.ToString());
            MySqlConnection connection = SQL.OpenConnection();
            List<Game> games = new List<Game>();
        string query =
                "SELECT Games.Naam, Ranks.Rank FROM ProfielGames JOIN Ranks ON ProfielGames.RankID = Ranks.RankID JOIN Games ON Games.GameID = ProfielGames.GameID WHERE ProfielGames.ProfileID = @id";
        var data = SQL.ReadQuery(connection, query, parameters);
        while (data.Read())
        {
            games.Add(new Game((string)data[0],(string)data[1]));
        }
        connection.Close();
        return games;
        }

        /// <summary>
        /// Gets all the user languages.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Language> GetLanguages(int AccountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", AccountID.ToString());

            MySqlConnection connection = SQL.OpenConnection();
            List<Language> languages = new List<Language>();
            string query =
                "SELECT `Taal` FROM `Taal` INNER JOIN ProfielTaal ON Taal.TaalID = ProfielTaal.TaalID WHERE ProfielTaal.ProfileID = @id";
            var data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                languages.Add((Language)Enum.Parse(typeof(Language), (string)data[0], true));
            }
            connection.Close();

            return languages;
        }

        /// <summary>
        /// Deletes all matches based on AccountID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool ProfileDeleteAllMatches(int ID)
        {
            string query = "DELETE FROM `Matches` WHERE (`ProfileID1` = @id) OR (`ProfileID2` = @id)";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", ID.ToString());
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the toxicity level of an user.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="toxic"></param>
        /// <returns></returns>
        public static bool ProfileUpdateToxicityLevel(int profileId, float toxic)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "UPDATE `Profiel` SET `ToxicityLevel`= @Toxic WHERE `ID` = @ProfileID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@Toxic", toxic.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a game to a user profile, including Rank and Playstyle.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="gameId"></param>
        /// <param name="rankId"></param>
        /// <param name="playstyle"></param>
        /// <returns></returns>
        public static bool ProfileAddGames(int profileId, int gameId, int rankId, Playstyle playstyle)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "INSERT INTO `ProfielGames` (`ProfileID`, `GameID`, `RankID`,`Speelstijl`) VALUES (@ProfileID, @GameId, @RankId, @Speelstijl);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@GameId", gameId.ToString());
            parameters.Add("@RankId", rankId.ToString());
            parameters.Add("@Speelstijl", Enum.GetName(playstyle));
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            else if (SQL.WriteQuery(conn, query, parameters) == -1)
            {
                conn.Close();
                return false;
            }
            return false;
        }

        /// <summary>
        /// Changes game statistics of a player.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="gameId"></param>
        /// <param name="rankId"></param>
        /// <param name="playstyle"></param>
        /// <returns></returns>
        public static bool ProfileChangeGames(int profileId, int gameId, int rankId, Playstyle playstyle)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "UPDATE `ProfielGames` SET `RankID` = @RankId, `Speelstijl` = @Speelstijl WHERE `ProfileID` = @ProfileID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@GameId", gameId.ToString());
            parameters.Add("@RankId", rankId.ToString());
            parameters.Add("@Speelstijl", Enum.GetName(playstyle));
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            else if (SQL.WriteQuery(conn, query, parameters) == -1)
            {
                conn.Close();
                return false;
            }
            return false;
        }

        /// <summary>
        /// Check if a profile has a game.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static bool ProfileCheckGames(int profileId, int gameId)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileId", profileId.ToString());
            parameters.Add("@GameId", gameId.ToString());


            string query =
                "SELECT COUNT(*) FROM ProfielGames WHERE ProfileId = @ProfileId AND GameId = @GameId";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }

        /// <summary>
        /// Adds a game to a user profile without a rank or playstyle.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static bool ProfileAddGamesNoRank(int profileId, int gameId)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "INSERT INTO `ProfielGames` (`ProfileID`, `GameID`, `RankID`) VALUES (@ProfileID, @GameId, @RankId);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@GameId", gameId.ToString());
            parameters.Add("@RankId", "109");
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a certain game from a profile.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static bool ProfileRemoveGame(int profileId, int gameId)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "DELETE FROM `ProfielGames` WHERE `ProfielGames`.`ProfileID` = @ProfileID AND `ProfielGames`.`GameID` = @GameId";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@GameId", gameId.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all games from a profile.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool ProfileDeleteAllGames(int ID)
        {
            string query = "DELETE FROM `ProfielGames` WHERE `ProfileID` = @id";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", ID.ToString());
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all possible ranks of a game.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static List<Rank> GetRanksOfGame(int gameId)
        {
            List<Rank> ranks = new List<Rank>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@gameId", gameId.ToString());
            string query = "SELECT `RankID`,`Rank`,`RankPriority` FROM `Ranks` WHERE `GameID` = @gameId OR GameID = 0 ORDER BY `RankPriority` DESC";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                ranks.Add(new Rank((int)data[0],(string)data[1],(int)data[2]));
            }
            connection.Close();
            return ranks;
        }

        /// <summary>
        /// Updates the summary on a profile.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="omschrijving"></param>
        /// <returns></returns>
        public static bool ProfileUpdateOmschrijving(int id, string omschrijving)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query = "UPDATE `Profiel` SET `Omschrijving`= @Omschrijving WHERE `ID` = @ProfileID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", id.ToString());
            parameters.Add("@Omschrijving", omschrijving);
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a language to a profile.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public static bool ProfileAddLanguage(int profileId, int languageID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query = "INSERT INTO `ProfielTaal` (`ProfileID`, `TaalID`) VALUES (@ProfileID, @TaalID);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@TaalID", languageID.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the language ID based on a language string.
        /// </summary>
        /// <param name="LanguageName"></param>
        /// <returns></returns>
        public static int ProfilegetLanguageID(string LanguageName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@Taal", LanguageName);
            string query =
                "SELECT `TaalID` FROM `Taal` WHERE `Taal` = @Taal";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                int id = (int)data[0];
                connection.Close();
                return id;
            }
            return -1;
        }

        /// <summary>
        /// Deletes all languages from a profile.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool ProfileDeleteAllLanguage(int ID)
        {
            string query = "DELETE FROM `ProfielTaal` WHERE `ProfileID` = @id";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", ID.ToString());
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates nationality from a profile.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nationality"></param>
        /// <returns></returns>
        public static bool ProfielUpdateNationality(int id, int nationality)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "UPDATE `ProfielNationaliteit` SET `NationaliteitID`= @Nationaliteit WHERE `ProfileID` = @ProfileID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", StaticProfile.UserProfile.Id.ToString());
            parameters.Add("@Nationaliteit", nationality.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a nationality to a profile.
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="nationality"></param>
        /// <returns></returns>
        public static bool ProfileAddNationality(int profileId, int nationality)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "INSERT INTO `ProfielNationaliteit` (`ProfileID`, `NationaliteitID`) VALUES (@ProfileID, @Nationality);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileID", profileId.ToString());
            parameters.Add("@Nationality", nationality.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes all nationalities from a profile.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool ProfileDeleteAllNationality(int ID)
        {
            string query = "DELETE FROM `ProfielNationaliteit` WHERE `ProfileID` = @id";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", ID.ToString());
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if current username already exists.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public static bool UsernameExists(string Username)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@username", Username);

            string query =
                "SELECT COUNT(*) FROM Profiel WHERE DisplayNaam = @username";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }

        /// <summary>
        /// Checks if current email already exists.
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool EmailExists(string Email)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@email", Email);

            string query =
                "SELECT COUNT(*) FROM Profiel WHERE Email = @email";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            data.Read();
            Int64 result = (Int64)data[0];
            conn.Close();
            return result > 0;
        }

        /// <summary>
        /// Checks if password is correct length.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool PasswordCorrect(string password)
        {
            if (password.Length < 8)
                return false;

            return true;
        }

        /// <summary>
        /// Updates the password of a profile.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ProfileUpdatePassword(int id, string password)
        {
            byte[] salt = Profile.RandomString(16);
            byte[] saltedPass = Profile.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), salt);

            MySqlConnection conn = SQL.OpenConnection();
            string query = "UPDATE `Profiel` SET `Wachtwoord` = @password, `Salt` = @Salt WHERE `Profiel`.`ID` = @id;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", id.ToString());
            parameters.Add("@Salt", Convert.ToBase64String(salt));
            parameters.Add("@password", Convert.ToBase64String(saltedPass));
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the display name. Doesn't check for current existence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ProfileUpdateDisplayNaam(int id, string name)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query = "UPDATE `Profiel` SET `DisplayNaam` = @name WHERE `Profiel`.`ID` = @id;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", id.ToString());
            parameters.Add("@name", name);
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the display name. Doesn't check for current existence.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ProfielUpdateEmail(int id, string name)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query = "UPDATE `Profiel` SET `Email` = @email WHERE `Profiel`.`ID` = @id";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", id.ToString());
            parameters.Add("@email", name);
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Regex to check for E-mail validity.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            return !match.Success;
        }

        /// <summary>
        /// Checks if password is correct.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool checkPassword(string email, string password)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);

            string query =
                "SELECT Wachtwoord, Salt, ID FROM Profiel WHERE Email = @email";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            while (data.Read())
            {
                byte[] salt = (byte[])data[1];

                string base64salt = Encoding.UTF8.GetString(salt);
                byte[] saltba = Convert.FromBase64String(base64salt);
                byte[] saltedPass = Profile.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), saltba);
                string base64string = Encoding.UTF8.GetString((byte[])data[0]);
                byte[] pw = Convert.FromBase64String(base64string);


                if (CompareByteArrays(saltedPass, pw))
                {
                    StaticProfile.UserProfile = GetProfile((int)data[2]);
                    SQL.CloseConnection(conn);
                    return true;
                }
                else
                {
                    SQL.CloseConnection(conn);
                    return false;
                }
            }
            SQL.CloseConnection(conn);
            return false;
        }

        /// <summary>
        /// Gets all usernames of friends that have the a certain game.
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="ProfileID"></param>
        /// <returns></returns>
        public static List<string> GetAllUsernamesOfAGame(int gameId, int ProfileID)
        {
            List<string> namesList = new List<string>();
            MySqlConnection connection = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GameId", gameId.ToString());
            parameters.Add("@ProfileId", ProfileID.ToString());
            string query =
                "SELECT DISTINCT Profiel.DisplayNaam FROM Profiel JOIN ProfielGames ON Profiel.ID = ProfielGames.ProfileID JOIN Matches ON((`ProfileID2` = Profiel.ID AND `ProfileID1` = @ProfileId) OR(`ProfileID1` = Profiel.ID AND `ProfileID2` = @ProfileId) )AND `isMatched` = 1 WHERE ProfielGames.GameID = @GameId";
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                namesList.Add((string)data[0]);
            }

            return namesList;
        }

        /// <summary>
        /// Get all friends that the current user and another profile have in common.
        /// </summary>
        /// <param name="otherID"></param>
        /// <returns></returns>
        public static List<string> GetincommonFriends(int otherID)
        {
            //prepare
            MySqlConnection connection1 = SQL.OpenConnection();
            List<string> namesList = new List<string>();
            List<int> ints1 = new List<int>();
            List<int> ints2 = new List<int>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@Id", StaticProfile.UserProfile.Id.ToString());
            parameters.Add("@Idother", otherID.ToString());
            //vrienden ophalen van jouw profiel
            string query1 =
                "SELECT `ProfileID1` FROM `Matches` WHERE `isMatched` = 1 AND `ProfileID2` = @Id AND `ProfileID1` != @Id UNION SELECT `ProfileID2`FROM Matches WHERE `isMatched` = 1 AND `ProfileID1` = @Id AND `ProfileID2` != @Id";
            MySqlDataReader data = SQL.ReadQuery(connection1, query1, parameters);
            while (data.Read())
            {
                ints1.Add((int)data[0]);
                
            }
            connection1.Close();
            //vrienden ophalen van profiel die je bezoekt
            MySqlConnection connection2 = SQL.OpenConnection();
            string query2 =
                "SELECT `ProfileID1` FROM `Matches` WHERE `isMatched` = 1 AND `ProfileID2` = @Idother AND `ProfileID1` != @Idother UNION SELECT `ProfileID2`FROM Matches WHERE `isMatched` = 1 AND `ProfileID1` = @Idother AND `ProfileID2` != @Idother";
            data = SQL.ReadQuery(connection2, query2, parameters);
            while (data.Read())
            {
                ints2.Add((int)data[0]);
            }
            connection2.Close();
            // voor elke vriend van de bezochte persoon, kijk of jij die ook hebt in jouw lijst. zo ja voeg hem toe in de lijst die je daarna returned
            foreach (int id in ints2)
            {
                if (ints1.Contains(id))
                {
                    namesList.Add(GetProfile(id).DisplayName);
                }
            }
            return namesList;
        }

        /// <summary>
        /// Gets all the possible games
        /// </summary>
        /// <returns></returns>
        public static List<Game> GetAllGames()
        {
            List<Game> games = new List<Game>();
            string query = "SELECT `GameID`,`Naam` FROM `Games`";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query);
            while (data.Read())
            {
                games.Add(new Game((int)data[0],(string)data[1]));
            }
            connection.Close();
            return games;
        }

        /// <summary>
        /// Gets the ID of a game based on its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetIdGame(string name)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@name", name);
            string query =
                "SELECT `GameID` FROM `Games` WHERE `Naam` = @name";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            int Id = 0;
            while (data.Read())
            {
                Id= (int)data[0];
            }
            SQL.CloseConnection(conn);
            return Id;
        }

        /// <summary>
        /// Gets the game info.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string infoGame(int id)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", id.ToString());
            string query =
                "SELECT Naam FROM Games WHERE GameID = @id";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            string info= "";
            while (data.Read())
            {
                info = (string)data[0];
            }
            SQL.CloseConnection(conn);
            if (info == "")
            {
                return "unknown";
            }
            return info;
        }

        /// <summary>
        /// Compare bytes, for password.
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a game object of provided ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Game GetGame(int id)
        {
            MySqlConnection conn = SQL.OpenConnection();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", id.ToString());
            string query =
                "SELECT Naam FROM Games WHERE GameID = @id";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            while (data.Read())
            {
                string naam = (string)data[0];
                SQL.CloseConnection(conn);
                return new Game(id, naam);
            }
            return null;
        }

        /// <summary>
        /// Makes a post in the database with a description.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="Beschrijving"></param>
        /// <returns></returns>
        public static int MakeProfilePost(int AccountID, string Beschrijving)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "INSERT INTO `Posts` (`UserID`, `Beschrijving`) VALUES (@AccountID, @Beschrijving);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@Beschrijving", Beschrijving);
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
            }

            conn = SQL.OpenConnection();

            parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            query =
                "SELECT PostID FROM Posts WHERE UserID = @AccountID ORDER BY PostID DESC";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            while (data.Read())
            {
                int postID= (int)data[0];
                SQL.CloseConnection(conn);
                return postID;
            }
            return -1;
        }

        /// <summary>
        /// Gets display name based on AccountID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetName(int id)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string query =
                "SELECT DisplayNaam FROM Profiel WHERE ID = @id";
            parameters.Add("@id", id.ToString());
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);
            while (data.Read())
            {
                string naam = (string)data[0];
                SQL.CloseConnection(conn);
                return naam;
            }
            return null;
        }
    }
}
