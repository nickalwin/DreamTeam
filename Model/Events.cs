using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    static class Events
    {
        
    }

    public class SurveyAnsweredArgs : EventArgs
    {
        public Option picked;
    }

    public enum Option
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Undefined = 5
    }
}
