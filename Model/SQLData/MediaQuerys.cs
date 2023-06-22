using System;
using System.Collections.Generic;
using Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata.Images;
using MySqlConnector;

namespace Model.SQLData
{
    public static class MediaQuerys
    {

        /// <summary>
        /// Uploads the image location on FTP Server to the database.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="accountID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool UploadImageLocation(string address, int accountID, ImageType type)
        {
            string query =
                "INSERT INTO `Media` (`AccountID`, `MediaName`, `ImageType`) VALUES (@accountID, @address, @type);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@accountID", accountID.ToString());
            parameters.Add("@address", address);
            parameters.Add("@type", Enum.GetName(type));
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Uploads the image location connected to a post.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="accountID"></param>
        /// <param name="type"></param>
        /// <param name="postID"></param>
        /// <returns></returns>
        public static bool UploadImageLocation(string address, int accountID, ImageType type, int postID)
        {
            string query =
                "INSERT INTO `Media` (`AccountID`, `MediaName`, `ImageType`, `PostID`) VALUES (@accountID, @address, @type, @postID);";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@accountID", accountID.ToString());
            parameters.Add("@address", address);
            parameters.Add("@type", Enum.GetName(type));
            parameters.Add("@postID", postID.ToString());
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the profile picture location from a profile.
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static string GetProfilePictureLocation(int accountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@id", accountID.ToString());
            string query = "SELECT `MediaName` FROM `Media` WHERE `AccountID` = @id AND ImageType = 'ProfilePicture'";
            
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);
            string FileName = "";
            while (data.Read())
            {
                FileName = (string)data[0];
            }
            connection.Close();
            return FileName;
        }

        /// <summary>
        /// Gets all profile pictures from an account.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<string> GetProfilePictures(int AccountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            string query = "SELECT `MediaName` FROM `Media` WHERE `ImageType` = 'ProfilePicture' AND AccountID = @AccountID";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);

            List<string> profilePics = new List<string>();

            while (data.Read())
            {
                profilePics.Add((string)data[0]);
            }
            connection.Close();
            return profilePics;
        }

        /// <summary>
        /// Gets the group profile picture.
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static List<string> GetGroupProfilePictures(int GroupID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@GroupID", GroupID.ToString());
            string query = "SELECT `MediaName` FROM `Media` WHERE `ImageType` = 'GroupProfilePicture' AND AccountID = @GroupID";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);

            List<string> profilePics = new List<string>();

            while (data.Read())
            {
                profilePics.Add((string)data[0]);
            }
            connection.Close();
            return profilePics;
        }

        /// <summary>
        /// Removes the picture name from the database.
        /// </summary>
        /// <param name="MediaName"></param>
        /// <returns></returns>
        public static bool RemovePictureFromDB(string MediaName)
        {
            string query =
                "DELETE FROM `Media` WHERE `MediaName`= @MediaName";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@MediaName", MediaName);
            MediaName.ToString();
            if (SQL.WriteQuery(SQL.OpenConnection(), query, parameters) >= 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all the profile posts from a user.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static List<Post> GetProfilePosts(int AccountID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@AccountID", AccountID.ToString());
            string query = "SELECT `PostID`,`Beschrijving`,`Date_Created` FROM `Posts` WHERE `UserID` = @AccountID ORDER BY PostID DESC";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);

            List<Post> ProfilePosts = new List<Post>();

            while (data.Read())
            {
                Post post = new Post((int)data[0], AccountID, null, (string)data[1], (DateTime)data[2]);
                post.AttachedMedia = MediaQuerys.GetPostMedia((int)data[0]);
                ProfilePosts.Add(post);
            }
            connection.Close();
            return ProfilePosts;
        }

        /// <summary>
        /// Gets all the media attached to a post.
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        public static List<Media> GetPostMedia(int postID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("@PostID", postID.ToString());
            string query = "SELECT `MediaName`,`ImageType` FROM `Media` WHERE `PostID` = @PostID ORDER BY ID ASC";
            MySqlConnection connection = SQL.OpenConnection();
            MySqlDataReader data = SQL.ReadQuery(connection, query, parameters);

            List<Media> PostMedia = new List<Media>();

            while (data.Read())
            {
                string MediaType = (string)data[1];
                string MediaLocation = (string)data[0];

                if(MediaType.Equals("AccountPicture"))
                {
                    PostMedia.Add(new Image(Media.GetLocalLocation(MediaLocation)));
                }
                else
                {
                    PostMedia.Add(new Video(Media.GetLocalLocation(MediaLocation)));
                }

                if (MediaLocation != "")
                {
                    if (!Media.FileExists(Media.GetLocalLocation(MediaLocation)))
                        Media.Retrieve("ftp://web0084.zxcs.nl/" + MediaLocation, Media.GetLocalLocation(MediaLocation));
                }
            }
            connection.Close();
            return PostMedia;
        }
    }
}