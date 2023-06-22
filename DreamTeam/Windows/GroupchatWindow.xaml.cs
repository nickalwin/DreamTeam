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
using Model.GroepsChat;
using System.Windows.Threading;
using Model;

namespace DreamTeam.Windows
{
    /// <summary>
    /// Interaction logic for GroupchatWindow.xaml
    /// </summary>
    public partial class GroupchatWindow : Window
    {
        List<Grid> gridLinks = new List<Grid>();
        List<Grid> gridRechts = new List<Grid>();
        List<Message> PlacedMessages = new List<Message>();
        DispatcherTimer timerSTA = new DispatcherTimer();
        GroupChat groupChat;
        public GroupchatWindow(int GroupChatId, List<Message> messages)
        {
            InitializeComponent();
            groupChat = GroepsChatQuerys.GetGroupChat(GroupChatId);
            InitializeTimer();
            PlaceInfo();
            foreach (Message message in messages)
            {
                PlaceMessage(message.SenderId == StaticProfile.UserProfile.Id, message);
            }
            try
            {
                UpdateProfilePic(Model.ProfielEnProfieldata.Images.Image.GetGroupProfilePicture(groupChat.ID).fileLocation);
            }
            catch (Exception ign)
            {

            }
            //check of de gebruiker gekicked is
            bool kicked = true;
            if (StaticProfile.UserProfile.Id == GroepsChatQuerys.GetGroupChat(groupChat.ID).AdminID)
            {
                kicked = false;
            }
            else
            {
                foreach (Profile profile in GroepsChatQuerys.GetMembersList(GroupChatId))
                {
                    if (profile.Id.Equals(StaticProfile.UserProfile.Id))
                    {
                        kicked = false;
                    }
                }
            }



            if (kicked)
            {
                chatInput.IsEnabled = false;
                chatInput.Text = "Je bent geen lid meer van deze groep.";
                Verstuurberichten.IsEnabled = false;
                settings.MouseDown -= settings_MouseDown;
                settings.MouseDown += settings_MouseDown2;
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
            List<Message> messages = GroepsChatQuerys.GetGroupChatMessages(groupChat.ID);
            foreach (Message message in messages)
            {
                if (!PlacedMessages.Exists(x => x.DateTime == message.DateTime || x.MessageContent.Equals(message.MessageContent) || x.SenderId == message.SenderId))
                {
                    PlaceMessage(message.SenderId == StaticProfile.UserProfile.Id, message);
                    PlacedMessages.Add(message);
                }
            }
        }
        private void PlaceInfo()
        {
            title.Content = groupChat.GroupName;
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
            messageGrid.Height += 150;
            scrollBar.ScrollToVerticalOffset(messageGrid.Height);

            string newLineMessage = NewLineMessage(message.MessageContent);
            int lines = newLineMessage.Split('\n').Length + 1;

            TextBlock text = new TextBlock
            {
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = message.MessageContent,
                Foreground = new SolidColorBrush(Colors.White),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18,
                Margin = new Thickness(5, 0, 0, 0),
                Width = 350
            };

            Label DateTimeLabel = new Label
            {
                Content = message.DateTime,
                FontSize = 12,
                Width = 350,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 30,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            Label NameLabel = new Label
            {
                Content = user ? $"{StaticProfile.UserProfile.DisplayName}" : (message.SenderId == - 1) ? "DreamTeam" : $"{ProfileQuerys.GetName(message.SenderId)}",
                FontSize = 12,
                Width = 350,
                Height = 30,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = new SolidColorBrush(Colors.White)
            };

            Grid LabelGrid = new Grid
            {
                Height = 100,
                Width = 350,
                Background = (message.SenderId == -1) ? new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#5077BE"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0) : new LinearGradientBrush((Color)ColorConverter.ConvertFromString("#FFF288A4"), (Color)ColorConverter.ConvertFromString("#FFF2AEC1"), 0)
            };

            LabelGrid.Children.Add(NameLabel);
            LabelGrid.Children.Add(text);
            LabelGrid.Children.Add(DateTimeLabel);

            LabelGrid.Margin = user ? new Thickness(1000, messageGrid.Height - 10, 100, 0) : new Thickness(0, messageGrid.Height - 10, (message.SenderId == -1) ? 500 : 1000, 0);

            if (user)
            {
                LabelGrid.Name = $"i{message.MessageId}i{groupChat.ID}";
                LabelGrid.MouseLeftButtonUp += LabelGrid_MouseLeftButtonUp;
                gridRechts.Add(LabelGrid);
            }
            else
            {
                gridLinks.Add(LabelGrid);
            }
            foreach (Grid grid in gridRechts)
            {
                grid.Margin = new Thickness(1000, grid.Margin.Top - (lines * 100), 100, 0);
            }
            foreach (Grid grid in gridLinks)
            {
                //grid.Margin = new Thickness(0, grid.Margin.Top - (lines * 100), 1000, 0);
                grid.Margin = new Thickness(0, grid.Margin.Top - (lines * 100), (message.SenderId == -1) ? 500 : 1000, 0);
            }
            messageGrid.Children.Add(LabelGrid);
        }

        private void LabelGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            PageRegistry.ChatChangeMessage = new Modals.ChatChangeMessage(grid, true);
            PageRegistry.ChatChangeMessage.Show();
        }

        private void VerstuurBericht_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(chatInput.Text))
            {
                
                
                if (GroepsChatQuerys.SendMessage(groupChat.ID, chatInput.Text, StaticProfile.UserProfile.Id))
                {
                    Message message = new Message(StaticProfile.UserProfile.Id, chatInput.Text, DateTime.Now, ChatQuerys.GetLastAiId(StaticProfile.UserProfile.Id, true));
                    PlaceMessage(message.SenderId == StaticProfile.UserProfile.Id, message);
                    chatInput.Text = "";
                }
                else
                {

                }
            }
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
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DreamTeam.Windows.Search searchwindow = new Search();
            searchwindow.Show();
            this.Close();
        }

        private void GroupchatButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.GroupchatWindowOverview = new GroupchatWindowOverview();
            PageRegistry.GroupchatWindowOverview.Show();
            this.Hide();
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        private void settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GroupchatSettings window = new GroupchatSettings(groupChat.ID, this);
            window.Show();
        }

        private void settings_MouseDown2(object sender, MouseButtonEventArgs e)
        {
            GroupChatLeave window = new GroupChatLeave(groupChat.ID);
            window.Show();
        }

        public void UpdateProfilePic(string Location)
        {
            ProfilePic.Source = new Uri(Location, UriKind.Absolute);
        }
    }
}
