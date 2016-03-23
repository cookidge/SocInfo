using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocInfo.Models
{
    public class Position
    { 
        public int rank { get; set; }
        public string team { get; set; }
        public string crestURI { get; set; }
        public int teamId { get; set; }
        public int playedGames { get; set; }
        public int points { get; set; }
        public int goals { get; set; }
        public int goalsAgainst { get; set; }
        public int goalDifference { get; set; }
    }
}