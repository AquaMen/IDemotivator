using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDemotivator.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using IDemotivator;
using System.IO;
using IDemotivator.Filters;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace IDemotivator.Hubs
{
    public class PostHub : Hub
    {

        private string imgFolder = "/images/";
        private string defaultAvatar = "user.png";
        public  string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }


        public void GetPosts(int DemId)
        {
            using (Entities db = new Entities())
            {
                var ret = (from post in db.Comments.Where(d => d.DemotivatorId == DemId).ToList()
                           orderby post.Date
                           select new
                           {
                               Message = post.Text,
                               PostedBy = post.AspNetUserId,
                               PostedByName = post.AspNetUser.UserName,
                               PostedByAvatar = imgFolder +  defaultAvatar,
                               PostedDate = TimeAgo(post.Date),
                               PostId = post.Id,
                               LikeCount = post.Likes.Count()
                           }).ToArray();
                Clients.All.loadPosts(ret, DemId);
            }
        }

        public void AddPost(Comment post, string UserId, int DemId1)
        {

            post.AspNetUserId = UserId;
            post.Date = DateTime.Now;
            post.DemotivatorId = DemId1;
            using (Entities db = new Entities())
            {
                db.Comments.Add(post);
                db.SaveChanges();
                var usr = db.AspNetUsers.FirstOrDefault(x => x.Id == post.AspNetUserId);
                var ret = new
                {
                    Message = post.Text,
                    PostedBy = post.AspNetUserId,
                    PostedByName = usr.UserName,
                    PostedByAvatar = imgFolder +  defaultAvatar,
                    PostedDate = TimeAgo(post.Date),
                    PostId = post.Id,
                    LikeCount = post.Likes.Count()
                };

                Clients.Caller.addPost(ret, DemId1);
                Clients.Others.addPost(ret, DemId1);
            }
        }

        
    }
}