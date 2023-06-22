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
using DreamTeam.Windows.Modals;
using Model.ProfielEnProfieldata;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for ProfileSettings.xaml
    /// </summary>
    public partial class ProfileSettings : Window
    {

        public ProfileSettings()
        {
            InitializeComponent();
            try
            {
                name.Content = StaticProfile.UserProfile.DisplayName;
                mail.Content = StaticProfile.UserProfile.Email;
                Bio.Content = StaticProfile.UserProfile.ProfileText;
                nationaliteit.Content = StaticProfile.UserProfile.Nationalities[0];
                CountLanguages.Content = StaticProfile.UserProfile.Languages.Count.ToString();
            }
            catch (NullReferenceException)
            {
                name.Content = "Unknown";
                mail.Content = "Unknown";
                Bio.Content = "Unknown";
            }
        } 

        private void ProfileSettingsPassword_Click(object sender, RoutedEventArgs e)
        {
            Windows.ProfileSettingsPassword window = new ProfileSettingsPassword();
            window.Show();
        }

        private void ProfilePictureChange_Click(object sender, RoutedEventArgs e)
        {
                Modals.ChangeProfilePicture cpp = new Modals.ChangeProfilePicture();
                cpp.Show();
        }

        private void nameWijzig_Click(object sender, RoutedEventArgs e)
        {
            Windows.ProfileSettingsInput window = new ProfileSettingsInput("Gamer naam");
            window.Show();
        }

        private void mailWijzig_Click(object sender, RoutedEventArgs e)
        {
            Windows.ProfileSettingsInput window = new ProfileSettingsInput("Email");
            window.Show();
        }

        private void deleteProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileSettingsDelete window = new ProfileSettingsDelete();
            window.Show();
        }

        private void BioWijzig_Click(object sender, RoutedEventArgs e)
        {
            Windows.ProfileSettingsInput window = new ProfileSettingsInput("Bio");
            window.Show();
        }

        private void NationaliteitWijzig_Click(object sender, RoutedEventArgs e)
        {
            Windows.ProfileSettingsInputNationality window = new ProfileSettingsInputNationality("Nationaliteit");
            window.Show();
        }

        private void LanguageChange_Click(object sender, RoutedEventArgs e)
        {
            ProfileSettingsInputLanguage window = new ProfileSettingsInputLanguage();
            window.Show();
        }

        private void Koppelingen_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.Koppelingen = new Koppelingen();
            PageRegistry.Koppelingen.Show();
        }
    }
}
