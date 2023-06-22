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
    /// Interaction logic for ProfileSettingsDelete.xaml
    /// </summary>
    public partial class ProfileSettingsDelete : Window
    {
        public ProfileSettingsDelete()
        {
            InitializeComponent();
        }

        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ProfileQuerys.DeleteProfile(StaticProfile.UserProfile.Id);
            PageRegistry.registerScreen = new Register();
            PageRegistry.registerScreen.Show();
            PageRegistry.ProfileSettingsscreen.Close();
            PageRegistry.profileScreen.Close();
            this.Close();
        }
    }
}
