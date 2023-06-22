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
    public partial class ProfileSettingsInputLanguage : Window
    {
        public List<Language> Languages { get; set; }
        public List<Language> DeleteLanguages = new List<Language>();
        //public Language languages { get; set; }
        List<CheckBox> checkBoxes = new List<CheckBox>();


        public ProfileSettingsInputLanguage()
        {
            Languages = new List<Language>();
            InitializeComponent();
            foreach (Language language in Enum.GetValues(typeof(Language)))
            {
                ic.Height += 20;
                CheckBox box = new CheckBox { Content = language.ToString() };
                box.Checked += CheckBox_Checked;
                box.Unchecked += CheckBox_Unchecked;
                checkBoxes.Add(box);
                if (StaticProfile.UserProfile.Languages.Contains(language))
                {
                    box.IsChecked = true;
                }
            }
            ic.ItemsSource = checkBoxes;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            Languages.Add((Language)Enum.Parse(typeof(Language), (string)box.Content));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;

            Languages.RemoveAll(s => Enum.GetName(typeof(Language), s) == (string)box.Content);
        }

        private void wijzig_Click(object sender, RoutedEventArgs e)
        {
            ProfileQuerys.ProfileDeleteAllLanguage(StaticProfile.UserProfile.Id);
            while (Languages.Count > 0)
            {
                foreach (Language lang in Languages)
                {
                    if (ProfileQuerys.ProfileAddLanguage(StaticProfile.UserProfile.Id, ProfileQuerys.ProfilegetLanguageID(lang.ToString())))
                    { DeleteLanguages.Add(lang); }
                }
                foreach (Language lang in DeleteLanguages)
                {
                    Languages.Remove(lang);
                };

            }
            if (Languages.Count == 0)
            {
                StaticProfile.UserProfile.Languages.Clear();
                foreach (Language language in ProfileQuerys.GetLanguages(StaticProfile.UserProfile.Id))
                {
                    StaticProfile.UserProfile.Languages.Add(language);
                }
                this.Close();
            }
            PageRegistry.profileScreen.UpdateContent();
        }


    }
}
