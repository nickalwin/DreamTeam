using Microsoft.Toolkit.Uwp.Notifications;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Controller
{
    public class MessagesTimer
    {
        public System.Timers.Timer Timer;

        public void Initialize()
        {
            Timer = new System.Timers.Timer();
            Timer.Interval = 2000;
            Timer.Elapsed += OnTimedEvent;
            Timer.Start();
        }
        private void OnTimedEvent(object o, ElapsedEventArgs e)
        {
            List<Message> messages = ChatQuerys.GetAllMessegesOfThisProfile();
            foreach(Message message in messages)
            {
                CreateWindowsNotification(message);
            }
        }
        public void CreateWindowsNotification(Message message)
        {
            new ToastContentBuilder()
            .AddArgument("SenderID", message.SenderId)
            .AddText($"{ProfileQuerys.GetProfile(message.SenderId).DisplayName} heeft een bericht gestuurd!")
            .AddText($"{message.MessageContent}")
            .Show();
        }
    }
}