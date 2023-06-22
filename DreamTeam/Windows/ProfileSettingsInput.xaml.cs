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
    /// Interaction logic for ProfileSettingsInput.xaml
    /// </summary>
    public partial class ProfileSettingsInput : Window
    {
        private string soort;
        public ProfileSettingsInput(string soort)
        {
            this.soort = soort;
            InitializeComponent();
            nieuwinput.Content = "Nieuw "+soort;
            title.Content = soort + " Wijzigen";
        }

        private void wijzig2_Click(object sender, RoutedEventArgs e)
        {
            if (soort == "Gamer naam")
            {
                if (Inputfield.Text != "")
                {
                    if (!ProfileQuerys.UsernameExists(Inputfield.Text))
                    {
                        if (ProfileQuerys.ProfileUpdateDisplayNaam(StaticProfile.UserProfile.Id, Inputfield.Text))
                        {
                            StaticProfile.UserProfile.DisplayName = Inputfield.Text;
                            if (PageRegistry.profileScreen == null)
                            {
                                PageRegistry.profileScreen = new Profiel();
                            }
                            PageRegistry.profileScreen.UpdateContent();
                            PageRegistry.ProfileSettingsscreen.name.Content = StaticProfile.UserProfile.DisplayName;
                            this.Close();
                        }
                        else
                        {
                            err2.Content = "er is iets fout gegaan met de query";
                        }
                    }
                    else
                    {
                        err2.Content = "Naam is al in beslag genomen.";
                    }
                    
                }
                else
                {
                    err2.Content = "Vul eerst een Naam in.";
                }
            }

            if (soort == "Email")
            {
                if (!ProfileQuerys.IsValidEmail(Inputfield.Text))
                {
                    if (!ProfileQuerys.EmailExists(Inputfield.Text))
                    {
                        if (ProfileQuerys.ProfielUpdateEmail(StaticProfile.UserProfile.Id, Inputfield.Text))
                        {
                            StaticProfile.UserProfile.Email = Inputfield.Text;
                            if (PageRegistry.profileScreen == null)
                            {
                                PageRegistry.profileScreen = new Profiel();
                            }
                            PageRegistry.profileScreen.UpdateContent();
                            PageRegistry.ProfileSettingsscreen.mail.Content = StaticProfile.UserProfile.Email;
                            this.Close();
                        }
                        else
                        {
                            err2.Content = "er is iets fout gegaan met de query";
                        }
                    }
                    else
                    {
                        err2.Content = "Email is al in beslag genomen.";
                    }

                }
                else
                {
                    err2.Content = "Vul eerst een Geldige Email in.";
                }
            }

            if (soort == "Bio")
            {
                if(ProfileQuerys.ProfileUpdateOmschrijving(StaticProfile.UserProfile.Id, Inputfield.Text))
                {
                    StaticProfile.UserProfile.ProfileText = Inputfield.Text;
                    if (PageRegistry.profileScreen == null)
                    {
                        PageRegistry.profileScreen = new Profiel();
                    }
                    PageRegistry.profileScreen.UpdateContent();
                    PageRegistry.ProfileSettingsscreen.Bio.Content = StaticProfile.UserProfile.ProfileText;
                    this.Close();
                }
                else
                {
                    err2.Content = "er is iets fout gegaan met de query";
                }
            }
            PageRegistry.profileScreen.UpdateContent();
        }
    }
}
