using System;
using System.Diagnostics;
using System.Net;
using Model.SQLData;

namespace Model.ProfielEnProfieldata.Images
{
    public class Image : Media
    {
        public Image(string FileLocation)
        {
            fileLocation = FileLocation;
        }


        /// <summary>
        /// Uploads current image as a profile picture to the FTP Server.
        /// Image should be 8MB max.
        /// Profile picture can only be changed for CURRENT LOGGED IN USER,
        /// thus no ProfileID Argument.
        /// </summary>
        public void UploadProfielfoto()
        {
            // Check if correct filesize, 8MB
            if(new System.IO.FileInfo(fileLocation).Length > 8388608)
            {
                throw new FileTooBigException(new System.IO.FileInfo(fileLocation).Length, 8388608);
            }

            Media.RemoveProfilePictures(StaticProfile.UserProfile.Id);

            string uuid = Guid.NewGuid().ToString();
            string address = $"profile-{uuid}.png";
            
            MediaQuerys.UploadImageLocation(address, StaticProfile.UserProfile.Id, ImageType.ProfilePicture);
            Store(fileLocation, "ftp://web0084.zxcs.nl/" + address);
        }

        /// <summary>
        /// Uploads current image as a account picture to the FTP Server.
        /// Image should be 16MB max.
        /// Account pictures can only be added for CURRENT LOGGED IN USER.
        /// thus no ProfileID Argument.
        /// </summary>
        public void UploadAccountPicture(int postID)
        {
            // Check if correct filesize, 16MB
            if (new System.IO.FileInfo(fileLocation).Length > 16777216)
            {
                throw new FileTooBigException(new System.IO.FileInfo(fileLocation).Length, 16777216);
            }

            string uuid = Guid.NewGuid().ToString();
            string address = $"account-{uuid}.png";

            MediaQuerys.UploadImageLocation(address, StaticProfile.UserProfile.Id, ImageType.AccountPicture, postID);
            Store(fileLocation, "ftp://web0084.zxcs.nl/" + address);
        }

        /// <summary>
        /// Uploads current image as a personal picture to the FTP Server.
        /// Image should be 8MB max.
        /// Account pictures can only be uploaded for current logged in user.
        /// thus no ProfileID Argument.
        /// </summary>
        public void UploadPersonalPicture()
        {
            // Check if correct filesize, 16MB
            if (new System.IO.FileInfo(fileLocation).Length > 8388608)
            {
                throw new FileTooBigException(new System.IO.FileInfo(fileLocation).Length, 8388608);
            }

            string uuid = Guid.NewGuid().ToString();
            string address = $"personal-{uuid}.png";

            MediaQuerys.UploadImageLocation(address, StaticProfile.UserProfile.Id, ImageType.PersonalPicture);
            Store(fileLocation, "ftp://web0084.zxcs.nl/" + address);
        }

        
        /// <summary>
        /// Uploads current picture as a group profile picture.
        /// </summary>
        /// <param name="GroupID"></param>
        public void UploadGroupProfilePicture(int GroupID)
        {
            // Check if correct filesize, 8MB
            if (new System.IO.FileInfo(fileLocation).Length > 8388608)
            {
                throw new FileTooBigException(new System.IO.FileInfo(fileLocation).Length, 8388608);
            }

            Media.RemoveGroupProfilePictures(GroupID);

            string uuid = Guid.NewGuid().ToString();
            string address = $"groupprofile-{uuid}.png";

            MediaQuerys.UploadImageLocation(address, GroupID, ImageType.GroupProfilePicture);
            Store(fileLocation, "ftp://web0084.zxcs.nl/" + address);
        }

        /// <summary>
        /// Gets profile picture based on accountID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static Image GetProfilePicture(int accountID)
        {
            string address = MediaQuerys.GetProfilePictureLocation(accountID);

            if(address != "")
            {
                if (!FileExists(Media.GetLocalLocation(address)))
                    Retrieve("ftp://web0084.zxcs.nl/" + address, Media.GetLocalLocation(address));

                return new Image(Media.GetLocalLocation(address));
            }

            return new Image("");
        }

        /// <summary>
        /// Gets group profile picture based on group ID
        /// </summary>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public static Image GetGroupProfilePicture(int GroupID)
        {
            string address = MediaQuerys.GetGroupProfilePictures(GroupID)[0];

            if (address != "")
            {
                if (!FileExists(Media.GetLocalLocation(address)))
                    Retrieve("ftp://web0084.zxcs.nl/" + address, Media.GetLocalLocation(address));

                return new Image(Media.GetLocalLocation(address));
            }

            return new Image("");
        }
    }
}