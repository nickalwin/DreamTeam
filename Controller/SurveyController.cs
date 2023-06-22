using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace Controller
{
    public static class SurveyController
    {
        public static float ToxicityLevel = 0;
        public static int currentQuestion = -1;
        
        static Dictionary<string, Tuple<string, string, string, string>> questions = new Dictionary<string, Tuple<string, string, string, string>>
        {
            {
                "Wat is je favoriete speelstijl?", 
                new Tuple<string, string, string, string>("Ik doe mijn best", "Casual", "Beide", "Competatief")
            },
            {
                "Je vriend gaat bijna dood! Wat doe jij?", 
                new Tuple<string, string, string, string>("Ik switch naar healer", "Ik switch naar tank", "Ik loop snel weg en doe alsof ik niks zag", "Ik schreeuw dat hij slecht is.")
            },
            {
                "Je team word dik ingemaakt in een game. Wat doe jij?", 
                new Tuple<string, string, string, string>("Ik blijf positief! Kan gebeuren...", "Ik moedig mijn team aan.", "Ik roep mijn team aan om de game te leaven.", "Ik leave de party.")
            },
            {
                "Je speelt veel beter dan je team. Wat zeg je aan het einde?", 
                new Tuple<string, string, string, string>("Lekker bezig boys!", "GG, Goed potje", "Dat was wel heel makkelijk", "How to raise bot difficulty?")
            },
            {
                "Je merkt dat er een meisje in je team zit. Hoe reageer je?", 
                new Tuple<string, string, string, string>("Je geeft geen vreemde reactie","OMG, ben jij een meisje?", "Kan ik misschien je vriend-/in zijn?",  "Aan het werk, vrouw!")
            },
            {
                "Je word ingemaakt in een 1 versus 1. Wat is je go-to reactie?", 
                new Tuple<string, string, string, string>("Good Game, Well Played", "De controller heeft een afwijking!", "Hij speelt met hacks!", "Mijn internet is ineens erg sloom!")
            },
            {
                "Hoeveel spullen heb je kapotgemaakt tijdens gamen?", 
                new Tuple<string, string, string, string>("Niks! Ik ben voorzichtig.", "Ik koop alles bij de action.", "Zo nu een dan een controller, maar dat moet kunnen.", "Regelmatig, mijn salaris is snel op.")
            },
            {
                "Hoe duur is je setup?", 
                new Tuple<string, string, string, string>("Gratis, hij stond naast de weg.", "Gewoon, gemiddeld.","Ik wil het er niet over hebben, maar hij heeft veel lampies.", "Geen idee, mam en pap hebben het betaald.")
            },
            {
                "Een russische tegenstander gebruikt opeens hacks. Wat doe je?", 
                new Tuple<string, string, string, string>("Je rapporteert hem en speelt door.", "Je verlaat het spel en zoekt een nieuw spel.", "Je scheld terug in het Russisch.", "Je DDoSed zijn internet.")
            },
            {
                "Er komt een vriend onverwacht op bezoek. Wat doe je?", 
                new Tuple<string, string, string, string>("Je maakt je game af, met je vriend die meekijkt.", "Je verlaat de game direct, je vriend is belangrijker.", "Je laat je vriend buiten staan.", "Je verlaat de game en gaat alleen op je kamer Netflix kijken.")
            }
        };
        public static void SurveyAnswered(Option picked)
        {
            ToxicityLevel += ((float)picked-1)/10;
            ToxicityLevel = (float)Math.Round(ToxicityLevel, 1);
        }

        public static CurrentQuestion NextQuestion()
        {
            currentQuestion++;

            // If question remaining
            if (currentQuestion < questions.Count)
            {
                return new CurrentQuestion
                {
                    QuestionNumber = currentQuestion + 1, Question = questions.ElementAt(currentQuestion).Key,
                    Answers = questions.ElementAt(currentQuestion).Value
                };
            }
            else
            {
                return null;
            }
        }
        public static void FinalizeScore()
        {
            ProfileQuerys.ProfileUpdateToxicityLevel(ProfileQuerys.ProfileGetId(StaticProfile.UserProfile.Email), ToxicityLevel);
        }
    }
    
    public class CurrentQuestion
    {
        public int QuestionNumber { get; set; }
        public string Question { get; set; }
        public Tuple<string, string, string, string> Answers { get; set; }
    }
}
