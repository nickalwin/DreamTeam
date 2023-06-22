using Model.SQLData;
using System;

namespace Model.ProfielEnProfieldata.Images
{
    public class Video : Media
    {
        public Video(string fileLocation)
        {
            this.fileLocation = fileLocation;
        }

        /// <summary>
        /// Uploads current video as a account video to the FTP Server.
        /// Video should be 64MB max.
        /// Account videos can only be added for CURRENT LOGGED IN USER.
        /// thus no ProfileID Argument.
        /// </summary>
        public void UploadAccountVideo(int postID)
        {

            // Check if correct filesize, 64MB
            if (new System.IO.FileInfo(fileLocation).Length > 67108864)
            {
                throw new FileTooBigException(new System.IO.FileInfo(fileLocation).Length, 67108864);
            }

            string uuid = Guid.NewGuid().ToString();
            string address = $"account-{uuid}.mp4";

            MediaQuerys.UploadImageLocation(address, StaticProfile.UserProfile.Id, ImageType.AccountVideo, postID);
            Store(fileLocation, "ftp://web0084.zxcs.nl/" + address);
        }

        /// <summary>
        /// Uploads current video as a personal video to the FTP Server.
        /// Video should be 64MB max.
        /// Account videos can only be uploaded for current logged in user.
        /// thus no ProfileID Argument.
        /// </summary>
        public void UploadPersonalVideo()
        {
            // Check if correct filesize, 64MB
            if (new System.IO.FileInfo(fileLocation).Length > 67108864)
            {
                throw new FileTooBigException(new System.IO.FileInfo(fileLocation).Length, 67108864);
            }

            string uuid = Guid.NewGuid().ToString();
            string address = $"personal-{uuid}.mp4";

            MediaQuerys.UploadImageLocation(address, StaticProfile.UserProfile.Id, ImageType.PersonalVideo);
            Store(fileLocation, "ftp://web0084.zxcs.nl/" + address);
        }

    }
}