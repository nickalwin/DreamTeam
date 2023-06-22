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
using Model;

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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(RadioButtons.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)))
            {
                Option optionPicked = Option.Undefined;

                int currentPos = 1;
                foreach(RadioButton child in RadioButtons.Children)
                {
                    if(child.IsChecked == true)
                        optionPicked = (Option)currentPos;

                    currentPos++;
                }

                // Fire Event with option picked
                PageRegistry.CallSurveyAnswered(this, new SurveyAnsweredArgs { picked = optionPicked });
            }
        }
    }
}
