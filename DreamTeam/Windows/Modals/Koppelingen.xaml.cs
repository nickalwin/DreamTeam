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

namespace DreamTeam.Windows.Modals
{
    /// <summary>
    /// Interaction logic for Koppelingen.xaml
    /// </summary>
    public partial class Koppelingen : Window
    {
        public List<Connection> connections;

        public Koppelingen()
        {
            InitializeComponent();

            UpdateConnections(true);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(UsernameBox.Text) && !UsernameBox.Text.Equals("Gebruikersnaam"))
            {
                ConnectionVisibility visibility = MakePublic.IsChecked == true ? ConnectionVisibility.Shown : ConnectionVisibility.Hidden;
                string PlatformName = PlatformSelector.Text;
                int PlatformID = StaticProfile.Platforms.FirstOrDefault(x => x.Value == PlatformName).Key;
                Connection.CreateConnection(PlatformID, UsernameBox.Text, visibility);
                UpdateConnections(true);
            }
        }

        private void UsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text == "Gebruikersnaam")
            {
                UsernameBox.Text = "";
            }
        }

        private void UsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameBox.Text))
            {
                UsernameBox.Text = "Gebruikersnaam";
            }
        }

        /// <summary>
        /// Updates all the current Profile Connections
        /// </summary>
        public void UpdateConnections(bool RetrieveFromDB = false)
        {
            ScrollViewGrid.Height = 325;
            ScrollViewGrid.Children.Clear();

            if (RetrieveFromDB)
                connections = ConnectionQuerys.getProfileConnections(StaticProfile.UserProfile.Id);

            int topMargin = 0;

            BitmapImage cog = new BitmapImage(new Uri(@"Assets/img/cog_icon_125323.png", UriKind.Relative));

            foreach (Connection conn in connections)
            {
                Grid Knuffelaar = new Grid { Height = 40, Width = 420 };
                ScrollViewGrid.Height += 40;

                Knuffelaar.Children.Add(new Label { Content = conn.Platform, FontSize = 18, Width = 100, Height = 40 });

                System.Windows.Controls.Button changeButton = new System.Windows.Controls.Button
                {
                    Background = new SolidColorBrush(Colors.Red),
                    Foreground = new SolidColorBrush(Colors.White),
                    BorderBrush = null,
                    FontSize = 18,
                    Name = conn.Platform,
                    Margin = new Thickness(140, 0, 0, 0),
                    Width = 40
                };

                changeButton.Background = new ImageBrush(cog);

                changeButton.Click += ChangeButton_Click;

                Knuffelaar.Children.Add(changeButton);

                Knuffelaar.Margin = new Thickness(0, topMargin, 90, 10);

                ScrollViewGrid.Children.Add(Knuffelaar);
                topMargin += (int)Knuffelaar.Height + 40;
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string platformName = button.Name;

            KoppelingSetting setting = new KoppelingSetting(connections.FirstOrDefault(x => x.Platform == platformName));
            setting.Show();
        }
    }
}
