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

namespace IDemotivator.Controllers
{
    [Authorize]
    public class DemotivatorsController : Controller
    {
        private Entities db = new Entities();

        // GET: Demotivators
        public async Task<ActionResult> Index()
        {
            Comment comment = new Comment();
            var s = comment.AspNetUser.UserName;
            comment.Text.ToString();
            string CurId = User.Identity.GetUserId();
            var demotivators = await db.Demotivators.Where(d => d.AspNetUserId == CurId).ToListAsync();
            return View(demotivators);
        }

        // GET: Demotivators/Details/5
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


        public void AddComment(string TextMessange, int IdDem)
        {
            var comment = new Comment();
            comment.Date = DateTime.Now;
            comment.AspNetUserId = User.Identity.GetUserId();
            comment.Text = TextMessange;
            comment.DemotivatorId = IdDem;
            db.Comments.Add(comment);
            db.SaveChanges();

        }




        // GET: Demotivators/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Demotivators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JSON,Id,AspNetUserId,Name,Date,Url_Img,Url_Img_Origin,Str1,Str2")] Demotivator demotivator)
        {
            if (ModelState.IsValid)
            {
                demotivator.Url_Img = "asdasdasda";
                demotivator.AspNetUserId = User.Identity.GetUserId();
                demotivator.Date = DateTime.Now;

                db.Demotivators.Add(demotivator);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", demotivator.AspNetUserId);
            return View(demotivator);
        }

        // GET: Demotivators/Edit/5
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

        // POST: Demotivators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Demotivators/Delete/5
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

        // POST: Demotivators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Demotivator demotivator = await db.Demotivators.FindAsync(id);
            db.Demotivators.Remove(demotivator);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost] 
        public JsonResult Upload()
        {
            List<ImageUploadResult> list = new List<ImageUploadResult>();
            JsonResult trem = new JsonResult();
            string fileName = "";
            ImageUploadResult uploadResult = new ImageUploadResult();
            ImageUploadResult uploadResult2 = new ImageUploadResult();
            ImageUploadParams uploadParams = new ImageUploadParams();
            ImageUploadParams uploadParams2 = new ImageUploadParams();
            Account account = new Account(
                       "aniknaemm",
                       "173434464182424",
                       "p3LleRLwWAxpm9yU3CHT63qKp_E");

            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);

            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {                   
                    fileName = System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath("~/" + fileName));
                    uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(Server.MapPath("~/" + fileName)),
                        PublicId = User.Identity.Name + fileName,
                    };        
                }
            }


            foreach (string file in Request.Form)
            {
                var upload = Request.Form[file];
                if (upload != null)
                {

                    string x = upload.Replace("data:image/png;base64,", "");
                    byte[] imageBytes = Convert.FromBase64String(x);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);


                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    image.Save(Server.MapPath("~/img.png"), System.Drawing.Imaging.ImageFormat.Png);

                    uploadParams2 = new ImageUploadParams()
                    {
                        File = new FileDescription(Server.MapPath("~/img.png")),
                        PublicId = User.Identity.Name + fileName +"demotevators"
                    };
                }
            }


            uploadResult = cloudinary.Upload(uploadParams);
            list.Add(uploadResult);
            uploadResult2 = cloudinary.Upload(uploadParams2);
            list.Add(uploadResult2);
            System.IO.File.Delete(Server.MapPath("~/" + fileName));
            System.IO.File.Delete(Server.MapPath("~/img.png"));
            return Json(list, JsonRequestBehavior.AllowGet);
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
