using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IDemotivator;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Text.RegularExpressions;
using IDemotivator.Filters;
using Nest;
using IDemotivator.Search;
using PagedList.Mvc;
using PagedList;

namespace IDemotivator.Controllers
{
    [Authorize]
    [Culture]
    public class DemotivatorsController : Controller
    {
        private Entities db = new Entities();

        public ActionResult AutocompleteSearch(string term)
        {
            var models = db.tags.Where(a => a.Name.Contains(term))
                            .Select(a => new { value = a.Name })
                            .Distinct();

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            string CurId = User.Identity.GetUserId();
            var demotivators = await db.Demotivators.Where(d => d.AspNetUserId == CurId).ToListAsync();
            return View(demotivators.ToPagedList(pageNumber, pageSize));
        }
        [AllowAnonymous]
        public async Task<ActionResult> ShowAll(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var demotivators = await db.Demotivators.ToListAsync();
            return View(demotivators.ToPagedList(pageNumber, pageSize));
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demotivator demotivator = await db.Demotivators.FindAsync(id);
            if (demotivator == null)
            {
                return HttpNotFound();
            }
            return View(demotivator);
        }

        public JsonResult AddComment(string TextMessange, int IdDem)
        {
            var comment = new Comment();
            comment.Date = DateTime.Now;
            comment.AspNetUserId = User.Identity.GetUserId();
            comment.Text = TextMessange;
            comment.DemotivatorId = IdDem;
            db.Comments.Add(comment);
            db.SaveChanges();
            var ret = new
            {
                UserName = User.Identity.Name,
                Date = comment.Date.ToString("s"),
                Text = comment.Text
            };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult SearchResult(int id)
        {
            var tags = db.tag_to_dem.Where(d => d.tagId == id).ToList();
            List<Demotivator> demotivators = new List<Demotivator>();
            foreach (var item in tags)
            {
                var demotivator1 = db.Demotivators.Where(s => s.Id == item.DemotivatorId).ToList();
                foreach (var item1 in demotivator1)
                {
                    demotivators.Add(item1);
                }
            }
            return View(demotivators);
        }

        public ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        public void TagSave(int DemId, int id)
        {
            tag_to_dem tag1 = new tag_to_dem();
            tag1.DemotivatorId = DemId;
            tag1.tagId = id;
            db.tag_to_dem.Add(tag1);
            db.SaveChanges();
        }

        public int AddLike(int CommentId)
        {
            string UserId = User.Identity.GetUserId();
            var CheckLike = db.Likes.FirstOrDefault(d => d.CommentId == CommentId && d.AspNetUserId == UserId);
            if (CheckLike == null)
            {
                Like like = new Like();
                like.CommentId = CommentId;
                like.AspNetUserId = UserId;
                db.Likes.Add(like);
            }
            else
            {
                db.Likes.Remove(CheckLike);
            }
            db.SaveChanges();
            int count = db.Comments.Find(CommentId).Likes.Count();
            return (count);
        }

        public void AddTag(string Tag, int DemId)
        {
            Regex regular = new Regex(@"\w+");
            MatchCollection tagi = regular.Matches(Tag);
            var tags = db.tags.ToList();
            bool flag = false;
            int ik = 0;
            foreach (var tagses in tagi)
            {
                ik++;
                if (ik > 5) break;
                flag = false;
                tags = db.tags.ToList();
                foreach (var item in tags)
                {
                    if (tagses.ToString() == item.Name)
                    {
                        TagSave(DemId, item.Id);
                        flag = true;
                    }
                }
                if (!flag)
                {
                    tag tag2 = new tag();
                    tag2.Name = tagses.ToString();
                    db.tags.Add(tag2);
                    db.SaveChanges();
                    TagSave(DemId, tag2.Id);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JSON,Id,AspNetUserId,Name,Date,Url_Img,Url_Img_Origin,Str1,Str2")] Demotivator demotivator, string newtag)
        {
            if (ModelState.IsValid)
            {
                demotivator.AspNetUserId = User.Identity.GetUserId();
                demotivator.Date = DateTime.Now;
                db.Demotivators.Add(demotivator);
                db.SaveChanges();
                using (var elastic = new elasticsearchNEST())
                {
                    elastic.Adding(demotivator);
                }
                AddTag(newtag, demotivator.Id);
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", demotivator.AspNetUserId);
            return View(demotivator);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demotivator demotivator = await db.Demotivators.FindAsync(id);
            if (demotivator == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", demotivator.AspNetUserId);
            return View(demotivator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JSON,Id,AspNetUserId,Name,Date,Url_Img,Url_Img_Origin,Str1,Str2")] Demotivator demotivator)
        {
            if (ModelState.IsValid)
            {
                demotivator.AspNetUserId = User.Identity.GetUserId();
                demotivator.Date = DateTime.Now;
                db.Entry(demotivator).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", demotivator.AspNetUserId);
            return View(demotivator);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demotivator demotivator = await db.Demotivators.FindAsync(id);
            if (demotivator == null)
            {
                return HttpNotFound();
            }
            return View(demotivator);
        }

        public void DeleteRates(int id)
        {
            var rates = db.rates.Where(t => t.DemotivatorId == id).ToList();
            foreach (var item in rates)
            {
                db.rates.Remove(item);
            }
        }

        public void DeleteLikes(int id)
        {
            var likes = db.Likes.Where(t => t.CommentId == id).ToList();
            foreach(var item in likes)
            {
                db.Likes.Remove(item);
            }
        }

        public void DeleteComments(int id)
        {
            var comments = db.Comments.Where(t => t.DemotivatorId == id).ToList();
            foreach (var item in comments)
            {
                DeleteLikes(item.Id);
                db.Comments.Remove(item);
            }
        }

        public void CheckTag(int id)
        {
            tag tag1 = new tag();
            tag1 = db.tags.Find(id);
            var count = db.tag_to_dem.Where(ds => ds.tagId == tag1.Id).Count();
            if (count == 0)
                db.tags.Remove(tag1);
        }

        public void DeleteTags(int id)
        {
            var tags = db.tag_to_dem.Where(t => t.DemotivatorId == id).ToList();
            foreach (var item in tags)
            {
                db.tag_to_dem.Remove(item);
                CheckTag(item.tagId);
            }
        }

        public void DeleteAdds(int id)
        {
            DeleteComments(id);
            DeleteRates(id);
            DeleteTags(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Demotivator demotivator = await db.Demotivators.FindAsync(id);
            if(User.Identity.GetUserId() != demotivator.AspNetUserId)
            {
                return RedirectToAction("Index", "Home");
            }
            DeleteAdds(id);
            db.Demotivators.Remove(demotivator);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Upload()
        {
            Account account = new Account(
                       "aniknaemm",
                       "173434464182424",
                       "p3LleRLwWAxpm9yU3CHT63qKp_E");
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);
            List<string> list = new List<string>();
            foreach (string file in Request.Files)
            {
                if (Request.Files[file] != null)
                    list.Add(UploadImage(Request.Files[file], cloudinary));
            }
            foreach (string file in Request.Form)
            {
                if (Request.Form[file] != null)
                    list.Add(UploadImageFabricJS(Request.Form[file], cloudinary));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private String UploadImage(HttpPostedFileBase  imageBase, CloudinaryDotNet.Cloudinary cloudinary)
        {
                var fileName = System.IO.Path.GetFileName(imageBase.FileName);
                imageBase.SaveAs(Server.MapPath("~/" + fileName));
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Server.MapPath("~/" + fileName)),
                    PublicId = User.Identity.Name + DateTime.Now,
                };
                ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
                System.IO.File.Delete(Server.MapPath("~/" + fileName));
                return uploadResult.Uri.ToString();
        }

        private String UploadImageFabricJS(String imageFile, CloudinaryDotNet.Cloudinary cloudinary)
        {
            string x = imageFile.Replace("data:image/png;base64,", "");
            byte[] imageBytes = Convert.FromBase64String(x);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            image.Save(Server.MapPath("~/image.png"), System.Drawing.Imaging.ImageFormat.Png);
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Server.MapPath("~/image.png")),
                PublicId = User.Identity.Name + DateTime.Now + "demotevators"
            };
            ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            System.IO.File.Delete(Server.MapPath("~/image.png"));
            return uploadResult.Uri.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
