using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocInfo.Models;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace SocInfo.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Index(string team)
        {
            team = (team == "") ? "Newcastle United FC" : team;

            ViewBag.TeamName = team;

            Team teamModel = GetTeamInfo(team);

            ViewBag.TeamId = teamModel.id;

            List <Player> playerModel = GetTeamPlayers(teamModel.id);
            List<Fixture> fixtureModel = GetTeamFixtures(teamModel.id);
            
            return View(Tuple.Create(playerModel, fixtureModel, teamModel));

        }

        // GET: Team Info
        public Team GetTeamInfo(string team)
        {
            TeamsController teamController = new TeamsController();
            Team currentTeam = new Team();

            foreach (Team curTeam in teamController.TeamsRequest())
            {
                if (team == curTeam.name)
                {
                    currentTeam = curTeam;
                }
            }

            return currentTeam;
        }

        // Player Info
        public List<Player> GetTeamPlayers(int teamId)
        {
            // Create list of players
            List<Player> teamPlayers = new List<Player>();

            // Retrieve positions from API
            RestClient client = new RestClient("http://api.football-data.org");

            // Create request
            RestRequest request = new RestRequest("/v1/teams/" + teamId + "/players", Method.GET);
            request.AddHeader("X-Auth-Token", "dd3e9cffe47c403e8b485b7a536874e0");
            request.AddHeader("X-Response-Control", "minified");

            // Retrieve response
            JObject returnedPlayers = JObject.Parse(client.Execute(request).Content);

            // Add each player  to list
            foreach (JToken playerToken in returnedPlayers["players"])
            {
                teamPlayers.Add(ParsePlayer(playerToken));
            }

            return teamPlayers;
        }

        // ParsePlayer method
        // --------------------
        static Player ParsePlayer(JToken player)
        {
            // Add Fixture details
            Player newPlayer = new Player();
            newPlayer.id = (int)player["id"];
            newPlayer.name = (string)player["name"];
            newPlayer.position = (string)player["position"];
            newPlayer.jerseyNumber = (int?)player["jerseyNumber"] ?? 0;
            newPlayer.dateOfBirth = (string)player["dateOfBirth"];
            newPlayer.nationality = (string)player["nationality"];
            newPlayer.contractUntil = (string)player["contractUntil"];
            newPlayer.marketValue = (string)player["marketValue"];

            // Add player
            return newPlayer;
        }

        // Fixture Info
        public List<Fixture> GetTeamFixtures(int teamId)
        {
            // Create list of players
            List<Fixture> teamFixtures = new List<Fixture>();

            // Retrieve positions from API
            RestClient client = new RestClient("http://api.football-data.org");

            // Create request
            RestRequest request = new RestRequest("/v1/teams/" + teamId + "/fixtures", Method.GET);
            request.AddHeader("X-Auth-Token", "dd3e9cffe47c403e8b485b7a536874e0");
            request.AddHeader("X-Response-Control", "minified");

            // Retrieve response
            JObject returnedFixtures = JObject.Parse(client.Execute(request).Content);

            // Add each fixture to list
            foreach (JToken playerToken in returnedFixtures["fixtures"])
            {
                teamFixtures.Add(FixturesController.ParseFixture(playerToken));
            }

            return teamFixtures.OrderByDescending(d => d.matchday).ToList();
        }
    }
}