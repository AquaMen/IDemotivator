﻿using System;
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

namespace IDemotivator.Controllers
{
    [Authorize]
    public class DemotivatorsController : Controller
    {
        private Entities db = new Entities();

        // GET: Demotivators
        public async Task<ActionResult> Index()
        {
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
        public ActionResult Create([Bind(Include = "Id,AspNetUserId,Name,Date,Url_Img,Url_Img_Origin,Str1,Str2")] Demotivator demotivator)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,AspNetUserId,Name,Date,Url_Img,Url_Img_Origin,Str1,Str2")] Demotivator demotivator)
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
            JsonResult trem = new JsonResult();
            ImageUploadResult uploadResult = new ImageUploadResult();
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    Account account = new Account(
                        "aniknaemm",
                        "173434464182424",
                        "p3LleRLwWAxpm9yU3CHT63qKp_E");

                    CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/" + fileName));

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(@"D:\Itransition\GitInt\IDemotivator\IDemotivator\" + fileName),
                        PublicId = User.Identity.Name + fileName,
                        Tags = "special, for_homepage"
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                    System.IO.File.Delete(@"D:\Itransition\GitInt\IDemotivator\IDemotivator\" + fileName);
                }

            }

            return Json(uploadResult, JsonRequestBehavior.AllowGet);
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
