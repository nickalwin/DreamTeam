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
using Model.GroepsChat;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GroupchatWindow.xaml
    /// </summary>
    public partial class GroupchatWindowOverview : Window
    {
        public List<GroupChat> GroupsChats = new List<GroupChat>();
        int topMargin = 0;

        public GroupchatWindowOverview()
        {
            InitializeComponent();
            GroupsChats = GroepsChatQuerys.GetPersonsGroepGroupChats(StaticProfile.UserProfile.Id);
            placeGroups();
        }

        private void ProfielButton_Click(object sender, RoutedEventArgs e)
        {
            Profiel profielwindow = new Profiel();
            profielwindow.Show();
            this.Hide();
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesOverview gameswindow = new GamesOverview();
            gameswindow.Show();
            this.Hide();
        }

        private void MatchesButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.Matches = new Matches();
            PageRegistry.Matches.InitializeComponent();
            PageRegistry.Matches.Show();
            this.Hide();
        }

        private void LikesButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.LikesWindow = new LikesWindow();
            PageRegistry.LikesWindow.Show();
            this.Hide();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Search searchwindow = new Search();
            searchwindow.Show();
            this.Close();
        }

        private void GroupchatButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NewGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGroupChat windowCreateNewGroupChat = new CreateNewGroupChat();
            windowCreateNewGroupChat.Show();
        }

        private void placeGroups()
        {
            foreach (GroupChat groupChat in GroupsChats)
            {
                //kijken of de persoon gekicked is
                bool kicked = true;
                bool show = true;
                if (StaticProfile.UserProfile.Id == GroepsChatQuerys.GetGroupChat(groupChat.ID).AdminID)
                {
                    kicked = false;
                }
                else
                {
                    foreach (Profile profile in GroepsChatQuerys.GetMembersList(groupChat.ID))
                    {
                        if (profile.Id.Equals(StaticProfile.UserProfile.Id))
                        {
                            kicked = false;
                        }
                    }
                }

                if (kicked)
                {
                    //check of hij er helemaal uit is
                    if(GroepsChatQuerys.ISKickedORDeleted(StaticProfile.UserProfile.Id, groupChat.ID).Equals(2))
                    {
                        //dont show
                        show = false;
                    }
                }

                if (show)
                {
                    Grid GridMatch = new Grid();

                    Label groupchatName = new Label();
                    groupchatName.Content = groupChat.GroupName;
                    groupchatName.FontSize = 20;
                    groupchatName.Width = 1400;
                    groupchatName.Height = 80;
                    groupchatName.Foreground = new SolidColorBrush(Colors.White);
                    groupchatName.Background = new LinearGradientBrush(
                        (Color) ColorConverter.ConvertFromString("#FFF288A4"),
                        (Color) ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                    groupchatName.Margin = new Thickness(0, 10, 0, 0);

                    Label labelDescription = new Label();
                    labelDescription.Content = groupChat.Description;
                    labelDescription.FontSize = 15;
                    labelDescription.Width = 1500;
                    labelDescription.Height = 40;
                    labelDescription.Foreground = new SolidColorBrush(Colors.White);
                    //LabelGender.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                    labelDescription.Margin = new Thickness(50, 40, 1000, 0);

                    Label labelCreatedAt = new Label();
                    labelCreatedAt.Content = groupChat.Created_at;
                    labelCreatedAt.FontSize = 15;
                    labelCreatedAt.Width = 200;
                    labelCreatedAt.Height = 40;
                    labelCreatedAt.Foreground = new SolidColorBrush(Colors.White);
                    //LabelAge.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                    labelCreatedAt.Margin = new Thickness(0, 40, 800, 0);

                    Image ImageProfile = new Image();
                    ImageProfile.Width = 100;
                    ImageProfile.Height = 100;

                    System.Windows.Controls.Button groupchatbutton = new System.Windows.Controls.Button();

                    if (kicked)
                    {
                        groupchatbutton.Content = "Inzien";
                    }
                    else
                    {
                        groupchatbutton.Content = "Chatten";
                    }

                    groupchatbutton.Width = 200;
                    groupchatbutton.Height = 50;
                    groupchatbutton.Margin = new Thickness(1195, 10, 30, 0);
                    groupchatbutton.Name = $"i{groupChat.ID}";

                    groupchatbutton.Click += OpenGroupChat;

                    Grid StyledGrid = StyleGridMatch(GridMatch, groupchatName, labelDescription, labelCreatedAt,
                        ImageProfile, groupchatbutton);
                    StyledGrid.Width = 1500;
                    StyledGrid.Height = 150;
                    StyledGrid.Margin = new Thickness(0, topMargin, 0, 0);
                    GroupchatGrid.Children.Add(StyledGrid);

                    topMargin += 100;
                    GroupchatGrid.Height = topMargin;
                }
            }
        }
        private Grid StyleGridMatch(Grid grid, Label LabelGamerNaam, Label LabelGender, Label LabelAge, Image ImageProfile, System.Windows.Controls.Button ButtonChatten)
        {
            grid.Children.Add(LabelGamerNaam);
            grid.Children.Add(LabelGender);
            grid.Children.Add(LabelAge);
            grid.Children.Add(ImageProfile);
            grid.Children.Add(ButtonChatten);
            grid.VerticalAlignment = VerticalAlignment.Top;
            return grid;
        }
        private void OpenGroupChat(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            /*PageRegistry.chat = new Chat(int.Parse(button.Name.Remove(0, 1)), ChatQuerys.GetMessages(int.Parse(button.Name.Remove(0, 1))));
            PageRegistry.chat.Show();*/
            PageRegistry.GroupchatWindow = new GroupchatWindow((int.Parse(button.Name.Remove(0, 1))), GroepsChatQuerys.GetAllGroupMessages((int.Parse(button.Name.Remove(0, 1)))));
            PageRegistry.GroupchatWindow.Show();
        }
    }
}
