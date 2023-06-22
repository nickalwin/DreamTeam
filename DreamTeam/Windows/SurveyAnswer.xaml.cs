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
using System.Threading;
using DreamTeam.Windows;

namespace DreamTeam
{
    /// <summary>
    /// Interaction logic for Survey.xaml
    /// </summary>
    public partial class SurveyAnswer : Window
    {

        public SurveyAnswer()
        {
            InitializeComponent();

            StepScore();
        }

        void StepScore()
        {
            ScoreNum.Content = SurveyController.ToxicityLevel + "/3 Score";
            if (SurveyController.ToxicityLevel < 1.5)
            {
                ScoreUitleg.Content = "Een lage score is een vriendelijkere speelstijl.";
            }
            else
            {
                ScoreUitleg.Content = "Een hoge score is een fanatiekere speelstijl.";
            }
            SurveyController.FinalizeScore();
        }

        /// <summary>
        /// Will be called when pressing 'complete' button in SurveyAnswer
        /// Should log the user in and open the profile window,
        /// along with closing this window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PageRegistry.profileScreen = new Profiel();
            PageRegistry.profileScreen.Show();
        }
    }
}
