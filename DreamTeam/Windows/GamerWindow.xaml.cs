using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.SQLData;
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
using static DreamTeam.Windows.GameWindow;
using Model.ProfielEnProfieldata.Images;
using System.Diagnostics;
using DreamTeam.Windows.Modals;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GamerWindow.xaml
    /// </summary>
    public partial class GamerWindow : Window
    {
        private int Id;

        public List<Post> ProfilePosts { get; private set; }
        public List<Connection> Connections { get; set; }
        public bool isFriend;

        // Box that will show on hover.
        HoverMouseDialog dialog = new HoverMouseDialog();

        public GamerWindow(int id)
        {
            this.Id = id;
            InitializeComponent();
            UpdateContent();
            UpdateConnections();

            //games
            List<Gamepie> games = new List<Gamepie>();
            List<Game> lijst = ProfileQuerys.getgamelist(id);
            foreach (Game game in lijst)
            {
                games.Add(new Gamepie(game.Name,game.Rank.NaamRank));
            }
            lvGames.ItemsSource = games;
            //gemeenschappelijke vrienden
            List<User> users = new List<User>();
            List<string> profiles = ProfileQuerys.GetincommonFriends(id);
            foreach (string name in profiles)
            {
                users.Add(new User(name));
            }
            lvcommun.ItemsSource = users;
            

            //andere vrienden
            List<Users> userslijst = new List<Users>();
            List<User> users2 = new List<User>();
            userslijst = MatchmakingQuerys.getMatches(id);
            foreach (Users user in userslijst)
            {
                bool dupe = false;
                foreach (User usere in users)
                {
                    if (usere.Name.Equals(user.MatchedProfile.DisplayName))
                    {
                        dupe = true;
                    }
                }
                if (!dupe)
                {
                    users2.Add(new User(user.MatchedProfile.DisplayName));
                }

                if (user.MatchedProfile.Id == StaticProfile.UserProfile.Id)
                    isFriend = true;
            }
            lvother.ItemsSource = users2;
        }

        public void UpdateProfilePic()
        {
            Model.ProfielEnProfieldata.Images.Image profilePic = Model.ProfielEnProfieldata.Images.Image.GetProfilePicture(Id);

            if (Media.FileExists(profilePic.GetFileLocation()) && profilePic.GetFileLocation() != "")
            {
                ProfileImage.Source = new BitmapImage(new Uri(profilePic.GetFileLocation(), UriKind.Absolute));
            }
        }

        public void UpdateContent()
        {
            GamerNaamLabel.Content = Model.Matchmaking.GamerProfile.getProfile(Id).DisplayName;
            GamerUsername.Content = $"Gender: {Model.Matchmaking.GamerProfile.getProfile(Id).Gender} | User id: {Id}";
            GamerBio.Text = Model.Matchmaking.GamerProfile.getProfile(Id).Omschrijving;

            UpdateProfilePosts();
            UpdateProfilePic();
        }
        private void ButtonGameOne_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Search window = new Search();
            window.Show();
            this.Close();
        }

        private void Profielbutton(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Profiel window = new Profiel();
            window.Show();
            this.Close();
        }
        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.GamesOverview window = new GamesOverview();
            window.Show();
            this.Close();
        }

        private void BackLable_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        public void UpdateProfilePosts()
        {
            ProfilePosts = MediaQuerys.GetProfilePosts(Id);

            if (ProfilePosts.Count > 0)
            {
                Post1.Source = new Uri(ProfilePosts[0].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                Post1.LoadedBehavior = MediaState.Manual;
                Post1.Play();
                Post1.ScrubbingEnabled = true;
                Post1.Position = TimeSpan.FromTicks(1);
                Post1.Pause();

                Post1.MouseUp += (sender, args) =>
                {
                    dialog = new HoverMouseDialog();
                    dialog.Content.Content = ProfilePosts[0].Beschrijving;
                    dialog.Media.Source = new Uri(ProfilePosts[0].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                    dialog.Media.Play();
                    dialog.Media.MediaEnded += (o, eventArgs) =>
                    {
                        dialog.Media.Position = TimeSpan.Zero;
                        dialog.Media.Play();
                    };
                    dialog.Show();
                };
            }
            if (ProfilePosts.Count > 1)
            {
                Post2.Source = new Uri(ProfilePosts[1].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                Post2.LoadedBehavior = MediaState.Manual;
                Post2.Play();
                Post2.ScrubbingEnabled = true;
                Post2.Position = TimeSpan.FromTicks(1);
                Post2.Pause();

                Post2.MouseUp += (sender, args) =>
                {
                    dialog = new HoverMouseDialog();
                    dialog.Content.Content = ProfilePosts[1].Beschrijving;

                    dialog.Media.Source = new Uri(ProfilePosts[1].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                    dialog.Media.Play();
                    dialog.Media.MediaEnded += (o, eventArgs) =>
                    {
                        dialog.Media.Position = TimeSpan.Zero;
                        dialog.Media.Play();
                    };

                    dialog.Show();
                };
            }
            if (ProfilePosts.Count > 2)
            {
                Post3.Source = new Uri(ProfilePosts[2].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                Post3.LoadedBehavior = MediaState.Manual;
                Post3.Play();
                Post3.ScrubbingEnabled = true;
                Post3.Position = TimeSpan.FromTicks(1);
                Post3.Pause();

                Post3.MouseUp += (sender, args) =>
                {
                    dialog = new HoverMouseDialog();
                    dialog.Content.Content = ProfilePosts[2].Beschrijving;

                    dialog.Media.Source = new Uri(ProfilePosts[2].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                    dialog.Media.Play();
                    dialog.Media.MediaEnded += (o, eventArgs) =>
                    {
                        dialog.Media.Position = TimeSpan.Zero;
                        dialog.Media.Play();
                    };

                    dialog.Show();
                };
            }
            if (ProfilePosts.Count > 3)
            {
                Post4.Source = new Uri(ProfilePosts[3].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                Post4.LoadedBehavior = MediaState.Manual;
                Post4.Play();
                Post4.ScrubbingEnabled = true;
                Post4.Position = TimeSpan.FromTicks(1);
                Post4.Pause();

                Post4.MouseUp += (sender, args) =>
                {
                    dialog = new HoverMouseDialog();
                    dialog.Content.Content = ProfilePosts[3].Beschrijving;

                    dialog.Media.Source = new Uri(ProfilePosts[3].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                    dialog.Media.Play();
                    dialog.Media.MediaEnded += (o, eventArgs) =>
                    {
                        dialog.Media.Position = TimeSpan.Zero;
                        dialog.Media.Play();
                    };

                    dialog.Show();
                };
            }
            if (ProfilePosts.Count > 4)
            {
                Post5.Source = new Uri(ProfilePosts[4].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                Post5.LoadedBehavior = MediaState.Manual;
                Post5.Play();
                Post5.ScrubbingEnabled = true;
                Post5.Position = TimeSpan.FromTicks(1);
                Post5.Pause();

                Post5.MouseUp += (sender, args) =>
                {
                    dialog = new HoverMouseDialog();
                    dialog.Content.Content = ProfilePosts[4].Beschrijving;

                    dialog.Media.Source = new Uri(ProfilePosts[4].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                    dialog.Media.Play();
                    dialog.Media.MediaEnded += (o, eventArgs) =>
                    {
                        dialog.Media.Position = TimeSpan.Zero;
                        dialog.Media.Play();
                    };

                    dialog.Show();
                };
            }
            if (ProfilePosts.Count > 5)
            {
                Post6.Source = new Uri(ProfilePosts[5].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                Post6.LoadedBehavior = MediaState.Manual;
                Post6.Play();
                Post6.ScrubbingEnabled = true;
                Post6.Position = TimeSpan.FromTicks(1);
                Post6.Pause();

                Post6.MouseUp += (sender, args) =>
                {
                    dialog = new HoverMouseDialog();
                    dialog.Content.Content = ProfilePosts[5].Beschrijving;

                    dialog.Media.Source = new Uri(ProfilePosts[5].AttachedMedia[0].GetFileLocation(), UriKind.Absolute);
                    dialog.Media.Play();
                    dialog.Media.MediaEnded += (o, eventArgs) =>
                    {
                        dialog.Media.Position = TimeSpan.Zero;
                        dialog.Media.Play();
                    };

                    dialog.Show();
                };
            }
        }

        private void Post1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
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

        private void Post1_MouseEnter(object sender, MouseEventArgs e)
        {
            Post1.Play();
        }

        private void Post1_MouseLeave(object sender, MouseEventArgs e)
        {
            Post1.Pause();
        }

        private void Post2_MouseEnter(object sender, MouseEventArgs e)
        {
            Post2.Play();
        }

        private void Post2_MouseLeave(object sender, MouseEventArgs e)
        {
            Post2.Pause();
        }

        private void Post3_MouseEnter(object sender, MouseEventArgs e)
        {
            Post3.Play();
        }

        private void Post3_MouseLeave(object sender, MouseEventArgs e)
        {
            Post3.Pause();
        }

        private void Post4_MouseEnter(object sender, MouseEventArgs e)
        {
            Post4.Play();
        }

        private void Post4_MouseLeave(object sender, MouseEventArgs e)
        {
            Post4.Pause();
        }

        private void Post5_MouseEnter(object sender, MouseEventArgs e)
        {
            Post5.Play();
        }

        private void Post5_MouseLeave(object sender, MouseEventArgs e)
        {
            Post5.Pause();
        }

        private void Post6_MouseEnter(object sender, MouseEventArgs e)
        {
            Post6.Play();
        }

        private void Post6_MouseLeave(object sender, MouseEventArgs e)
        {
            Post6.Pause();
        }

        private void GroupchatButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.GroupchatWindowOverview = new GroupchatWindowOverview();
            PageRegistry.GroupchatWindowOverview.Show();
            this.Hide();
        }
        private void lvcommun_MouseUp(object sender, MouseButtonEventArgs e)
        {
            User SelectedItem = (User)lvcommun.SelectedItems[0];

            GamerWindow window = new GamerWindow(ProfileQuerys.ProfileGetIdName(SelectedItem.Name));
            window.Show();
        }

        private void lvother_MouseUp(object sender, MouseButtonEventArgs e)
        {
            User SelectedItem = (User)lvother.SelectedItems[0];

            GamerWindow window = new GamerWindow(ProfileQuerys.ProfileGetIdName(SelectedItem.Name));
            window.Show();
        }

        private void lvGames_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Gamepie SelectedItem = (Gamepie)lvGames.SelectedItems[0];

            GameWindow window = new GameWindow(new Game(ProfileQuerys.GetIdGame(SelectedItem.Name)));
            window.Show();
            this.Close();
        }
        private void lvGames_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void UpdateConnections()
        {
            List<Connection> conns = ConnectionQuerys.getProfileConnections(Id);
            List<Connection> filteredConns = new List<Connection>();
            foreach (Connection conn in conns)
            {
                if (conn.Visibility.Equals(ConnectionVisibility.Hidden) && isFriend)
                {
                    filteredConns.Add(conn);
                } else if (conn.Visibility.Equals(ConnectionVisibility.Shown))
                {
                    filteredConns.Add(conn);
                }
            }

            lvKoppeling.ItemsSource = conns;
        }

        private void lvKoppeling_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Connection selected = (Connection)lvKoppeling.SelectedItem;

            string url = selected.getLink();
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }

    public class Gamepie
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public Gamepie(String name, string rank)
        {
            Name = name;
            Rank = rank;
        }
    }
}
