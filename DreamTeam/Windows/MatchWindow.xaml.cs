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
using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata.Images;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class MatchWindow : Window
    {
        private Queue<GamerProfile> potentials;
        private GamerProfile currentProfile;
        public MatchWindow(Queue<GamerProfile> potentials)
        {
            this.potentials = potentials;
            InitializeComponent();

            NextPotential();
        }

        public GamerProfile NextPotential()
        {
            if (potentials.Count() > 0)
            {
                GamerProfile nextPotential = potentials.Dequeue();
                currentProfile = nextPotential;

                UpdateScreen();

                return currentProfile;
            } else
            {
                this.Hide();
            }

            return null;
        }

        private void ProfielButton_Click(object sender, RoutedEventArgs e)
        {
            Profiel profielwindow = new Profiel();
            profielwindow.Show();
            this.Hide();
        }
        private void MatchesBtn_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.Matches = new Matches();
            PageRegistry.Matches.InitializeComponent();
            PageRegistry.Matches.Show();
            this.Hide();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Search searchwindow = new Search();
            searchwindow.Show();
            this.Hide();
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesOverview gameswindow = new GamesOverview();
            gameswindow.Show();
            this.Hide();
        }
        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            PageRegistry.mainWindow = new MainWindow();
            PageRegistry.mainWindow.Show();
            this.Close();
        }

        private void UpdateScreen()
        {
            GamerName.Content = currentProfile.DisplayName;
            GenderLabel.Content = Enum.GetName(currentProfile.Gender);
            LeeftijdLabel.Content = currentProfile.Age;
            DescriptionBox.Text = currentProfile.Omschrijving;

            GameInfoBox1.Content = "";
            GameInfoBox2.Content = "";
            GameInfoBox3.Content = "";

            if (currentProfile.Games.Count() > 0)
            {
                GameInfoBox1.Content = currentProfile.Games[0].Playstyle + " | " + currentProfile.Games[0].Rank;
            }
            if(currentProfile.Games.Count() > 1)
            {
                GameInfoBox2.Content = currentProfile.Games[1].Playstyle + " | " + currentProfile.Games[1].Rank;
            }
            if (currentProfile.Games.Count() > 2)
            {
                GameInfoBox3.Content = currentProfile.Games[2].Playstyle + " | " + currentProfile.Games[2].Rank;
            }

            Model.ProfielEnProfieldata.Images.Image profilePic = Model.ProfielEnProfieldata.Images.Image.GetProfilePicture(currentProfile.Id);

            if (Media.FileExists(profilePic.GetFileLocation()) && profilePic.GetFileLocation() != "")
            {
                ProfilePic.Source = new BitmapImage(new Uri(profilePic.GetFileLocation(), UriKind.Absolute));
            }
        }

        private void LikeBtn_Click(object sender, RoutedEventArgs e)
        {
            currentProfile.LikeGamer(StaticProfile.UserProfile.Id);
            NextPotential();
        }

        private void DisLikeBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add to database
            NextPotential();
        }

        private void GameLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GamerName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void DescriptionBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GameInfoBox3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GameInfoBox2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GameInfoBox1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GenderHeader_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GenderLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void LeeftijdHeader_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void LeeftijdLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GameIcon1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GameIcon2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void GameIcon3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void InfoLable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void CircleInfoLable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }
        private void BorderGamerInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageRegistry.gamerWindow = new GamerWindow(currentProfile.Id);
            PageRegistry.gamerWindow.Show();
        }

        private void LikesButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.LikesWindow = new LikesWindow();
            PageRegistry.LikesWindow.Show();
            this.Hide();
        }

        private void GroupchatButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.GroupchatWindowOverview = new GroupchatWindowOverview();
            PageRegistry.GroupchatWindowOverview.Show();
            this.Hide();
        }
    }
}
