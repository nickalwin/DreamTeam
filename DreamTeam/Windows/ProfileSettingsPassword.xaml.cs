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
using Model;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for ProfileSettingsPassword.xaml
    /// </summary>
    public partial class ProfileSettingsPassword : Window
    {
        public ProfileSettingsPassword()
        {
            InitializeComponent();


        }

        private void wijzig_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileQuerys.checkPassword(StaticProfile.UserProfile.Email, PasswordInput.Password))
            {
                if (PasswordInput2.Password == PasswordInput3.Password)
                {
                    if (ProfileQuerys.PasswordCorrect(PasswordInput2.Password))
                    {
                        if (ProfileQuerys.ProfileUpdatePassword(StaticProfile.UserProfile.Id, PasswordInput2.Password))
                        {
                            this.Close();
                        }
                        else
                        {
                            err.Content = "Er is iets mis gegaan in de query";
                        }
                    }
                    else
                    {
                        err.Content = "Ingevulde wachtwoord moet minimaal uit 8 tekens bestaan.";
                    }
                }
                else
                {
                    err.Content = "Nieuwe wachtwoord en de heralings wachtwoord komen niet overeen.";
                }
            }
            else
            {
                err.Content = "huidig wachtwoord klopt niet.";
            }
            PageRegistry.profileScreen.UpdateContent();
        }
    }
}
