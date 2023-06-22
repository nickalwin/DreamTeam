using Model;
using Model.Matchmaking;
using Model.ProfielEnProfieldata;
using Model.SQLData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        List<Grid> gridLinks = new List<Grid>();
        List<Grid> gridRechts = new List<Grid>();
        List<Message> PlacedMessages = new List<Message>();
        DispatcherTimer timerSTA = new DispatcherTimer();
        int FriendId;
        String FriendName;

       public Chat(int FriendId, List<Message> messages)
        {
            InitializeComponent();
            this.FriendId = FriendId;
            FriendName = ProfileQuerys.GetName(FriendId);
            PlaceName();
            InitializeTimer();
            
            foreach (Message message in messages)
            {
                PlaceMessage(message.SenderId == StaticProfile.UserProfile.Id, message);
            }
        }
        private void InitializeTimer()
        {
            timerSTA.Tick += new EventHandler(dispatcherTimer_Tick);
            timerSTA.Interval = new TimeSpan(0, 0, 1);
            timerSTA.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            List<Message> messages = ChatQuerys.GetMessagesOfFriend(FriendId);
            foreach (Message message in messages)
            {
                if (!PlacedMessages.Exists(x => x.DateTime == message.DateTime && x.MessageContent.Equals(message.MessageContent)))
                {
                    PlaceMessage(message.SenderId == StaticProfile.UserProfile.Id, message);
                    PlacedMessages.Add(message);
                }
            }
        }
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Search window = new Search();
            window.Show();
            this.Close();
        }
        private void PlaceName()
        {
            GamerNaamLabel9.Content = FriendName;
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
        private string NewLineMessage(string message)
        {
            string[] splitString = message.Split(" ");
            StringBuilder builder = new StringBuilder();
            int count = 0;

            foreach (string word in splitString)
            {
                builder.Append($"{word} ");
                count++;
                if (count == 5)
                {
                    builder.Append("\n");
                    count = 0;
                }
            }
            return builder.ToString();
        }
        
        private void PlaceMessage(bool user, Message message)
        {
            messageGrid.Height += 50;
            scrollBar.ScrollToVerticalOffset(messageGrid.Height);

            string newLineMessage = NewLineMessage(message.MessageContent);
            int lines = newLineMessage.Split('\n').Length + 1;

            TextBlock text = new TextBlock { 
                TextWrapping = TextWrapping.WrapWithOverflow, 
                Text = message.MessageContent,
                Foreground = new SolidColorBrush(Colors.White),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18, Margin = new Thickness(5,0,0,0),
                Width = 350
            };

            Label DateTimeLabel = new Label { 
                Content = message.DateTime,
                FontSize = 12,
                Width = 350,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 30,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            Label NameLabel = new Label {
                Content = user ? $"{StaticProfile.UserProfile.DisplayName}" : $"{FriendName}",
                FontSize = 12,
                Width = 350,
                Height = 30,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Colors.White)
            };

            Grid LabelGrid = new Grid {
                Height = 100,
                Width = 350,
                Background = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0),
            };

            LabelGrid.Children.Add(NameLabel);
            LabelGrid.Children.Add(text);
            LabelGrid.Children.Add(DateTimeLabel);

            LabelGrid.Margin = user ? new Thickness(1000, messageGrid.Height - 10, 100, 0) : new Thickness(0, messageGrid.Height - 10, 1000, 0);

            if (user)
            {
                LabelGrid.Name = $"i{message.MessageId}i{FriendId}";
                LabelGrid.MouseLeftButtonUp += LabelGrid_MouseLeftButtonUp;
                gridRechts.Add(LabelGrid);
            }
            else
            {
                gridLinks.Add(LabelGrid);
            }

            foreach (Grid grid in gridRechts)
                grid.Margin = new Thickness(1000, grid.Margin.Top - (lines * 100), 100, 0);
            foreach (Grid grid in gridLinks)
                grid.Margin = new Thickness(0, grid.Margin.Top - (lines * 100), 1000, 0);

            messageGrid.Children.Add(LabelGrid);
        }

        private void LabelGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            PageRegistry.ChatChangeMessage = new Modals.ChatChangeMessage(grid, false);
            PageRegistry.ChatChangeMessage.Show();
        }

        private void VerstuurBericht_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(chatInput.Text))
            {
                if (ChatQuerys.SendMessage(FriendId, chatInput.Text))
                {
                    
                    Message message = new Message(StaticProfile.UserProfile.Id, FriendId, chatInput.Text, DateTime.Now, ChatQuerys.GetLastAiId(StaticProfile.UserProfile.Id, false));
                    chatInput.Text = "";
                    PlaceMessage(message.SenderId == StaticProfile.UserProfile.Id, message);
                }
                else
                {

                }
            }
        }
        private void ButtonVerwijderChat_Click(object sender, RoutedEventArgs e)
        {
            ChatDelete window = new ChatDelete(FriendId);
            window.Show();
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