using Controller;
using Model;
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

namespace DreamTeam
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            this.Height = SystemParameters.PrimaryScreenHeight * 0.95;
            this.Width = SystemParameters.PrimaryScreenWidth * 0.95;
        }
        private void PasswordInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordInput.Password))
            {
                PasswordLabel.Visibility = Visibility.Visible;
            }
        }
        private void PasswordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordLabel.Visibility = Visibility.Hidden;
        }
        private void EmailInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailInput.Text == "E-mailadres")
            {
                EmailInput.Text = "";
            }
        }
        private void EmailInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailInput.Text))
            {
                EmailInput.Text = "E-mailadres";
            }
        }
        private void EmailInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void MakeAcount_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PageRegistry.registerScreen = new Register();
            PageRegistry.registerScreen.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Profile.LoginUser(EmailInput.Text, PasswordInput.Password))
            {
                PageRegistry.loginScreen.Hide();

                if (StaticProfile.UserProfile.ToxicityLevel == -1)
                {
                    PageRegistry.surveyScreen = new Survey();
                    PageRegistry.surveyScreen.Show();
                }
                else
                {
                    MessagesTimer messagesTimer = new MessagesTimer();
                    messagesTimer.Initialize();
                    PageRegistry.profileScreen = new Windows.Profiel();
                    PageRegistry.profileScreen.Show();
                }
            }
            else
            {
                int ErrorID = Convert.ToByte(Profile.LoginError(EmailInput.Text, PasswordInput.Password));
                if ((ErrorID >> 0 & 1) == 1)
                {
                    EmailErrorLable.Content = "Email does not exist.";
                }
                else
                {
                    EmailErrorLable.Content = "";
                }
                if ((ErrorID >> 1 & 1) == 1)
                {
                    PasswordErrorLable.Content = "Password is wrong.";
                }
                else
                {
                    PasswordErrorLable.Content = "";
                }
            }
        }
    }
}
