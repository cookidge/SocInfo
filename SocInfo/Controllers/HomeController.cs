using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocInfo.Models;

namespace SocInfo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Position> leagueModel = GetLeaguePositions();
            List<Fixture> fixtureModel = GetFixtures();
            List<Team> teamModel = GetTeams();

            return View(Tuple.Create(leagueModel,fixtureModel,teamModel));
        }

        public List<Position> GetLeaguePositions()
        {
            LeagueController leagueController = new LeagueController();
            return leagueController.GetLeaguePositions();
        }

        public List<Fixture> GetFixtures()
        {
            FixturesController fixtureController = new FixturesController();
            return fixtureController.DateFixtures(DateTime.Now.ToString("dd/MM/yyyy"));
        }

        public List<Team> GetTeams()
        {
            TeamsController teamsController = new TeamsController();
            return teamsController.TeamsRequest();
        }

    }
}