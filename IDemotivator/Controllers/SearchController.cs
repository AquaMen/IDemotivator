using IDemotivator.Models;
using IDemotivator.Search;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDemotivator.Controllers
{
    public class SearchController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index(string r)
        {
            SearchViewModel model = new SearchViewModel();
            if (r == null || r.Length == 0)
                return View(model);
            List<Demotivator> demotivatorList = new List<Demotivator>();
            List<ApplicationUser> userList = new List<ApplicationUser>();
            using (var elastic = new elasticsearchNEST())
            {
                var demotivators = elastic.SearchDemotivators(r);
                var users = elastic.SearchUser(r);
                if (demotivators.Count() != 0) {
                    foreach (var dem in demotivators) {
                        demotivatorList.Add(dem);
                    }
                }
                if (users.Count() != 0) {
                    foreach (var user in users)
                    {
                        userList.Add(user);
                    }
                }
            }

            model.demotivators = demotivatorList;
            model.User = userList;
            return View(model);
        }



        [HttpPost]
        public JsonResult Search(string term)
        {
            List<string> jsonka = new List<string>();
            using (var elastic = new elasticsearchNEST())
            {
                var resultDem = elastic.SearchDemotivators(term);
                var resultUser = elastic.SearchUser(term);

                foreach (var user in resultUser)
                {
                    jsonka.Add(user.UserName);
                }
                foreach (var dem in resultDem) {
                    jsonka.Add(dem.Name);
                }

            }
            jsonka = jsonka.Distinct().ToList();
            return Json(jsonka, JsonRequestBehavior.AllowGet);
        }

    }
}