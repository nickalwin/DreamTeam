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
            if(!string.IsNullOrWhiteSpace(EmailInput.Text) && !string.IsNullOrWhiteSpace(UsernameInput.Text) && !string.IsNullOrWhiteSpace(PasswordInput.Password) && !string.IsNullOrWhiteSpace(GenderInput.SelectedItem.ToString()))
            {
                string str = GenderInput.SelectedItem.ToString();
                bool returnVal;
                bool returnVal2;
                bool AgeError = false;
                int age = 0;
                try
                {
                    age = DateTime.Today.Year - agepicker.SelectedDate.Value.Year;
                    string agestring = "" + agepicker.SelectedDate.Value.Year + "-" + agepicker.SelectedDate.Value.Month +
                                       "-" + agepicker.SelectedDate.Value.Day;
                    returnVal = Profile.RegisterUser(UsernameInput.Text, PasswordInput.Password, EmailInput.Text, agestring, (Gender)Enum.Parse(typeof(Gender), str));
                    returnVal2 = ProfileQuerys.ProfileAddNationality(ProfileQuerys.ProfileGetId(EmailInput.Text), ProfileQuerys.GetNationalityId(NationalityInput.SelectedItem.ToString()));
                }
                catch (System.InvalidOperationException)
                {
                    returnVal = false;
                    returnVal2 = false;
                    AgeError = true;
                }
                

                if(returnVal && returnVal2)
                {
                    List<Nationality> nationalities = new List<Nationality>();
                    str = NationalityInput.SelectedItem.ToString();
                    nationalities.Add((Nationality)Enum.Parse(typeof(Nationality), str));
                    StaticProfile.UserProfile.Nationalities  = nationalities;
                    this.Hide();
                    PageRegistry.surveyScreen = new Survey();
                    PageRegistry.surveyScreen.Show();
                }
                if (!returnVal)
                {
                    bool Error = true;
                    bool NameError = false;
                    bool EmailError = false;
                    bool PasswordError = false;
                    bool GenderError = false;
                    bool NationalityError = false;
                    if (ProfileQuerys.UsernameExists(UsernameInput.Text))
                    {
                        NameError = true;
                    }
                    if (ProfileQuerys.EmailExists(EmailInput.Text))
                    {
                        EmailError = true;
                    }
                    if (!ProfileQuerys.PasswordCorrect(PasswordInput.Password))
                    {
                        PasswordError = true;
                    }
                    if (age < 16 || age > 199)
                    {
                        AgeError = true;                        
                    }
                    if (GenderInput.SelectedItem.Equals(null))
                    {
                        GenderError = true;
                    }
                    if (returnVal2)
                    {
                        NationalityError = true;
                    }
                    if (Error)
                    {
                        int ErrorID = Convert.ToByte(Profile.RegisterError(NameError, EmailError, ProfileQuerys.IsValidEmail(EmailInput.Text), PasswordError, AgeError, GenderError, NationalityError));
                        if ((ErrorID >> 6 & 1) == 1)
                        {
                            UsernameErrorLable.Content = "Username already exists.";
                        }
                        else
                        {
                            UsernameErrorLable.Content = "";
                        }
                        if ((ErrorID >> 5 & 1) == 1)
                        {
                            EmailErrorLable.Content = "Email already exists.";
                        }
                        else
                        {
                            EmailErrorLable.Content = "";
                        }
                        if ((ErrorID >> 4 & 1) == 1)
                        {
                            EmailErrorLable.Content = "Email is not valid.";
                        }
                        if ((ErrorID >> 3 & 1) == 1)
                        {
                            PasswordErrorLable.Content = "Password needs to be at least 8 caracters long.";
                        }
                        else
                        {
                            PasswordErrorLable.Content = "";
                        }
                        if ((ErrorID >> 12 & 1) == 1)
                        {
                            AgeErrorLable.Content = "You need to be between 16 - 199 years old.";
                        }
                        else
                        {
                            AgeErrorLable.Content = "";
                        }
                        if ((ErrorID >> 1 & 1) == 1)
                        {
                            GenderErrorLable.Content = "Please select a gender";
                        }
                        else
                        {
                            GenderErrorLable.Content = "";
                        }
                        if ((ErrorID >> 0 & 1) == 1)
                        {
                            NationalityErrorLable.Content = "Please select a nationality";
                        }
                        else
                        {
                            NationalityErrorLable.Content = "";
                        }
                    }
                }

            }
            else
            {
                UsernameErrorLable.Content = "Not all information filled in.";
            }
        }


        private void LoginAnyway_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PageRegistry.loginScreen = new Login();
            PageRegistry.loginScreen.Show();
        }
    }
}
