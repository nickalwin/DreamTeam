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
    /// Interaction logic for GroupChatLeave.xaml
    /// </summary>
    public partial class GroupChatLeave : Window
    {
        public int GroupID;
        public GroupChatLeave(int groupID)
        {
            InitializeComponent();
            GroupID = groupID;
        }

        private void DiscardBtnChat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteBtnChat_Click(object sender, RoutedEventArgs e)
        {
            GroepsChatQuerys.RemoveMember(StaticProfile.UserProfile.Id, GroupID);
            GroepsChatQuerys.LeaveGroupForGood(StaticProfile.UserProfile.Id, GroupID);
            this.Close();
            if (PageRegistry.GroupchatWindow != null)
            {
                PageRegistry.GroupchatWindow.Close();
            }
            PageRegistry.profileScreen.Show();
        }
    }
}
