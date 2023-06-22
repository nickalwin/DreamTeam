
using Model;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using System.Windows;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GameWindowVoegToeDialoog.xaml
    /// </summary>
    public partial class GameWindowDelete : Window
    {
        private Game game;
        public GameWindowDelete(Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ProfileQuerys.ProfileRemoveGame(StaticProfile.UserProfile.Id, game.Id))
            {
                GameWindow.CloseWindowNow();
                GameWindow window = new GameWindow(new Game(game.Id));
                window.Show();
                this.Close();
            }
        }

        private void DiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
