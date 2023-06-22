using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model.ProfielEnProfieldata
{
    public class Rank
    {
        public int RankId { get; set; }
        public string NaamRank { get; set; }
        public int Priority { get; set; }

        public Rank(int rankId, string naamRank, int priority)
        {
            RankId = rankId;
            NaamRank = naamRank;
            Priority = priority;
        }

        public Rank(string naam)
        {
            NaamRank = naam;
        }

        public override string ToString()
        {
            return NaamRank;
        }
    }
}
