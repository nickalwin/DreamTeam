using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.GroepsChat;
using Model.ProfielEnProfieldata;
using MySqlConnector;

namespace Model.SQLData
{
    public static class GroepsChatQuerys
    {

        /// <summary>
        /// get groupchat info based on ID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static GroupChat GetGroupChat(int GroupID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "SELECT `ID`, `GroepNaam`, `Beschrijving`, `BeheerderID`, `Created_at` FROM `GroepChat` WHERE `ID` = @GroupID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroupID", GroupID.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            GroupChat groupChat = null;
            while (data.Read())
            {
                groupChat = new GroupChat((int) data[0], (string) data[1], (string) data[2], (int) data[3],
                    (DateTime) data[4]);
            }
            connection.Close();
            return groupChat;
        }

        /// <summary>
        /// if user wants to completly get rid of a group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupID"></param>
        public static void LeaveGroupForGood(int id, int groupID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "UPDATE `GroepChatMember` SET `INGroup` = '2' WHERE `GroepChatMember`.`ProfielID` = @IDPerson AND `GroepChatMember`.`GroepsChatID` = @IDGroup;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@IDPerson", id.ToString());
            parameters.Add("@IDGroup", groupID.ToString());
            SQL.WriteQuery(connection, query, parameters);
        }

        /// <summary>
        /// checkes if your kicked of group so you can stil see messages but not send them
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static int ISKickedORDeleted(int id, int groupID)
        {
            int result = 0;
            var connection = SQL.OpenConnection();
            string query =
                "SELECT `INGroup` FROM `GroepChatMember` WHERE `ProfielID` = @IDPerson AND `GroepsChatID` = @IDGroup";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@IDPerson", id.ToString());
            parameters.Add("@IDGroup", groupID.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                result = (int)data[0];
            }
            connection.Close();
            return result;
        }

        /// <summary>
        /// Get all groupchats that the user is a part of. for the overview
        /// </summary>
        /// <param name="IDPerson"></param>
        /// <returns></returns>
        public static List<GroupChat> GetPersonsGroepGroupChats(int IDPerson)
        {
            List<GroupChat> groupChats = new List<GroupChat>();
            var connection = SQL.OpenConnection();
            string query =
                "SELECT DISTINCT `GroepChat`.`ID`,`GroepChat`.`GroepNaam`,`GroepChat`.`Beschrijving`,`GroepChat`.`BeheerderID`,`GroepChat`.`Created_at` FROM `GroepChat` JOIN GroepChatMember ON `GroepChat`.`ID` = GroepChatMember.GroepsChatID OR GroepChat.BeheerderID = @IDPerson WHERE GroepChatMember.ProfielID = @IDPerson OR GroepChat.BeheerderID = @IDPerson ORDER BY `GroepChat`.`ID` DESC";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@IDPerson", IDPerson.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                groupChats.Add(new GroupChat((int)data[0],(string)data[1],(string)data[2],(int)data[3],(DateTime)data[4]));
            }
            connection.Close();
            return groupChats;
        }

        /// <summary>
        /// create new groupchat
        /// </summary>
        /// <param name="BeheerderID"></param>
        /// <param name="Beschrijving"></param>
        /// <param name="GroepsNaam"></param>
        /// <returns></returns>
        public static bool CreateGroepChat(int BeheerderID, string Beschrijving, string GroepsNaam, List<int> FriendIDs)
        {
            if (FriendIDs.Count <= 0)
            {
                return false;
            }
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `GroepChat` (`ID`, `GroepNaam`, `Beschrijving`, `BeheerderID`, `Created_at`) VALUES (NULL, @GroepsNaam, @Beschrijving, @BeheerderID, current_timestamp());";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@BeheerderID", BeheerderID.ToString());
            parameters.Add("@Beschrijving", Beschrijving);
            parameters.Add("@GroepsNaam", GroepsNaam);
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                int ID = GetIDGroupChat(BeheerderID, Beschrijving, GroepsNaam);
                parameters.Add("@ID", ID.ToString());
                //groepchatmemberremoved updaten
                var connection2 = SQL.OpenConnection();
                string query2 =
                    "INSERT INTO `GroepChatMemberRemoved` (`ProfielID`, `GroepsChatID`, `Time`, `ISRemoved`) VALUES (@BeheerderID, @ID, current_timestamp(), '0');";
                SQL.WriteQuery(connection2, query2, parameters);


                foreach (int friend in FriendIDs)
                {
                    AddMember(ID, friend);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// get newest groupchatID that has this admin description an groupname (used for CreateGroepChat)
        /// </summary>
        /// <param name="BeheerderID"></param>
        /// <param name="Beschrijving"></param>
        /// <param name="GroepsNaam"></param>
        /// <returns></returns>
        public static int GetIDGroupChat(int BeheerderID, string Beschrijving, string GroepsNaam)
        {
            var connection = SQL.OpenConnection();
            string query = "SELECT `ID` FROM `GroepChat` WHERE `GroepNaam` = @GroepsNaam AND `Beschrijving` = @Beschrijving AND `BeheerderID` = @BeheerderID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@BeheerderID", BeheerderID.ToString());
            parameters.Add("@Beschrijving", Beschrijving);
            parameters.Add("@GroepsNaam", GroepsNaam);
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            int ID = 0;
            while (data.Read())
            {
                ID = (int)data[0];
            }
            connection.Close();
            return ID;
        }

        /// <summary>
        /// get alle members die er niet zijn uit gekicked in een groeps-chat
        /// </summary>
        /// <param name="GroepChatID"></param>
        /// <returns></returns>
        public static List<Profile> GetMembersList(int GroepChatID)
        {
            List<Profile> profiles = new List<Profile>();
            var connection = SQL.OpenConnection();
            string query =
                "SELECT `GroepChatMember`.`ProfielID`, Profiel.DisplayNaam FROM `GroepChatMember` JOIN Profiel ON GroepChatMember.ProfielID = Profiel.ID WHERE `GroepsChatID` = @GroepChatID AND `INGroup` = 1";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                profiles.Add(new Profile((int)data[0], (string)data[1]));
            }
            connection.Close();
            return profiles;
        }

        /// <summary>
        /// kick member uit de groep of iemand leaved
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="GroepChatID"></param>
        /// <returns></returns>
        public static bool RemoveMember(int MemberID, int GroepChatID)
        {
            //check of de beheerder leaved
            bool isBeheerder = false;
            var connection1 = SQL.OpenConnection();
            string query1 =
                "SELECT `BeheerderID` FROM `GroepChat` WHERE `ID` = @GroepChatID AND `BeheerderID` = @MemberID";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            parameters.Add("@MemberID", MemberID.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection1, query1, parameters);
            while (data.Read())
            {
                isBeheerder = true;
            }
            connection1.Close();

            //kick iemand anders dan beheerder
            if (!isBeheerder)
            {
                var connection = SQL.OpenConnection();
                string query =
                    "UPDATE `GroepChatMember` SET `INGroup` = '0' WHERE `GroepChatMember`.`ProfielID` = @MemberID AND `GroepChatMember`.`GroepsChatID` = @GroepChatID;";
                if (SQL.WriteQuery(connection, query, parameters) >= 1)
                {
                    SendLeaveMessage(GroepChatID,MemberID);
                    //groepchatmemberremoved updaten
                    var connection2 = SQL.OpenConnection();
                    string query2 =
                        "INSERT INTO `GroepChatMemberRemoved` (`ProfielID`, `GroepsChatID`, `Time`, `ISRemoved`) VALUES (@MemberID, @GroepChatID, current_timestamp(), '1');";
                    SQL.WriteQuery(connection2, query2, parameters);
                    return true;
                }
                return false;
            }
            //kick de beheerder
            else
            {
                //vind de gene die als langst in de groep zit
                var connection2 = SQL.OpenConnection();
                int idmember = 0;
                string query2 ="SELECT `ProfielID` FROM `GroepChatMember` WHERE `GroepsChatID` = @GroepChatID AND `INGroup` = 1 LIMIT 1";
                MySqlDataReader data2 = SQL.ReadQuery(connection2, query2, parameters);
                while (data2.Read())
                {
                    idmember = (int) data2[0];
                }
                connection2.Close();
                //als er niemand meer in zit
                if (idmember == 0)
                {
                    return DeleteGroepsChat(GroepChatID);
                }
                else //volgende member word beheerder
                {
                    //groepchatmemberremoved updaten
                    var connection3 = SQL.OpenConnection();
                    string query3 =
                        "INSERT INTO `GroepChatMemberRemoved` (`ProfielID`, `GroepsChatID`, `Time`, `ISRemoved`) VALUES (@MemberID, @GroepChatID, current_timestamp(), '1');";
                    SQL.WriteQuery(connection3, query3, parameters);
                    SendLeaveMessage(GroepChatID, MemberID);
                    return ChangeBeheerder(idmember, GroepChatID);
                }
            }
        }

        /// <summary>
        /// send a message to everyone when a user leaves or is kicked
        /// </summary>
        /// <param name="GroepChatID"></param>
        /// <param name="ProfileID"></param>
        public static void SendLeaveMessage(int GroepChatID, int ProfileID)
        {
            var connection = SQL.OpenConnection();
            string gebruiker = ProfileQuerys.GetProfile(ProfileID).DisplayName;
            string message = $"{gebruiker} heeft de groep verlaten.  :(";
            string query =
                "INSERT INTO `GroepChatBerichten` (`GroupChatID`, `VerzenderID`, `Bericht`, `DateTime`) VALUES (@GroepChatID, -1, @Message, current_timestamp());";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            parameters.Add("@Message", message);
            SQL.WriteQuery(connection, query, parameters);
        }

        /// <summary>
        /// nieuwe member toevoegen
        /// </summary>
        /// <param name="GroepChatID"></param>
        /// <param name="ProfileID"></param>
        /// <returns></returns>
        public static bool AddMember(int GroepChatID, int ProfileID)
        {
            //check if there is room left (100max)
            List<Profile> list = GetMembersList(GroepChatID);
            if (list.Count >= 100)
            {
                return false;
            }
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            parameters.Add("@ProfileID", ProfileID.ToString());

            //groepchatmemberremoved updaten
            var connection3 = SQL.OpenConnection();
            string query3 =
                "INSERT INTO `GroepChatMemberRemoved`(`ProfielID`, `GroepsChatID`, `Time`, `ISRemoved`) VALUES (@ProfileID,  @GroepChatID, current_timestamp(), '0')";
            SQL.WriteQuery(connection3, query3, parameters);

            
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `GroepChatMember` (`ProfielID`, `GroepsChatID`, `INGroup`) VALUES (@ProfileID, @GroepChatID, '1');";
            try
            {
                MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            int rows = SQL.WriteQuery(connection, query, parameters);
            if (rows >= 1 || rows == -1)
            {
                return true;
            }
            return false;
            }
            //als gebuiker verwijderd was add hem weer
            catch (MySqlConnector.MySqlException e)
            {
                var connection2 = SQL.OpenConnection();
                string query2 =
                    "UPDATE `GroepChatMember` SET `INGroup` = '1' WHERE `GroepChatMember`.`ProfielID` = @ProfileID AND `GroepChatMember`.`GroepsChatID` =  @GroepChatID;";
                MySqlDataReader data = SQL.ReadQuery(connection2, query2, parameters);
                int rows2 = SQL.WriteQuery(connection2, query2, parameters);
                if (rows2 >= 1 || rows2 == -1)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// message versturen
        /// </summary>
        /// <param name="GroepChatID"></param>
        /// <param name="Message"></param>
        /// <param name="SenderID"></param>
        /// <returns></returns>
        public static bool SendMessage(int GroepChatID, string Message, int SenderID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "INSERT INTO `GroepChatBerichten` (`GroupChatID`, `VerzenderID`, `Bericht`, `DateTime`) VALUES (@GroepChatID, @SenderID, @Message, current_timestamp());";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@SenderID", SenderID.ToString());
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            parameters.Add("@Message", Message);
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// als de groep leeg is word de groep automatisch verwijderd
        /// </summary>
        /// <param name="GroepChatID"></param>
        /// <returns></returns>
        public static bool DeleteGroepsChat(int GroepChatID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            //delete group-chat-removed
            var connection4 = SQL.OpenConnection();
            string query4 = "DELETE FROM `GroepChatMemberRemoved` WHERE `GroepsChatID` = @GroepChatID";
            SQL.WriteQuery(connection4, query4, parameters);
            //delete alle messages nog.
            var connection2 = SQL.OpenConnection();
            string query2 = "DELETE FROM `GroepChatBerichten` WHERE `GroupChatID` = @GroepChatID";
            SQL.WriteQuery(connection2, query2, parameters);
            //delete members
            var connection3 = SQL.OpenConnection();
            string query3 = "DELETE FROM `GroepChatMember` WHERE `GroepChatMember`.`GroepsChatID` = @GroepChatID";
            SQL.WriteQuery(connection3, query3, parameters);
            
            //delete group-chat
            var connection = SQL.OpenConnection();
            string query = "DELETE FROM `GroepChat` WHERE `GroepChat`.`ID` = @GroepChatID";
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// als de beheerder verlaat word er een nieuwe beheerder aangewezen (de gene die als langst in de groep zit)
        /// </summary>
        /// <param name="BeheerderID"></param>
        /// <param name="GroepChatID"></param>
        /// <returns></returns>
        public static bool ChangeBeheerder(int BeheerderID, int GroepChatID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "UPDATE `GroepChat` SET `BeheerderID` = @BeheerderID WHERE `GroepChat`.`ID` = @GroepChatID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@BeheerderID", BeheerderID.ToString());
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                //delete nieuwe beheerder uit normale member list
                var connection2 = SQL.OpenConnection();
                string query2 =
                    "DELETE FROM `GroepChatMember` WHERE `GroepChatMember`.`ProfielID` = @BeheerderID AND `GroepChatMember`.`GroepsChatID` = @GroepChatID";
                SQL.WriteQuery(connection2, query2, parameters);
                return true;
            }
            return false;
        }

        /// <summary>
        /// de groepsnaam aanpassen
        /// </summary>
        /// <param name="GroepNaam"></param>
        /// <param name="GroepChatID"></param>
        /// <returns></returns>
        public static bool ChangeGroepNaam(string GroepNaam, int GroepChatID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "UPDATE `GroepChat` SET `GroepNaam` = @GroepNaam WHERE `GroepChat`.`ID` = @GroepChatID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepNaam", GroepNaam);
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// beschrijving aanpassen
        /// </summary>
        /// <param name="Beschrijving"></param>
        /// <param name="GroepChatID"></param>
        /// <returns></returns>
        public static bool ChangeBeschrijving(string Beschrijving, int GroepChatID)
        {
            var connection = SQL.OpenConnection();
            string query =
                "UPDATE `GroepChat` SET `Beschrijving` = @GroepNaam WHERE `GroepChat`.`ID` = @GroepChatID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepNaam", Beschrijving);
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            if (SQL.WriteQuery(connection, query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// gets all new messages of groupchat
        /// </summary>
        /// <param name="GroupChatID"></param>
        /// <returns></returns>
        public static List<Message> GetGroupChatMessages(int GroupChatID)
        {
            List<Message> messages = new List<Message>();
            DateTime now = DateTime.Now;
            var connection = SQL.OpenConnection();
            string query =
                "SELECT `VerzenderID`, `Bericht`, `DateTime`, `ID` FROM `GroepChatBerichten` WHERE `GroupChatID` = @GroupChatID AND VerzenderID != @personID AND DateTime > @now ORDER BY `DateTime` ASC;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroupChatID", GroupChatID.ToString());
            parameters.Add("@now", now.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("@personID", StaticProfile.UserProfile.Id.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                messages.Add(new Message((int)data[0],(string)data[1], Convert.ToDateTime(data[2]), (int)data[3]));
            }
            connection.Close();
            return messages;
        }

        /// <summary>
        /// gets all groupchat messages when opend
        /// </summary>
        /// <param name="GroupChatID"></param>
        /// <returns></returns>
        public static List<Message> GetAllGroupMessages(int GroupChatID)
        {
            List<Message> messages = new List<Message>();
            DateTime now = DateTime.Now;
            var connection = SQL.OpenConnection();
            string query =
                "SELECT `VerzenderID`, `Bericht`, `DateTime`, `ID` FROM `GroepChatBerichten` WHERE `GroupChatID` = @GroupChatID ORDER BY `DateTime` ASC;";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroupChatID", GroupChatID.ToString());
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            while (data.Read())
            {
                messages.Add(new Message((int)data[0], (string)data[1], Convert.ToDateTime(data[2]), (int)data[3]));
            }
            connection.Close();
            return messages;
        }

        /// <summary>
        /// sends a messages to every member when new user joins group
        /// </summary>
        /// <param name="GroepChatID"></param>
        /// <param name="ProfileID"></param>
        public static void SendNewMemberMessage(int GroepChatID, int ProfileID)
        {
            var connection = SQL.OpenConnection();
            string gebruiker = ProfileQuerys.GetProfile(ProfileID).DisplayName;
            string message = $"{gebruiker} is toegevoegd aan de groep  :)";
            string query =
                "INSERT INTO `GroepChatBerichten` (`GroupChatID`, `VerzenderID`, `Bericht`, `DateTime`) VALUES (@GroepChatID, -1, @Message, current_timestamp());";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroepChatID", GroepChatID.ToString());
            parameters.Add("@Message", message);
            SQL.WriteQuery(connection, query, parameters);
        }
    }
}
