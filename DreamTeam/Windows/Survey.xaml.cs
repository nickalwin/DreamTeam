using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Model;
using Controller;
using Model.ProfielEnProfieldata;

namespace DreamTeam
{
    /// <summary>
    /// Interaction logic for Survey.xaml
    /// </summary>
    public partial class Survey : Window
    {
        public Survey()
        {
            InitializeComponent();
            SurveyName.Content = StaticProfile.UserProfile.DisplayName;
            SetQuestion(SurveyController.NextQuestion());
        }

        void SetQuestion(CurrentQuestion question)
        {
            QuestionText.Content = question.Question;
            QuestionNumber.Content = "Vraag " + question.QuestionNumber;
            Radio1.Content = question.Answers.Item1;
            Radio2.Content = question.Answers.Item2;
            Radio3.Content = question.Answers.Item3;
            Radio4.Content = question.Answers.Item4;

            Radio1.IsChecked = false;
            Radio2.IsChecked = false;
            Radio3.IsChecked = false;
            Radio4.IsChecked = false;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (RadioButtons.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                Option optionPicked = Option.Undefined;

                int currentPos = 1;
                foreach (RadioButton child in RadioButtons.Children)
                {
                    if (child.IsChecked == true)
                        optionPicked = (Option)currentPos;

                    currentPos++;
                }

                // Fire Event with option picked
                SurveyController.SurveyAnswered(optionPicked);
                CurrentQuestion question = SurveyController.NextQuestion();
                if (question != null)
                {
                    SetQuestion(question);
                }
                else
                {
                    this.Hide();
                    PageRegistry.surveyAnswerScreen = new SurveyAnswer();
                    PageRegistry.surveyAnswerScreen.Show();
                }
            }
        }

    }
}
