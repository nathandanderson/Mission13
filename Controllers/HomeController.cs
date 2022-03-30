using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    { 
        private BowlersDbContext _context { get; set; }
        public HomeController(BowlersDbContext temp)
        {
            _context = temp;
        }


        public IActionResult Index()
        {
            ViewBag.Teams = _context.Teams.OrderBy(x => x.TeamName).ToList();
            ViewBag.Name = "All Bowlers";

            var blah = _context.Bowlers
                .Include(x => x.Team)
                .ToList();

            return View(blah);
        }

         
        public IActionResult TeamView(string TeamName)
        {
            ViewBag.Teams = _context.Teams.OrderBy(x => x.TeamName).ToList();

            ViewBag.Name = RouteData?.Values["teamname"];

            var blah = _context.Bowlers
                .Include(x => x.Team)
                .Where(x => x.Team.TeamName == TeamName)
                .ToList();
            return View("Index", blah);
        }


        [HttpGet]
        public IActionResult NewBowler()
        {
            ViewBag.Teams = _context.Teams.OrderBy(x => x.TeamName).ToList();

            return View();
        }
        [HttpPost]
        public IActionResult NewBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                _context.Update(b);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Teams = _context.Teams.OrderBy(x => x.TeamName).ToList();
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit (int BowlerID)
        {
            ViewBag.Teams = _context.Teams.OrderBy(x => x.TeamName).ToList();
            var bowl = _context.Bowlers.Single(x => x.BowlerID == BowlerID);

            return View("NewBowler", bowl);
        }

        [HttpPost]
        public IActionResult Edit (Bowler b)
        {
            _context.Update(b);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }




        //Render the delete page for a given bowler
        [HttpGet]
        public IActionResult Delete(int BowlerID)
        {
            var task = _context.Bowlers.Single(x => x.BowlerID == BowlerID);

            return View(task);
        }

        //Actually delete the bowler after asking for confirmation
        [HttpPost]
        public IActionResult Delete(Bowler b)
        {
            _context.Bowlers.Remove(b);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
