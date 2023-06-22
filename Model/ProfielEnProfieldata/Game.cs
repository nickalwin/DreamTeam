using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Model.ProfielEnProfieldata;
using Model.ProfielEnProfieldata;
using Model.SQLData;

namespace Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rank Rank { get; set; }
        public Playstyle Playstyle { get; set; }

        public Game(int id, string name, Rank rank, Playstyle playstyle)
        {
            Id = id;
            Name = name;
            Rank = rank;
            Playstyle = playstyle;
        }

        public Game(int id)
        {
            Id = id;
            Name = ProfileQuerys.infoGame(id);
        }

        public Game(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Game(string name, string rank)
        {
            Name = name;
            Rank = new Rank(rank);
        }
    }
}
