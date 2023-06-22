using Model.ProfielEnProfieldata;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SQLData
{
    public static class ConnectionQuerys
    {
        /// <summary>
        /// returns all de connections with other apps that user has added
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        static public List<Connection> getProfileConnections(int AccountID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            string query =
                "SELECT `PlatformID`,`IGN`,`Visibility` FROM `ProfielKoppelingen` WHERE `AccountID` = @AccountID";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            List<Connection> Connections = new List<Connection>();

            while (data.Read())
            {
                int PlatformID = (int)data[0];
                string IGN = (string)data[1];
                ConnectionVisibility visibility = (ConnectionVisibility) Enum.Parse(typeof(ConnectionVisibility), (string)data[2]);
                string PlatformName = StaticProfile.Platforms[PlatformID];

                Connection profConn = new Connection(PlatformID, PlatformName, IGN, visibility, AccountID);
                Connections.Add(profConn);
            }
            SQL.CloseConnection(conn);
            return Connections;
        }

        /// <summary>
        /// get all the platforms possible to make a connection with
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetPlatforms()
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string query =
                "SELECT `PlatformID`,`PlatformNaam` FROM `Platforms`";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            Dictionary<int, string> Platforms = new Dictionary<int, string>();

            while (data.Read())
            {
                int PlatformID = (int)data[0];
                string PlatformName = (string)data[1];

                Platforms.Add(PlatformID, PlatformName);
            }
            SQL.CloseConnection(conn);
            return Platforms;
        }

        /// <summary>
        /// sets the visibility of an connection to see for public
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="platformID"></param>
        /// <param name="visibility"></param>
        /// <returns></returns>
        static public bool SetVisibility(int accountID, int platformID, ConnectionVisibility visibility)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string visibilityStr = (string)Enum.GetName(typeof(ConnectionVisibility), visibility);
            string query =
                "UPDATE `ProfielKoppelingen` SET `Visibility`= @Visibility WHERE `PlatformID` = @PlatformID AND `AccountID` = @AccountID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", accountID.ToString());
            parameters.Add("@Visibility", visibilityStr);
            parameters.Add("@PlatformID", platformID.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// creates a connection with a platform
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PlatformID"></param>
        /// <param name="IGN"></param>
        /// <param name="Visibility"></param>
        /// <returns></returns>
        public static bool CreateConn(int AccountID, int PlatformID, string IGN, ConnectionVisibility Visibility)
        {
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `ProfielKoppelingen` (`AccountID`, `PlatformID`, `IGN`, `Visibility`) VALUES (@AccountID, @PlatformID, @IGN, @Visibility);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            parameters.Add("@PlatformID", PlatformID.ToString());
            parameters.Add("@IGN", IGN);
            parameters.Add("@Visibility", Enum.GetName(typeof(ConnectionVisibility), Visibility));
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// checked of een connection met een platform al gemaakt is
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PlatformID"></param>
        /// <returns></returns>
        public static bool CheckExistance(int AccountID, int PlatformID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@PlatformID", PlatformID.ToString());
            parameters.Add("@AccountID", AccountID.ToString());
            string query =
                "SELECT COUNT(*) FROM `ProfielKoppelingen` WHERE `PlatformID` = @PlatformID AND `AccountID` = @AccountID";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            while (data.Read())
            {
                Int64 results = (Int64)data[0];
                conn.Close();
                return results == 0 ? false : true;
            }
            conn.Close();
            return false;
        }

        /// <summary>
        /// removes an connection with a platform
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="PlatformID"></param>
        public static void RemoveConnection(int AccountID, int PlatformID)
        {
            MySqlConnection conn = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@PlatformID", PlatformID.ToString());
            parameters.Add("@AccountID", AccountID.ToString());
            string query =
                "DELETE FROM `ProfielKoppelingen` WHERE `PlatformID` = @PlatformID AND `AccountID` = @AccountID";
            MySqlDataReader data = SQL.ReadQuery(conn, query, parameters);

            conn.Close();
        }

        /// <summary>
        /// changes the username of an user for platform connection (In Game Name)
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="platformID"></param>
        /// <param name="IGN"></param>
        /// <returns></returns>
        static public bool ChangeIGN(int accountID, int platformID, string IGN)
        {
            MySqlConnection conn = SQL.OpenConnection();
            string query =
                "UPDATE `ProfielKoppelingen` SET `IGN`= @IGN WHERE `PlatformID` = @PlatformID AND `AccountID` = @AccountID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", accountID.ToString());
            parameters.Add("@IGN", IGN);
            parameters.Add("@PlatformID", platformID.ToString());
            if (SQL.WriteQuery(conn, query, parameters) >= 1)
            {
                conn.Close();
                return true;
            }
            return false;
        }
    }
}
