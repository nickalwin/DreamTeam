using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.GroepsChat
{
    public class GroupChat
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int AdminID { get; set; }
        public DateTime Created_at { get; set; }

        public GroupChat(int id, string groupName, string description, int adminId, DateTime createdAt)
        {
            ID = id;
            GroupName = groupName;
            Description = description;
            AdminID = adminId;
            Created_at = createdAt;
        }

        /// <summary>
        /// string with all variables of class for debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ID: {ID}, GroupName: {GroupName}, Description: {Description}, AdminID: {AdminID}, Created_at: {Created_at}";
        }
    }
}
