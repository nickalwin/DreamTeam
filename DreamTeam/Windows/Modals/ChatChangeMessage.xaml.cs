using Model.ProfielEnProfieldata;
using Model.SQLData;
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

namespace DreamTeam.Windows.Modals
{
    /// <summary>
    /// Interaction logic for ChatChangeMessage.xaml
    /// </summary>
    public partial class ChatChangeMessage : Window
    {
        int ChatID;
        int FriendID;
        bool IsGroupChat;
        public ChatChangeMessage(Grid grid, bool IsGroupChat)
        {
            InitializeComponent();
            grid.Name.Remove(0, 1);
            string[] ChatIdFriendId = grid.Name.Split('i');
            ChatID = int.Parse(ChatIdFriendId[1]);
            FriendID = int.Parse(ChatIdFriendId[2]);
            this.IsGroupChat = IsGroupChat;
        }

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (ChatQuerys.DeleteMessage(ChatID, IsGroupChat))
            {
                if (IsGroupChat)
                {
                    PageRegistry.GroupchatWindow.Close();
                    PageRegistry.GroupchatWindow = new GroupchatWindow(FriendID, GroepsChatQuerys.GetAllGroupMessages(FriendID));
                    PageRegistry.GroupchatWindow.Show();
                    Close();
                }
                else
                {
                    PageRegistry.chat.Close();
                    PageRegistry.chat = new Chat(FriendID, ChatQuerys.GetMessages(FriendID));
                    PageRegistry.chat.Show();
                    Close();
                }
                
            }
        }

        private void ButtonOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (ChatQuerys.ChangeMessage(ChatID, TextInput.Text, IsGroupChat))
            {
                if (IsGroupChat)
                {
                    PageRegistry.GroupchatWindow.Close();
                    PageRegistry.GroupchatWindow = new GroupchatWindow(FriendID, GroepsChatQuerys.GetAllGroupMessages(FriendID));
                    PageRegistry.GroupchatWindow.Show();
                    Close();
                }
                else
                {
                    PageRegistry.chat.Close();
                    PageRegistry.chat = new Chat(FriendID, ChatQuerys.GetMessages(FriendID));
                    PageRegistry.chat.Show();
                    Close();
                }
            }
        }
    }
}
