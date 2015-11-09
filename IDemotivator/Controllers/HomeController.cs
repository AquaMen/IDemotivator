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
using IDemotivator.Filters;
using Nest;

namespace IDemotivator.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index()
        {
            HomeViewModel home = new HomeViewModel();
            var tags = db.tags.ToList();
            var demotivators = db.Demotivators.Include(s => s.AspNetUser).ToList();
            var TopNewDemotivators = demotivators.OrderByDescending(d => d.Date).Take(9).ToList();
            var TopRateDemotivators = demotivators.OrderByDescending(r => r.RateCount).Take(9).ToList();
            var TopDiscusDemotivators = demotivators.OrderByDescending(d => d.Comments.Count()).Take(9).ToList();
            home.TopDiscusDemotivators = TopDiscusDemotivators;
            home.TopDateDemotivators = TopNewDemotivators;
            home.TopRateDemotivators = TopRateDemotivators;
            home.demotivators = demotivators;
            home.tags = tags;
            home.DemCount = demotivators.Count();
            return View(home);
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
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
            var CurrentUser = User.Identity.GetUserId();
            var isIt = db.rates.Where(v => v.AspNetUserId == CurrentUser && v.DemotivatorId == autoId).FirstOrDefault();
            if (isIt != null)
            {
                HttpCookie cookie = new HttpCookie(url, "true");
                Response.Cookies.Add(cookie);
                return Json("<br />You have already rated this post, thanks !");
            }
            return Json("");
        }

        public float GetRateCount (Demotivator demotivator)
        {
            int currentVotesCount = 0;
            int totalNumberOfVotes = 0;
            int totalVoteCount = 0;
            Single Average = 0;
                if (demotivator.Rate.Length>0)
                {
                    string[] votes = demotivator.Rate.Split(',');
                    for (int i = 0; i < votes.Length; i++)
                    {
                        currentVotesCount = int.Parse(votes[i]);
                        totalNumberOfVotes = totalNumberOfVotes + currentVotesCount;
                        totalVoteCount = totalVoteCount + (currentVotesCount * (i + 1));
                    }
                    Average = (float)totalVoteCount / (float)totalNumberOfVotes;
                }
            return (Average);
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
            var CurrentUser = User.Identity.GetUserId();
            var isIt = db.rates.Where(v => v.AspNetUserId == CurrentUser && v.DemotivatorId == autoId).FirstOrDefault();
                    if (isIt != null)
                    {
                        HttpCookie cookie = new HttpCookie(url, "true");
                        Response.Cookies.Add(cookie);
                        return Json("<br />You have already rated this post, thanks !");
                    }
                    var demotivator = db.Demotivators.Where(sc => sc.Id == autoId).FirstOrDefault();
                    if (demotivator != null)
                    {
                        object obj = demotivator.Rate;
                        string updatedVotes = string.Empty;
                        string[] votes = null;
                        if (obj != null && obj.ToString().Length > 0)
                        {
                            string currentVotes = obj.ToString(); 
                            votes = currentVotes.Split(',');
                            if (votes.Length.Equals(5))
                            {
                                int currentNumberOfVote = int.Parse(votes[thisVote - 1]);
                                currentNumberOfVote++;
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
                        foreach (string ss in votes)
                        {
                            updatedVotes += ss + ",";
                        }
                        updatedVotes = updatedVotes.Substring(0, updatedVotes.Length - 1);
                        db.Entry(demotivator).State = EntityState.Modified;
                        demotivator.Rate = updatedVotes;
                        demotivator.RateCount = GetRateCount(demotivator);
                        db.SaveChanges();
                        rate viewmodel = new rate()
                        {
                            AspNetUserId = User.Identity.GetUserId(),
                            Count = thisVote,
                            DemotivatorId = autoId,
                             IsRate = true
                        };
                        db.rates.Add(viewmodel);
                        db.SaveChanges();
                        HttpCookie cookie = new HttpCookie(url, "true");
                        Response.Cookies.Add(cookie);
            }
            return Json("<br />You rated " + r + " star(s), thanks !");
        }
    }
}