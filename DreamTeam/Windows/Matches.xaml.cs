using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using Model.Matchmaking;
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
    /// Interaction logic for Matches.xaml
    /// </summary>
    public partial class Matches : Window
    {
        public List<Users> matches;
        int topMargin = 0;
        public Matches()
        {
            InitializeComponent();
            matches = MatchmakingQuerys.getMatches(StaticProfile.UserProfile.Id);
            PlaceMatches();
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

        private void instellingenProfiel(object sender, RoutedEventArgs e)
        {
            PageRegistry.ProfileSettingsscreen = new ProfileSettings();
            PageRegistry.ProfileSettingsscreen.Show();
            //ProfileSettings window = new ProfileSettings();
            //window.Show();
        }
        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            PageRegistry.mainWindow = new MainWindow();
            PageRegistry.mainWindow.Show();
            this.Close();
        }
        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.GamesOverview window = new GamesOverview();
            window.Show();
            this.Close();
        }
        private void PlaceMatches()
        {
            foreach (Users match in matches)
            {
                Grid GridMatch = new Grid();
                
                Label LabelGamerNaam = new Label();
                LabelGamerNaam.Content = match.MatchedProfile.DisplayName;
                LabelGamerNaam.FontSize = 20;
                LabelGamerNaam.Width = 1400;
                LabelGamerNaam.Height = 80;
                LabelGamerNaam.Foreground = new SolidColorBrush(Colors.White);
                LabelGamerNaam.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                LabelGamerNaam.Margin = new Thickness(0, 10, 0, 0);
                LabelGamerNaam.MouseLeftButtonDown += LabelGamerNaam_MouseLeftButtonDown;
                LabelGamerNaam.Name = $"i{match.MatchedProfile.Id.ToString()}";

                Label LabelGender = new Label();
                LabelGender.Content = match.MatchedProfile.Gender;
                LabelGender.FontSize = 15;
                LabelGender.Width = 100;
                LabelGender.Height = 40;
                LabelGender.Foreground = new SolidColorBrush(Colors.White);
                //LabelGender.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                LabelGender.Margin = new Thickness(0, 40, 1200, 0);

                Label LabelAge = new Label();
                LabelAge.Content = match.MatchedProfile.Age;
                LabelAge.FontSize = 15;
                LabelAge.Width = 100;
                LabelAge.Height = 40;
                LabelAge.Foreground = new SolidColorBrush(Colors.White);
                //LabelAge.Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0);
                LabelAge.Margin = new Thickness(0, 40, 1300, 0);

                Image ImageProfile = new Image();
                ImageProfile.Width = 100;
                ImageProfile.Height = 100;

                System.Windows.Controls.Button ButtonChatten = new System.Windows.Controls.Button();
                ButtonChatten.Content = "Chatten";
                ButtonChatten.Width = 200;
                ButtonChatten.Height = 50;
                ButtonChatten.Margin = new Thickness(1195, 10, 30, 0);
                ButtonChatten.Name = $"i{match.MatchedProfile.Id.ToString()}";
                
                ButtonChatten.Click += OpenChat;

                Grid StyledGrid = StyleGridMatch(GridMatch, LabelGamerNaam, LabelGender, LabelAge, ImageProfile, ButtonChatten);
                StyledGrid.Width = 1500;
                StyledGrid.Height = 150;
                StyledGrid.Margin = new Thickness(0, topMargin, 0, 0);
                MatchesGrid.Children.Add(StyledGrid);

                topMargin += 200;
                MatchesGrid.Height = topMargin;
            }
        }

        private void LabelGamerNaam_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Label label = (System.Windows.Controls.Label)sender;
            PageRegistry.gamerWindow = new GamerWindow(int.Parse(label.Name.Remove(0, 1)));
            PageRegistry.gamerWindow.Show();
        }

        private Grid StyleGridMatch(Grid grid, Label LabelGamerNaam, Label LabelGender, Label LabelAge, Image ImageProfile, System.Windows.Controls.Button ButtonChatten)
        {
            grid.Children.Add(LabelGamerNaam);
            grid.Children.Add(LabelGender);
            grid.Children.Add(LabelAge);
            grid.Children.Add(ImageProfile);
            grid.Children.Add(ButtonChatten);
            return grid;
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            PlaceMatches();
        }
        private void OpenChat(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            PageRegistry.chat = new Chat(int.Parse(button.Name.Remove(0,1)), ChatQuerys.GetMessages(int.Parse(button.Name.Remove(0, 1))));
            PageRegistry.chat.Show();
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
