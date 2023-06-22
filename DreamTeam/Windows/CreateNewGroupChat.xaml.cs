using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Win32;
using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for CreateNewGroupChat.xaml
    /// </summary>
    public partial class CreateNewGroupChat : Window
    {
        OpenFileDialog fileDialog;
        string FileLocation = "";

        List<CheckBox> checkBoxes = new List<CheckBox>();
        public List<Users> Friends = new List<Users>();

        public CreateNewGroupChat()
        {
            InitializeComponent();

            fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".png"; // Required file extension 
            fileDialog.Filter = "Image Document (.png)|*.png"; // Optional file extensions

            fileDialog.FileOk += FileDialog_FileOk;

            //checkboxes van de vriendenlijst
            Friends = MatchmakingQuerys.getMatches(StaticProfile.UserProfile.Id);
            foreach (Users friend in Friends)
            {
                friendlist.Height += 20;
                CheckBox box = new CheckBox { Content = friend.MatchedProfile.DisplayName };
                checkBoxes.Add(box);
            }
            friendlist.ItemsSource = checkBoxes;
        }

        private void FileDialog_FileOk(object sender, CancelEventArgs e)
        {
            FileLocation = fileDialog.FileName;
            PicturePreview.Source = new Uri(FileLocation, UriKind.Absolute);
        }

        private void GroepNaamInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GroepNaam.Text == "Groepnaam")
            {
                GroepNaam.Text = "";
            }
        }

        private void BeschrijvingInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Beschrijving.Text == "Beschrijving")
            {
                Beschrijving.Text = "";
            }
        }

        private void Beschrijving_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Beschrijving.Text))
            {
                Beschrijving.Text = "Beschrijving";
            }
        }

        private void GroepNaam_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GroepNaam.Text))
            {
                GroepNaam.Text = "GroepNaam";
            }
        }

        private void Aanmaken_Click(object sender, RoutedEventArgs e)
        {
            string name = GroepNaam.Text;
            if (name == "Groepnaam") { name = ""; }
            string beschrijving = Beschrijving.Text;
            if (beschrijving == "Beschrijving") { beschrijving = ""; }
            //vrienden toevoegen die gechecked zijn
            List<int> friendsIDs = new List<int>();
            foreach (CheckBox box in friendlist.ItemsSource)
            {
                if ((bool)box.IsChecked)
                {
                    foreach (Users user in Friends)
                    {
                        if (user.MatchedProfile.DisplayName.Equals((string)box.Content))
                        {
                            friendsIDs.Add(user.MatchedProfile.Id);
                        }
                    }
                }
            }

            if (friendsIDs.Count > 0)
            {
                if (name != "" && name != "GroepNaam")
                {
                    if (GroepsChatQuerys.CreateGroepChat(StaticProfile.UserProfile.Id, beschrijving, name, friendsIDs))
                    {
                        int ID = GroepsChatQuerys.GetIDGroupChat(StaticProfile.UserProfile.Id, beschrijving, name);

                        if (!string.IsNullOrWhiteSpace(FileLocation))
                        {
                            Model.ProfielEnProfieldata.Images.Image img = new Model.ProfielEnProfieldata.Images.Image(fileDialog.FileName);
                            img.UploadGroupProfilePicture(ID);
                            Model.ProfielEnProfieldata.Images.Image.GetGroupProfilePicture(ID);
                        }

                        PageRegistry.GroupchatWindowOverview.Close();
                        PageRegistry.GroupchatWindow = new GroupchatWindow(ID, GroepsChatQuerys.GetAllGroupMessages(ID));
                        PageRegistry.GroupchatWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    error.Content = "Je moet een naam mee geven.";
                }
            }
            else
            {
                error.Content = "Je moet minimaal 1 vriend toevoegen.";
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            fileDialog.ShowDialog();
        }
    }
}
