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
using Model.Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GameWindowVoegToeDialoog.xaml
    /// </summary>
    public partial class GameWindowVoegToeDialoog : Window
    {
        private Game game;
        public List<Rank> data;
        public GameWindowVoegToeDialoog(Game game)
        {
            InitializeComponent();
            if (ProfileQuerys.ProfileCheckGames(StaticProfile.UserProfile.Id, game.Id))
            {
                GameChange.Content = "Aanpassen";
            }
            else
            {
                GameChange.Content = "Voeg Toe";
            }
            this.game = game;
        }

        private List<Rank> GetRanks(int game)
        {
            List<Rank> ranks = new List<Rank>();
            ranks = ProfileQuerys.GetRanksOfGame(game);
            return ranks;
        }

        private void VoegToe(object sender, RoutedEventArgs e)
        {
            //do thing in de database

                ComboBoxItem comboBoxItem = (ComboBoxItem)PlayingStyle.SelectedItem;
                string str = comboBoxItem.Content.ToString();
                Playstyle playstyle = (Playstyle)Enum.Parse(typeof(Playstyle), str);
                int rank = data[RankDropdown.SelectedIndex].RankId;
                if (ProfileQuerys.ProfileAddGames(StaticProfile.UserProfile.Id, game.Id, rank, playstyle))
                {
                GameWindow.CloseWindowNow();
                GameWindow window = new GameWindow(new Game(game.Id));
                window.Show();
            }
                else if(ProfileQuerys.ProfileChangeGames(StaticProfile.UserProfile.Id, game.Id, rank, playstyle))
                {
                GameWindow.CloseWindowNow();
                GameWindow window = new GameWindow(new Game(game.Id));
                window.Show();
            }
            this.Close();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            data = GetRanks(game.Id);
            RankDropdown.ItemsSource = data;
            RankDropdown.SelectedIndex = 0;
        }

    }
}
