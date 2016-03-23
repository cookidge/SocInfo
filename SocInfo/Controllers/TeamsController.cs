using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SocInfo.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq;

namespace SocInfo.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        public ActionResult Index()
        {
            return View(TeamsRequest());
        }

        // TeamsRequest method
        // -------------------
        // Retrieves a json object containing teams
        // and then parses each team and adds to a list
        // of teams which is returned
        public List<Team> TeamsRequest()
        {
            // Declare list of teams
            List<Team> teams = new List<Team>();

            // Retrieve teams from
            RestClient client = new RestClient("http://api.football-data.org");

            // Create request
            RestRequest request = new RestRequest("/v1/soccerseasons/398/teams", Method.GET);
            request.AddHeader("X-Auth-Token", "dd3e9cffe47c403e8b485b7a536874e0");
            request.AddHeader("X-Response-Control", "minified");

            // Retrieve response
            JObject returnedTeams = JObject.Parse(client.Execute(request).Content);

            // Add each fixture to fixture list
            foreach (JToken teamToken in returnedTeams["teams"])
            {
                teams.Add(ParseTeam(teamToken));
            }

            return teams.OrderBy(o => o.name).ToList();
        }

        // ParseTeam method
        // --------------------
        // It accepts a json token (with a single team contained)
        // and casts it to a custom Team object and then returns that
        // object.
        public Team ParseTeam(JToken team)
        {
            // Add Team
            Team newTeam = new Team();
            newTeam.id = (int)team["id"];
            newTeam.name = (string)team["name"];
            newTeam.shortName = (string)team["shortName"];
            newTeam.squadMarketValue = (string)team["squadMarketValue"];
            newTeam.crestUrl = (string)team["crestUrl"];

            // Add fixture
            return newTeam;
        }

        // GetTeamInfo method
        // --------------------
        // It accepts a team name and the required piece of information
        // and returns the result
        public string GetTeamInfo(string teamName, string info)
        {
            string result = "";
            
            foreach (Team team in TeamsRequest())
            {
                if (team.name == teamName)
                {
                    result = team.crestUrl;
                }
            }

            return result;
        }
    }
}