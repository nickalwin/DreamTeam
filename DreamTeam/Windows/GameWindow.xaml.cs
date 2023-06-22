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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Game game;
        public GameWindow(Game game)
        {
            this.game = game;
            InitializeComponent();
            if (ProfileQuerys.ProfileCheckGames(StaticProfile.UserProfile.Id, game.Id))
            {
                GameAddBtn.Content = "Aanpassen";
                DeleteBtn.IsEnabled = true;
                DeleteBtn.Opacity = 100;
            }
            else
            {
                GameAddBtn.Content = "Voeg Toe";
                DeleteBtn.IsEnabled = false;
                DeleteBtn.Opacity = 0;
            }
            List<User> items = new List<User>();
            //maak users aan gebaseerd op wie je er in wilt hebben.
            List<string> names = ProfileQuerys.GetAllUsernamesOfAGame(game.Id, StaticProfile.UserProfile.Id);
            foreach (string name in names)
            {
                items.Add(new User(name));
            }
            lvUsers.ItemsSource = items;
            titleName.Content = this.game.Name;
        }

        public static void CloseWindowNow()
        {
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                if (Application.Current.Windows[i].GetType().FullName.Contains("GameWindow"))
                {
                    Application.Current.Windows[i].Close();
                }
            }
        }

        private void VoegToeButtonClick(object sender, RoutedEventArgs e)
        {
            GameWindowVoegToeDialoog window = new GameWindowVoegToeDialoog(game);
            window.Show();
            //PageRegistry.registerScreen.Show();
        }

        private void GaTerug(object sender, RoutedEventArgs e)
        {
            GamesOverview window = new GamesOverview();
            window.Show();
            this.Close();
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            GameWindowDelete window = new GameWindowDelete(game);
            window.Show();
            //verwijder van profiel in database
        }

        private void Profiel(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Profiel window = new Profiel();
            window.Show();
            this.Close();
        }

        public class User
        {
            public string Name { get; set; }
            public User(String name)
            {
                Name = name;
            }
        }

        private void ProfielButton_Click(object sender, RoutedEventArgs e)
        {
            Profiel profielwindow = new Profiel();
            profielwindow.Show();
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

        private void GroupchatButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.GroupchatWindowOverview = new GroupchatWindowOverview();
            PageRegistry.GroupchatWindowOverview.Show();
            this.Hide();
        }
    }
}
