using Model;
using Model.Matchmaking;
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

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        List<Game> selectedGames = new List<Game>();

        public Search()
        {
            InitializeComponent();

            List<CheckBox> checkBoxes = new List<CheckBox>();
            List<Game> games = ProfileQuerys.GetAllGames();

            foreach (Game game in games)
            {
                ic.Height += 20;
                CheckBox box = new CheckBox { Content = game.Name };
                box.Checked += CheckBox_Checked;
                box.Unchecked += CheckBox_Unchecked;
                checkBoxes.Add(box);
            }
            ic.ItemsSource = checkBoxes;
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

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            PageRegistry.mainWindow = new MainWindow();
            PageRegistry.mainWindow.Show();
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            int gameId = ProfileQuerys.GetIdGame((string)box.Content);

            selectedGames.Add(ProfileQuerys.GetGame(gameId));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;

            selectedGames.RemoveAll(s => s.Name == (string)box.Content);
        }

        private void LeftAgeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LeftAgeSlider != null && RightAgeSlider != null && AgeLabel != null)
            {
                Slider leftSlider = (Slider)sender;
                Slider rightSlider = RightAgeSlider;

                if (leftSlider.Value > rightSlider.Value)
                {
                    rightSlider.Value = leftSlider.Value;
                }

                AgeLabel.Content = Math.Round(leftSlider.Value) + " - " + Math.Round(rightSlider.Value);
            }
        }

        private void RightAgeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LeftAgeSlider != null && RightAgeSlider != null && AgeLabel != null)
            {
                Slider rightSlider = (Slider)sender;
                Slider leftSlider = LeftAgeSlider;

                if (rightSlider.Value < leftSlider.Value)
                {
                    leftSlider.Value = rightSlider.Value;
                }

                AgeLabel.Content = Math.Round(leftSlider.Value) + " - " + Math.Round(rightSlider.Value);
            }
        }

        private void SearchButton1_Click(object sender, RoutedEventArgs e)
        {
            SearchCriteria criteria = new SearchCriteria { hasGames = selectedGames, InclusiveAgeRange = new Tuple<int, int>((int)Math.Round(LeftAgeSlider.Value), (int)Math.Round(RightAgeSlider.Value)), toxicityLevel = (float)Math.Round(ToxicitySlider.Value, 1) };

            if (GeenVoorkeurGamestyle.IsChecked == true)
            {
                criteria.PlayStyle = Playstyle.GeenVoorkeur;
            } else if (CasualGamestyle.IsChecked == true && CompetatiefGamestyle.IsChecked == true)
            {
                criteria.PlayStyle = Playstyle.Beide;
            } else if(CasualGamestyle.IsChecked == true)
            {
                criteria.PlayStyle = Playstyle.Casual;
            } else if(CompetatiefGamestyle.IsChecked == true)
            {
                criteria.PlayStyle = Playstyle.Competitief;
            }

            Queue<GamerProfile> potentials = Users.getPotentials(criteria);

            if(potentials.Count() > 0)
            {
                PageRegistry.matchWindow = new MatchWindow(potentials);
                PageRegistry.matchWindow.Show();
            } else
            {
                ErrorSearch.Content = "Geen resultaten gevonden!";
            }
        }

        private void ToxicitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider toxicitySlider = (Slider)sender;

            if(Toxicity_level != null)
            {
                Toxicity_level.Content = Math.Round(toxicitySlider.Value, 1);
            }
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
