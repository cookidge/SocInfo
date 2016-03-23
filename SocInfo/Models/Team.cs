using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocInfo.Models
{
    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string squadMarketValue { get; set; }
        public string crestUrl { get; set; }
    }

    public class Player
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public int jerseyNumber { get; set; }
        public string dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string contractUntil { get; set; }
        public string marketValue { get; set; }
    }
}