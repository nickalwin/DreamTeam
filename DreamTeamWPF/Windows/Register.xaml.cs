using Model;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void UsernameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameInput.Text == "Gebruikersnaam")
            {
                UsernameInput.Text = "";
            }
            
        }

        private void UsernameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameInput.Text))
            {
                UsernameInput.Text = "Gebruikersnaam";
            }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(EmailInput.Text) && !string.IsNullOrWhiteSpace(UsernameInput.Text) && !string.IsNullOrWhiteSpace(PasswordInput.Password))
            {
                bool returnVal = Profile.RegisterUser(UsernameInput.Text, PasswordInput.Password, EmailInput.Text, 0, Gender.Anders);

                if(returnVal)
                {
                    this.Hide();
                    PageRegistry.surveyScreen = new Survey();
                    PageRegistry.surveyScreen.Show();
                }
            }
        }
    }
}
