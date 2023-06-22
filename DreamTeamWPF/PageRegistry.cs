using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamTeam
{
    static public class PageRegistry
    {
        public static Register registerScreen;
        public static Login loginScreen;
        public static Survey surveyScreen;

        public static event EventHandler<SurveyAnsweredArgs> SurveyAnswered;

        public static void CallSurveyAnswered(object sender, SurveyAnsweredArgs args)
        {
            SurveyAnswered?.Invoke(sender, args);
        }
    }

}
