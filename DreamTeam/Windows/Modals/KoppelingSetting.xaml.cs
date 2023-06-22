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
    /// Interaction logic for KoppelingSetting.xaml
    /// </summary>
    public partial class KoppelingSetting : Window
    {
        public Connection Conn;

        public KoppelingSetting(Connection conn)
        {
            InitializeComponent();
            Conn = conn;

            KoppelingTitle.Content = conn.Platform + " Connectie";
            UsernameBox.Text = conn.Username;
            IsPublic.IsChecked = (conn.Visibility == ConnectionVisibility.Shown) ? true : false;
        }

        private void DeleteKoppeling_Click(object sender, RoutedEventArgs e)
        {
            Conn.RemoveConnection();
            this.Hide();

            PageRegistry.Koppelingen.UpdateConnections(true);
        }

        private void MakeChanges_Click(object sender, RoutedEventArgs e)
        {
            ConnectionVisibility newVis = IsPublic.IsChecked == true ? ConnectionVisibility.Shown: ConnectionVisibility.Hidden;
            // If username-box is not empty
            if(!string.IsNullOrWhiteSpace(UsernameBox.Text))
            {
                if(!UsernameBox.Text.Equals(Conn.Username))
                {
                    ConnectionQuerys.ChangeIGN(Conn.AccountID, Conn.PlatformID, UsernameBox.Text);
                    PageRegistry.Koppelingen.UpdateConnections(true);
                }
                
                if(newVis != Conn.Visibility)
                {
                    ConnectionQuerys.SetVisibility(Conn.AccountID, Conn.PlatformID, newVis);

                    PageRegistry.Koppelingen.UpdateConnections(true);
                }

                this.Hide();
            }
        }
    }
}
