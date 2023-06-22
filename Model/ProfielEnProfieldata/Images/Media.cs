using Model.SQLData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Model.ProfielEnProfieldata.Images
{
    public class Media
    {
        public string fileLocation;

        /// <summary>
        /// Tries to create data folder. If there already is one, does nothing.
        /// </summary>
        public static void TryCreateDataFolder()
        {
            string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DreamTeam");

            if (!System.IO.File.Exists(FolderPath))
            {
                System.IO.Directory.CreateDirectory(FolderPath);
            }
        }
        
        // Gets the local file location based on the filename.
        public static string GetLocalLocation(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DreamTeam/") + fileName;
        }

        // Gets the path of the data folder
        public static string GetDataFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DreamTeam");
        }

        /// <summary>
        /// General store function for media. Can be used to store media from PC to FTP server
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="address"></param>
        public static void Store(string fileLocation, string address)
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential("DreamFTP@naimverboom.nl", "oQyLbemB");
                client.UploadFile(address, WebRequestMethods.Ftp.UploadFile, fileLocation);
            }
        }
        
        /// <summary>
        /// General retrieve function for media. Can be used to retrieve media from FTP server to PC
        /// </summary>
        /// <param name="address"></param>
        /// <param name="LocalLocation"></param>
        public static void Retrieve(string address, string LocalLocation)
        {
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential("DreamFTP@naimverboom.nl", "oQyLbemB");
                client.DownloadFile(address, LocalLocation);
            }
        }

        /// <summary>
        /// Checks if file exists locally
        /// </summary>
        /// <param name="LocalLocation"></param>
        /// <returns></returns>
        public static bool FileExists(string LocalLocation)
        {
            return System.IO.File.Exists(LocalLocation);
        }

        /// <summary>
        /// Clears all the files and folders in the cache/data folder.
        /// </summary>
        public static void ClearCache()
        {
            DirectoryInfo dInfo = new DirectoryInfo(GetDataFolder());

            foreach(var file in dInfo.GetFiles())
            {
                file.Delete();
            }

            foreach(var dir in dInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Returns fileLocation.
        /// </summary>
        /// <returns></returns>
        public string GetFileLocation()
        {
            return fileLocation;
        }

        /// <summary>
        /// Removes all profile picture of an user based on ID from both the FTP server and the database.
        /// </summary>
        /// <param name="ID"></param>
        public static void RemoveProfilePictures(int ID)
        {
            List<string> profilePictures = MediaQuerys.GetProfilePictures(ID);

            foreach(string imageLocation in profilePictures)
            {
                MediaQuerys.RemovePictureFromDB(imageLocation);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://web0084.zxcs.nl/"+imageLocation);
                request.Credentials = new NetworkCredential("DreamFTP@naimverboom.nl", "oQyLbemB");
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
        }

        /// <summary>
        /// Removes all profile picture of an group based on ID from both the FTP server and the database.
        /// </summary>
        /// <param name="GroupID"></param>
        public static void RemoveGroupProfilePictures(int GroupID)
        {
            List<string> profilePictures = MediaQuerys.GetGroupProfilePictures(GroupID);

            foreach (string imageLocation in profilePictures)
            {
                MediaQuerys.RemovePictureFromDB(imageLocation);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://web0084.zxcs.nl/" + imageLocation);
                request.Credentials = new NetworkCredential("DreamFTP@naimverboom.nl", "oQyLbemB");
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
        }
    }

    /// <summary>
    /// Determines the type of Media
    /// </summary>
    public enum ImageType
    {
        ProfilePicture,
        AccountVideo,
        PersonalVideo,
        PersonalPicture,
        AccountPicture,
        GroupProfilePicture
    }

    /// <summary>
    /// Throws when the file is too big for the upload type.
    /// </summary>
    public class FileTooBigException : Exception
    {
        public FileTooBigException(long size, int max)
            : base(String.Format("File of size: {0}, should be {1}", size, max))
        {

        }
    }
}