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

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for LikesWindow.xaml
    /// </summary>
    public partial class LikesWindow : Window
    {
        private List<Users> likes = new List<Users>();
        int topMargin = 0;
        public LikesWindow()
        {
            InitializeComponent();
            likes = MatchmakingQuerys.getLikes(StaticProfile.UserProfile.Id);
            PlaceMatches();
        }
        private void PlaceMatches()
        {
            topMargin = 0;
            foreach (Users like in likes)
            {
                Grid GridMatch = new Grid();

                Label LabelGamerNaam = new Label();
                LabelGamerNaam.Content = like.MatchedProfile.DisplayName;
                LabelGamerNaam.FontSize = 20;
                LabelGamerNaam.Width = 1400;
                LabelGamerNaam.Height = 80;
                LabelGamerNaam.Foreground = new SolidColorBrush(Colors.White);
                LabelGamerNaam.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                LabelGamerNaam.Margin = new Thickness(0, 10, 0, 0);

                Label LabelGender = new Label();
                LabelGender.Content = like.MatchedProfile.Gender;
                LabelGender.FontSize = 15;
                LabelGender.Width = 100;
                LabelGender.Height = 40;
                LabelGender.Foreground = new SolidColorBrush(Colors.White);
                //LabelGender.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                LabelGender.Margin = new Thickness(0, 40, 1200, 0);

                Label LabelAge = new Label();
                LabelAge.Content = like.MatchedProfile.Age;
                LabelAge.FontSize = 15;
                LabelAge.Width = 100;
                LabelAge.Height = 40;
                LabelAge.Foreground = new SolidColorBrush(Colors.White);
                //LabelAge.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                LabelAge.Margin = new Thickness(0, 40, 1300, 0);

                Image ImageProfile = new Image();
                ImageProfile.Width = 100;
                ImageProfile.Height = 100;

                System.Windows.Controls.Button ButtonLike = new System.Windows.Controls.Button();
                ButtonLike.Content = "Like";
                ButtonLike.Width = 200;
                ButtonLike.Height = 50;
                ButtonLike.Margin = new Thickness(1195, 10, 30, 0);
                ButtonLike.Name = $"i{like.MatchedProfile.Id.ToString()}";
                ButtonLike.Click += Like;

                Grid StyledGrid = StyleGridMatch(GridMatch, LabelGamerNaam, LabelGender, LabelAge, ImageProfile, ButtonLike);
                StyledGrid.Width = 1500;
                StyledGrid.Height = 150;
                StyledGrid.Margin = new Thickness(0, topMargin, 0, 0);
                MatchesGrid.Children.Add(StyledGrid);

                topMargin += 200;
                MatchesGrid.Height = topMargin;
            }
        }
        private Grid StyleGridMatch(Grid grid, Label LabelGamerNaam, Label LabelGender, Label LabelAge, Image ImageProfile, System.Windows.Controls.Button ButtonLike)
        {
            grid.Children.Add(LabelGamerNaam);
            grid.Children.Add(LabelGender);
            grid.Children.Add(LabelAge);
            grid.Children.Add(ImageProfile);
            grid.Children.Add(ButtonLike);
            return grid;
        }
        private void Like(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            MatchmakingQuerys.ApproveLike(StaticProfile.UserProfile.Id, int.Parse(button.Name.Remove(0, 1)));
            PageRegistry.LikesWindow = new LikesWindow();
            PageRegistry.LikesWindow.Show();
            this.Hide();
        }

        private void EmptyGrid(Grid grid)
        {
            grid.Children.RemoveRange(0, grid.Children.Count);
        }

        private void ProfielButton_Click(object sender, RoutedEventArgs e)
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

        private void MatchesButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.Matches = new Matches();
            PageRegistry.Matches.InitializeComponent();
            PageRegistry.Matches.Show();
            this.Hide();
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.GamesOverview window = new GamesOverview();
            window.Show();
            this.Close();
        }

        private void GroupchatButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.GroupchatWindowOverview = new GroupchatWindowOverview();
            PageRegistry.GroupchatWindowOverview.Show();
            this.Hide();
        }
    }
}
