using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for ChatDelete.xaml
    /// </summary>
    public partial class ChatDelete : Window
    {
        private int FriendId { get; set; }

        public ChatDelete(int FriendId)
        {
            this.FriendId = FriendId;
            InitializeComponent();
        }

        private void DiscardBtnChat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteBtnChat_Click(object sender, RoutedEventArgs e)
        {
            ChatQuerys.DeleteMessages(FriendId);
            PageRegistry.chat.Close();
            PageRegistry.chat = new Windows.Chat(FriendId, ChatQuerys.GetMessages(FriendId));
            PageRegistry.chat.Show();
            this.Close();
        }
    }

}
