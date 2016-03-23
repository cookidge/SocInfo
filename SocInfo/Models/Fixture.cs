using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocInfo.Models
{
    public class Fixture
    {
        public int id { get; set; }
        public int soccerseasonId { get; set; }
        public string date { get; set; }
        public int matchday { get; set; }
        public string homeTeamName { get; set; }
        public int homeTeamId { get; set; }
        public string awayTeamName { get; set; }
        public int awayTeamId { get; set; }
        public int homeTeamGoals { get; set; }
        public int awayTeamGoals { get; set; }
        public bool completed { get; set; }
    }
}