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
    public class LeagueController : Controller
    {
        // GET: LeagueTable
        public ActionResult Index()
        {
            return View(GetLeaguePositions());
        }

        // LeaguePositions method
        public List<Position> GetLeaguePositions()
        {
            // Create list of type position
            List<Position> leaguePositions = new List<Position>();

            // Retrieve positions from API
            RestClient client = new RestClient("http://api.football-data.org");

            // Create request
            RestRequest request = new RestRequest("/v1/soccerseasons/426/leagueTable", Method.GET);
            request.AddHeader("X-Auth-Token", "dd3e9cffe47c403e8b485b7a536874e0");
            request.AddHeader("X-Response-Control", "minified");

            // Retrieve response
            JObject returnedPositions = JObject.Parse(client.Execute(request).Content);

            // Add each fixture to fixture list
            foreach (JToken positionToken in returnedPositions["standing"])
            {
                    leaguePositions.Add(ParsePosition(positionToken));
            }

            return leaguePositions;
        }

        public Position ParsePosition(JToken position)
        {
            // Create new position
            Position newPosition = new Position();
            newPosition.rank = (int)position["rank"];
            newPosition.teamId = (int)position["teamId"];
            newPosition.team = (string)position["team"];
            newPosition.crestURI = (string)position["crestURI"];
            newPosition.playedGames = (int)position["playedGames"];
            newPosition.points = (int)position["points"];
            newPosition.goals = (int)position["goals"];
            newPosition.goalsAgainst = (int)position["goalsAgainst"];
            newPosition.goalDifference = (int)position["goalDifference"];

            return newPosition;
        }


    }
}