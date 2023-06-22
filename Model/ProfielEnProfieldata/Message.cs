using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ProfielEnProfieldata
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
        public DateTime DateTime { get; set; }

        public Message(int senderId, int receiverId, string messageContent, DateTime dateTime)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageContent = messageContent;
            DateTime = dateTime;
        }
        public Message(int senderId, int receiverId, string messageContent, DateTime dateTime, int messageId)
        {
            MessageId = messageId;
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageContent = messageContent;
            DateTime = dateTime;
        }

        public Message(int i, string s, DateTime toDateTime, int ID)
        {
            SenderId = i;
            MessageContent = s;
            DateTime = toDateTime;
            MessageId = ID;
        }
    }
}
