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
    /// Interaction logic for ProfileSettingsInput.xaml
    /// </summary>
    public partial class ProfileSettingsInputNationality : Window
    {
        private string soort;
        public ProfileSettingsInputNationality(string soort)
        {
            this.soort = soort;
            InitializeComponent();
            nieuwinput.Content = "Nieuw "+soort;
            title.Content = soort + " Wijzigen";
        }

        private void wijzig3_Click(object sender, RoutedEventArgs e)
        {
            if (soort == "Nationaliteit")
            {
                if (NationalityBox.Text != "")
                {

                    if (ProfileQuerys.ProfielUpdateNationality(StaticProfile.UserProfile.Id, ProfileQuerys.GetNationalityId(NationalityBox.SelectedItem.ToString())))
                    {
                        StaticProfile.UserProfile.Nationaliteit = NationalityBox.SelectedItem.ToString();
                        if (PageRegistry.profileScreen == null)
                        {
                            PageRegistry.profileScreen = new Profiel();
                        }
                        string str = NationalityBox.SelectedItem.ToString();
                        StaticProfile.UserProfile.Nationalities[0] = (Nationality)Enum.Parse(typeof(Nationality), str);
                        PageRegistry.profileScreen.UpdateContent();
                        PageRegistry.ProfileSettingsscreen.nationaliteit.Content = StaticProfile.UserProfile.Nationaliteit;
                        this.Close();
                    }
                    else
                    {
                        err3.Content = "er is iets fout gegaan met de query";
                    }
                }
                else
                {
                    err3.Content = "Vul eerst een Nationaliteit in.";
                }
            }
            PageRegistry.profileScreen.UpdateContent();
        }


    }
}
