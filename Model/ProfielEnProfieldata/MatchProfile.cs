using Model.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ProfielEnProfieldata
{
    class MatchProfile
    {
        public int Id { get; set; }

        public List<Connection> Connections { get; set; }

        /// <summary>
        /// Gets all the connection the user has.
        /// </summary>
        /// <returns></returns>
        public List<Connection> GetConnections()
        {
            Connections = ConnectionQuerys.getProfileConnections(Id);
            return Connections;
        }
    }
}
