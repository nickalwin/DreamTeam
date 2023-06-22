using Model.ProfielEnProfieldata.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ProfielEnProfieldata
{
    public class Post
    {
        public int PostID;
        public int UserID;
        public List<Media> AttachedMedia;
        public string Beschrijving;
        public DateTime DateTime;

        public Post(int postID, int userID, List<Media> attachedMedia, string beschrijving, DateTime dateTime)
        {
            PostID = postID;
            UserID = userID;
            AttachedMedia = attachedMedia;
            Beschrijving = beschrijving;
            DateTime = dateTime;
        }
    }
}
