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
using Microsoft.Win32;
using Model;
using Model.GroepsChat;
using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GroupchatSettings.xaml
    /// </summary>
    public partial class GroupchatSettings : Window
    {
        OpenFileDialog fileDialog;
        string FileLocation = "";

        public int GroupID;
        List<CheckBox> checkBoxes = new List<CheckBox>();
        public List<Users> Friends = new List<Users>();
        public List<Profile> InGroup;
        public GroupChat groupChat;
        GroupchatWindow groupChatWindow;

        public GroupchatSettings(int groupID, object creator)
        {
            GroupID = groupID;
            InitializeComponent();
            groupChat = GroepsChatQuerys.GetGroupChat(GroupID);
            GroepNaam.Text = groupChat.GroupName;
            Beschrijving.Text = groupChat.Description;
            InGroup = GroepsChatQuerys.GetMembersList(GroupID);
            groupChatWindow = (GroupchatWindow)creator;

            fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".png"; // Required file extension 
            fileDialog.Filter = "Image Document (.png)|*.png"; // Optional file extensions

            fileDialog.FileOk += FileDialog_FileOk;

            //kijken of het de gebruiker de beheerder is
            if (!groupChat.AdminID.Equals(StaticProfile.UserProfile.Id))
            {
                GroepNaam.IsEnabled = false;
                Beschrijving.IsEnabled = false;
                UploadButton.Visibility = Visibility.Hidden;
                Aanpassen.Visibility = Visibility.Hidden;
                //mensen plaatsen
                //beheerder plaatsen
                friendlist2.Height += 20;
                CheckBox box1 = new CheckBox { Content = ProfileQuerys.GetProfile(groupChat.AdminID).DisplayName };
                box1.IsChecked = true;
                box1.IsEnabled = false;
                checkBoxes.Add(box1);
                //de rest plaatsen
                foreach (Profile friend in InGroup)
                {
                    friendlist2.Height += 20;
                    CheckBox box = new CheckBox { Content = friend.DisplayName };
                    foreach (Profile profile in InGroup)
                    {
                        box.IsChecked = true;
                        box.IsEnabled = false;
                    }
                    checkBoxes.Add(box);
                }
                friendlist2.ItemsSource = checkBoxes;
            }
            else
            {
                //je zelf plaatsen
                friendlist2.Height += 20;
                CheckBox box1 = new CheckBox { Content = ProfileQuerys.GetProfile(groupChat.AdminID).DisplayName };
                box1.IsChecked = true;
                box1.IsEnabled = false;
                checkBoxes.Add(box1);
                //vriendenlijst plaatsen
                Friends = MatchmakingQuerys.getMatches(StaticProfile.UserProfile.Id);
                foreach (Users friend in Friends)
                {
                    friendlist2.Height += 20;
                    CheckBox box = new CheckBox {Content = friend.MatchedProfile.DisplayName};
                    foreach (Profile profile in InGroup)
                    {
                        if (profile.DisplayName.Equals(friend.MatchedProfile.DisplayName))
                        {
                            box.IsChecked = true;
                        }
                    }

                    checkBoxes.Add(box);
                }

                friendlist2.ItemsSource = checkBoxes;
            }
        }

        /// <summary>
        /// Stores location of file when file dialog is confirmed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void leave_Click(object sender, RoutedEventArgs e)
        {
            GroupChatLeave window = new GroupChatLeave(GroupID);
            window.Show();
        }

        private void Aanpassen_Click(object sender, RoutedEventArgs e)
        {
            //update naam
            if (!groupChat.GroupName.Equals(GroepNaam.Text))
            {
                GroepsChatQuerys.ChangeGroepNaam(GroepNaam.Text, GroupID);
            }

            //update beschrijving
            if (!groupChat.Description.Equals(Beschrijving.Text))
            {
                GroepsChatQuerys.ChangeBeschrijving(Beschrijving.Text, GroupID);
            }

            //members aanpassen in de groeps-chat
            //zet de vrienden om in string zodat ik contains kan gebruiken
            List<string> ingroep2 = new List<string>();
            foreach (Profile profile in InGroup)
            {
                ingroep2.Add(profile.DisplayName);
            }

            //kijk wat de veranderingen zijn en gooi ze er uit of voeg ze toe
            foreach (CheckBox box in friendlist2.ItemsSource)
            {
                if ((bool)box.IsChecked)
                {
                    if (!ingroep2.Contains(box.Content))
                    {
                        //nieuw in de groep
                        foreach (Users user in Friends)
                        {
                            if (user.MatchedProfile.DisplayName.Equals(box.Content))
                            {
                                GroepsChatQuerys.AddMember(GroupID, user.MatchedProfile.Id);
                                //GroepsChatQuerys.SendNewMemberMessage(GroupID, user.MatchedProfile.Id);
                            }
                        }
                    }
                }
                else
                {
                    if (ingroep2.Contains(box.Content))
                    {
                        // moet uit de groep
                        foreach (Users user in Friends)
                        {
                            if (user.MatchedProfile.DisplayName.Equals(box.Content))
                            {
                                GroepsChatQuerys.RemoveMember(user.MatchedProfile.Id,GroupID);
                            }
                        }
                    }
                }
            }

            if(!string.IsNullOrWhiteSpace(FileLocation))
            {
                Model.ProfielEnProfieldata.Images.Image img = new Model.ProfielEnProfieldata.Images.Image(fileDialog.FileName);
                img.UploadGroupProfilePicture(GroupID);
                Model.ProfielEnProfieldata.Images.Image.GetGroupProfilePicture(GroupID);

                groupChatWindow.UpdateProfilePic(img.fileLocation);
            }

            //close dit
            PageRegistry.GroupchatWindow.Close();
            PageRegistry.GroupchatWindow = new GroupchatWindow(GroupID, GroepsChatQuerys.GetAllGroupMessages(GroupID));
            PageRegistry.GroupchatWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Opens dialog window upon clicking upload button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            fileDialog.ShowDialog();
        }
    }
}
