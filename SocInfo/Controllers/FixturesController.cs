using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SocInfo.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq;

namespace SocInfo.Controllers
{
    public class FixturesController : Controller
    {
        // GET: Index
        public ActionResult Index(string date, string day)
        {
            if (date == null)
            {
                date = DateTime.Now.ToString("dd/MM/yyyy");
                day = DateTime.Now.DayOfWeek.ToString();

            }
            
            // Set days
            ViewBag.Day1 = DateTime.Now.DayOfWeek.ToString();
            ViewBag.Day2 = DateTime.Now.AddDays(1).DayOfWeek.ToString();
            ViewBag.Day3 = DateTime.Now.AddDays(2).DayOfWeek.ToString();
            ViewBag.Day4 = DateTime.Now.AddDays(3).DayOfWeek.ToString();
            ViewBag.Day5 = DateTime.Now.AddDays(4).DayOfWeek.ToString();
            ViewBag.Day6 = DateTime.Now.AddDays(5).DayOfWeek.ToString();
            ViewBag.Day7 = DateTime.Now.AddDays(6).DayOfWeek.ToString();

            // Set dates
            ViewBag.Date1 = DateTime.Now.ToShortDateString();
            ViewBag.Date2 = DateTime.Now.AddDays(1).ToShortDateString();
            ViewBag.Date3 = DateTime.Now.AddDays(2).ToShortDateString();
            ViewBag.Date4 = DateTime.Now.AddDays(3).ToShortDateString();
            ViewBag.Date5 = DateTime.Now.AddDays(4).ToShortDateString();
            ViewBag.Date6 = DateTime.Now.AddDays(5).ToShortDateString();
            ViewBag.Date7 = DateTime.Now.AddDays(6).ToShortDateString();

            ViewBag.CurrentDay = day;
            ViewBag.CurrentDate = date;

            return View(DateFixtures(date));
        }

        // GET: Date fixtures
        public List<Fixture> DateFixtures(string date)
        {
            // Return in order of date
            return DateFixtureRequest("/v1/soccerseasons/398/fixtures?timeFrame=n7", date);
        }

        // TeamFixtureRequest method
        // Request method that accepts a teamId (string)
        // and returns a lsit of their fixtures for the current
        // season
        static List<Fixture> TeamFixtureRequest(int teamId)
        {
            // Declare list of fixtures
            List<Fixture> fixtures = new List<Fixture>();

            // Retrieve fixtures for today from API
            RestClient client = new RestClient("http://api.football-data.org");

            // Create request
            RestRequest request = new RestRequest("", Method.GET);
            request.AddHeader("X-Auth-Token", "dd3e9cffe47c403e8b485b7a536874e0");
            request.AddHeader("X-Response-Control", "minified");

            // Retrieve response
            JObject returnedFixtures = JObject.Parse(client.Execute(request).Content);

            // Add each fixture to fixture list
            foreach (JToken fixtureToken in returnedFixtures["fixtures"])
            {

            }

            return fixtures.OrderBy(o => o.date).ToList();
        }

        // DateFixtureRequest method
        // Request method that accepts a request string
        // build to handle returning fixtures and therefore
        // passes back a list of fixtures that match the date
        static List<Fixture> DateFixtureRequest(string requestString, string date)
        {
            // Declare list of fixtures
            List<Fixture> fixtures = new List<Fixture>();

            // Retrieve fixtures for today from API
            RestClient client = new RestClient("http://api.football-data.org");

            // Create request
            RestRequest request = new RestRequest(requestString, Method.GET);
            request.AddHeader("X-Auth-Token", "dd3e9cffe47c403e8b485b7a536874e0");
            request.AddHeader("X-Response-Control", "minified");

            // Retrieve response
            JObject returnedFixtures = JObject.Parse(client.Execute(request).Content);

            // Add each fixture to fixture list
            foreach (JToken fixtureToken in returnedFixtures["fixtures"])
            {
                DateTime strFixtureDate = (DateTime)fixtureToken["date"];

                if (date == strFixtureDate.ToShortDateString())
                {
                    fixtures.Add(ParseFixture(fixtureToken));
                }            
            }

            return fixtures.OrderBy(o => o.date).ToList();
        }

        // ParseFixture method
        // --------------------
        // It accepts a json token (with a single fixture contained)
        // and casts it to a custom Fixure object and then returns that
        // object.
        public static Fixture ParseFixture(JToken fixture)
        {
            // Add Fixture details
            Fixture newFixture = new Fixture();
            newFixture.id = (int)fixture["id"];
            newFixture.soccerseasonId = (int)fixture["soccerseasonId"];
            newFixture.date = (string)fixture["date"];
            newFixture.matchday = (int)fixture["matchday"];
            newFixture.homeTeamName = (string)fixture["homeTeamName"];
            newFixture.homeTeamId = (int)fixture["homeTeamId"];
            newFixture.awayTeamName = (string)fixture["awayTeamName"]; ;
            newFixture.awayTeamId = (int)fixture["awayTeamId"];

            // Add result
            JToken results = fixture["result"];

            if ((string)results["goalsHomeTeam"] != null)
            {
                newFixture.completed = true;
                newFixture.homeTeamGoals = (int)results["goalsHomeTeam"];
                newFixture.awayTeamGoals = (int)results["goalsAwayTeam"];
            }
            else
            {
                newFixture.completed = false;
            }

            // Add fixture
            return newFixture;
        }
    }
}