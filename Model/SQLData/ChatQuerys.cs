using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ProfielEnProfieldata;
using MySqlConnector;

namespace Model.SQLData
{
    public static class ChatQuerys
    {
        static ChatQuerys()
        {
        }


        /// <summary>
        /// makes a connection with the database which saves a message of an user
        /// </summary>
        /// <param name="ReceiverId"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static bool SendMessage(int ReceiverId, string Message)
        {
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `Chats` (`VerzenderId`, `OntvangerId`, `Bericht`) VALUES (@SenderId, @ReceiverId, @Message);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@SenderId", StaticProfile.UserProfile.Id.ToString());
            parameters.Add("@ReceiverId", ReceiverId.ToString());
            parameters.Add("@Message", Message);
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                connection.Close();
                return true;
            }
            return false;
        }
        /// <summary>
        /// makes an connection with the database which returns a message with an friend if there is a new message
        /// </summary>
        /// <param name="FriendId"></param>
        /// <returns></returns>
        public static List<Message> GetMessagesOfFriend(int FriendId)
        {
            DateTime now = DateTime.Now;
            var connection = SQL.OpenConnection();
            List<Message> messages = new List<Message>();
            string query = "SELECT `VerzenderId`,`OntvangerId`,`Bericht`,`DateTime`, `ChatID` FROM `Chats` WHERE (`VerzenderId` = @ReceiverId AND `OntvangerId` = @SenderId) AND DateTime > @now ORDER BY `DateTime` ASC";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@SenderId", StaticProfile.UserProfile.Id.ToString());
            parameters.Add("@ReceiverId", FriendId.ToString());
            parameters.Add("@now", now.AddSeconds(-2).ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                messages.Add(new Message((int)data[0],(int)data[1],(string)data[2],Convert.ToDateTime(data[3]),(int)data[4]));
            }
            connection.Close();
            return messages;
        }

        /// <summary>
        /// Read out all message's from 1 to 1 person
        /// </summary>
        /// <param name="OtherprofileId"></param>
        /// <returns></returns>
        public static List<Message> GetMessages(int OtherprofileId)
        {
            var connection = SQL.OpenConnection();
            List<Message> messages = new List<Message>();
            string query = "SELECT `VerzenderId`,`OntvangerId`,`Bericht`,`DateTime`, `ChatID` FROM `Chats` WHERE (`VerzenderId` = @SenderId AND `OntvangerId` = @ReceiverId) OR (`VerzenderId` = @ReceiverId AND `OntvangerId` = @SenderId) ORDER BY `DateTime` ASC";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@SenderId", StaticProfile.UserProfile.Id.ToString());
            parameters.Add("@ReceiverId", OtherprofileId.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                messages.Add(new Message((int)data[0], (int)data[1], (string)data[2], Convert.ToDateTime(data[3]), (int)data[4]));
            }
            connection.Close();
            return messages;
        }
        /// <summary>
        /// reads when user gets a message from whomever
        /// </summary>
        /// <returns></returns>
        public static List<Message> GetAllMessegesOfThisProfile()
        {
            DateTime now = DateTime.Now;

            var connection = SQL.OpenConnection();
            List<Message> messages = new List<Message>();
            string query =
                "SELECT * FROM Chats WHERE DateTime > @now AND OntvangerId = @profile ORDER BY DateTime ASC";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@now", now.AddSeconds(-2).ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("@profile", StaticProfile.UserProfile.Id.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                messages.Add(new Message((int)data[0], (int)data[1], (string)data[2], Convert.ToDateTime(data[3]), (int)data[4]));
            }
            connection.Close();
            return messages;
        }

        /// <summary>
        /// deletes all messages between user and other gamer
        /// </summary>
        /// <param name="OtherprofileId"></param>
        /// <returns></returns>
        public static bool DeleteMessages(int OtherprofileId)
        {
            var connection = SQL.OpenConnection();
            List<Message> messages = new List<Message>();
            string query = "DELETE FROM `Chats` WHERE (`VerzenderId` = @SenderId AND `OntvangerId` = @ReceiverId) OR (OntvangerId = @SenderId AND `VerzenderId` = @ReceiverId)";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@SenderId", StaticProfile.UserProfile.Id.ToString());
            parameters.Add("@ReceiverId", OtherprofileId.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// deletes all messages of an user
        /// </summary>
        /// <param name="ProfileID"></param>
        public static void DeleteMessagesOfProfile(int ProfileID)
        {
            var connection = SQL.OpenConnection();
            List<Message> messages = new List<Message>();
            string query = "DELETE FROM `Chats` WHERE (`VerzenderId` = @ProfileId) OR (OntvangerId = @ProfileId)";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@ProfileId", ProfileID.ToString());
            SQL.WriteQuery(connection, query, parameters);
        }

        /// <summary>
        /// delete one specific message user has send
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="isGroupChat"></param>
        /// <returns></returns>
        public static bool DeleteMessage(int MessageId, bool isGroupChat)
        {
            var connection = SQL.OpenConnection();
            string query = (isGroupChat) ? "DELETE FROM GroepChatBerichten WHERE `ID` = @MessageID;" : "DELETE FROM Chats WHERE `ChatID` = @MessageID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@MessageID", MessageId.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// changes a specific message to a message the user wants
        /// </summary>
        /// <param name="MessageId"></param>
        /// <param name="message"></param>
        /// <param name="isGroupChat"></param>
        /// <returns></returns>
        public static bool ChangeMessage(int MessageId, string message, bool isGroupChat)
        {
            var connection = SQL.OpenConnection();
            string query = isGroupChat ? "UPDATE GroepChatBerichten SET Bericht = @message WHERE ID = @MessageID;" : "UPDATE Chats SET Bericht = @message WHERE ChatID = @MessageID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@message", message);
            parameters.Add("@MessageID", MessageId.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// gets last auto incremented groupchat message id
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="isGroupChat"></param>
        /// <returns></returns>
        public static int GetLastAiId(int profileId, bool isGroupChat)
        {
            int id = -1;
            var connection = SQL.OpenConnection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string query = isGroupChat ? "SELECT MAX(ID) FROM GroepChatBerichten WHERE VerzenderID = @profileID;" : "SELECT MAX(ChatID) FROM Chats WHERE VerzenderId = @profileId;";
            parameters.Add("@profileId", profileId.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                id = (int)data[0];
            }
            connection.Close();
            return id;
        }
    }
}
