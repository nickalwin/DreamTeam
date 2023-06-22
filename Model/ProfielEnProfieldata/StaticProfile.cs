using Model.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ProfielEnProfieldata
{
    public static class StaticProfile
    {
        /// <summary>
        /// Stores all data from the currently logged in profile.
        /// </summary>
        public static Profile UserProfile { get; set; }

        /// <summary>
        /// Stores all the platforms connected to the user.
        /// </summary>
        public static Dictionary<int, string> Platforms = ConnectionQuerys.GetPlatforms();
    }
}
