using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using IDemotivator;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IDemotivator.Models;


namespace IDemotivator.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        public async Task<ActionResult> Index()
        {
            var demotivators = db.Demotivators.Include(s => s.AspNetUser);
            return View(await demotivators.ToListAsync());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}