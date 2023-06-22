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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DreamTeam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResetWindows();
        }

        private void ResetWindows()
        {
            PageRegistry.registerScreen = new Register();
            PageRegistry.loginScreen = new Login();

            PageRegistry.registerScreen.Closed += OnWindowClosed;
            PageRegistry.loginScreen.Closed += OnWindowClosed;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            this.Show();

            ResetWindows();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            /*loginScreen.Show();
            this.Hide();*/

            this.Hide();
            PageRegistry.loginScreen.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            PageRegistry.registerScreen.Show();
            this.Hide();
        }
    }
}
