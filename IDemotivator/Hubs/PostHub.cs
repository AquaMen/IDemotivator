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

namespace IDemotivator.Hubs
{
    public class PostHub : Hub
    {
        private string imgFolder = "/images/";
        private string defaultAvatar = "user.png";

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
                               PostedDate = post.Date,
                               PostId = post.Id,
                           }).ToArray();
                Clients.All.loadPosts(ret);
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
                    PostedDate = post.Date,
                    PostId = post.Id
                };

                Clients.Caller.addPost(ret);
                Clients.Others.newPost(ret);
            }
        }

        
    }
}