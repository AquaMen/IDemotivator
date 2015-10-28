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

        public JsonResult PreRating(string r, string s, string id, string url)
        {
            int autoId = 0;
            Int16 thisVote = 0;
            Int16.TryParse(r, out thisVote);
            int.TryParse(id, out autoId);
            var Cir = User.Identity.GetUserId();
            var isIt = db.rates.Where(v => v.AspNetUserId == Cir && v.DemotivatorId == autoId).FirstOrDefault();
            if (isIt != null)
            {
                // keep the school voting flag to stop voting by this member
                HttpCookie cookie = new HttpCookie(url, "true");
                Response.Cookies.Add(cookie);
                return Json("<br />You have already rated this post, thanks !");
            }
            return Json("");
        }


        public JsonResult SendRating(string r, string s, string id, string url)
        {
            int autoId = 0;
            Int16 thisVote = 0;
            Int16.TryParse(r, out thisVote);
            int.TryParse(id, out autoId);

            if (!User.Identity.IsAuthenticated)
            {
                return Json("Not authenticated!");
            }

            if (autoId.Equals(0))
            {
                return Json("Sorry, record to vote doesn't exists");
            }

                    // check if he has already voted
                      var Cir = User.Identity.GetUserId();
                    var isIt = db.rates.Where(v => v.AspNetUserId == Cir && v.DemotivatorId == autoId).FirstOrDefault();
                    if (isIt != null)
                    {
                        // keep the school voting flag to stop voting by this member
                        HttpCookie cookie = new HttpCookie(url, "true");
                        Response.Cookies.Add(cookie);
                        return Json("<br />You have already rated this post, thanks !");
                    }

                    var sch = db.Demotivators.Where(sc => sc.Id == autoId).FirstOrDefault();
                    if (sch != null)
                    {
                        object obj = sch.Rate;

                        string updatedVotes = string.Empty;
                        string[] votes = null;
                        if (obj != null && obj.ToString().Length > 0)
                        {
                            string currentVotes = obj.ToString(); // votes pattern will be 0,0,0,0,0
                            votes = currentVotes.Split(',');
                            // if proper vote data is there in the database
                            if (votes.Length.Equals(5))
                            {
                                // get the current number of vote count of the selected vote, always say -1 than the current vote in the array 
                                int currentNumberOfVote = int.Parse(votes[thisVote - 1]);
                                // increase 1 for this vote
                                currentNumberOfVote++;
                                // set the updated value into the selected votes
                                votes[thisVote - 1] = currentNumberOfVote.ToString();
                            }
                            else
                            {
                                votes = new string[] { "0", "0", "0", "0", "0" };
                                votes[thisVote - 1] = "1";
                            }
                        }
                        else
                        {
                            votes = new string[] { "0", "0", "0", "0", "0" };
                            votes[thisVote - 1] = "1";
                        }

                        // concatenate all arrays now
                        foreach (string ss in votes)
                        {
                            updatedVotes += ss + ",";
                        }
                        updatedVotes = updatedVotes.Substring(0, updatedVotes.Length - 1);

                        db.Entry(sch).State = EntityState.Modified;
                        sch.Rate = updatedVotes;
                        db.SaveChanges();

                        rate vm = new rate()
                        {
                            AspNetUserId = User.Identity.GetUserId(),
                            Count = thisVote,
                            DemotivatorId = autoId,
                             IsRate = true
                        };

                        db.rates.Add(vm);

                        db.SaveChanges();

                        // keep the school voting flag to stop voting by this member
                        HttpCookie cookie = new HttpCookie(url, "true");
                        Response.Cookies.Add(cookie);
            }
            return Json("<br />You rated " + r + " star(s), thanks !");
        }
    }
}