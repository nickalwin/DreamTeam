using Model.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ProfielEnProfieldata
{
    public class Connection
    {
        public int PlatformID { get; set; }
        public string Platform { get; set; }
        public string Username { get; set; }
        public ConnectionVisibility Visibility { get; set; }
        public int AccountID { get; set; }

        public Connection(int platformID, string platform, string username, ConnectionVisibility visibility, int accountID)
        {
            PlatformID = platformID;
            Platform = platform;
            Username = username;
            Visibility = visibility;
            AccountID = accountID;
        }

        /// <summary>
        /// Generates link to platform based on username
        /// </summary>
        /// <returns></returns>
        public string getLink()
        {
            switch (Platform)
            {
                case "Steam":
                    return $"https://steamcommunity.com/id/{Username}";
                case "Discord":
                    return $"https://discordapp.com/users/{Username}";
                default:
                    return Username;
            }
        }

        /// <summary>
        /// Sets visibility of current connection.
        /// </summary>
        /// <param name="visibility"></param>
        public void setVisibility(ConnectionVisibility visibility)
        {
            Visibility = visibility;

            bool res = ConnectionQuerys.SetVisibility(AccountID, PlatformID, Visibility);
        }

        /// <summary>
        /// Makes a connection in the database,
        /// also runs checks.
        /// AccountID is ALWAYS the static ID.
        /// </summary>
        /// <param name="PlatformID"></param>
        /// <param name="IGN"></param>
        public static Connection CreateConnection(int PlatformID, string IGN, ConnectionVisibility visibility)
        {
            if(!ConnectionQuerys.CheckExistance(StaticProfile.UserProfile.Id, PlatformID))
            {
                bool succes = ConnectionQuerys.CreateConn(StaticProfile.UserProfile.Id, PlatformID, IGN, visibility);

                if (succes)
                    return new Connection(PlatformID, StaticProfile.Platforms[PlatformID], IGN, visibility, StaticProfile.UserProfile.Id);

                return null;
            }

            return null;
        }

        /// <summary>
        /// Removes the current connection from the database.
        /// </summary>
        public void RemoveConnection()
        {
            ConnectionQuerys.RemoveConnection(AccountID, PlatformID);
        }
    }   

    
}
