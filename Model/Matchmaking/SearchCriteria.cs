using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Matchmaking
{
    public class SearchCriteria
    {
        public float toxicityLevel { get; set; }
        public List<Game> hasGames { get; set; }
        public Tuple<int, int> InclusiveAgeRange { get; set; }
        public Playstyle PlayStyle { get; set; }


        public SearchCriteria()
        {
            this.hasGames = new List<Game>();
            this.toxicityLevel = 0;
            InclusiveAgeRange = new Tuple<int, int>(0, 199);
            PlayStyle = Playstyle.GeenVoorkeur;
        }
    }
}
