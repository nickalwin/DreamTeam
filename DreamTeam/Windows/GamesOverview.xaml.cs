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
using Microsoft.VisualBasic.CompilerServices;
using Model;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GamesOverview.xaml
    /// </summary>
    public partial class GamesOverview : Window
    {
        public GamesOverview()
        {
            InitializeComponent();

            List<Button> buttons = new List<Button>();
            List<Game> games = ProfileQuerys.GetAllGames();

            foreach (Game game in games)
            {
                buttons.Add(new Button { ButtonContent = game.Name, ButtonID = game.Id.ToString() });
            }
            ic.ItemsSource = buttons;
        }

        private void OpenGamePage(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string name = (string)button.Content;
            GameWindow window = new GameWindow(new Game(ProfileQuerys.GetIdGame(name)));
            window.Show();
            this.Close();
        }

        private void Profiel(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Profiel window = new Profiel();
            window.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Search searchwindow = new Search();
            searchwindow.Show();
            this.Close();
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.GamesOverview window = new GamesOverview();
            window.Show();
            this.Close();
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

    class Button
    {
        public string ButtonContent { get; set; }
        public string ButtonID { get; set; }
    }
}
