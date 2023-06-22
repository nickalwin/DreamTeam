using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Matchmaking
{
    public class Messages
    {
        private int Sender { get; set; }
        private int Receiver { get; set; }
        private string Message { get; set; }
        private DateTime DateTime { get; set; }

        public Messages(int sender, int receiver, string message, DateTime dateTime)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            DateTime = dateTime;
        }

        public Messages(int sender, string message, DateTime dateTime)
        {
            Sender = sender;
            Receiver = 0;
            Message = message;
            DateTime = dateTime;
        }
    }
}
