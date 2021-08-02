using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using InsertShowImage;
using KooyWebApp_MVC.Classes;

namespace Hamafarin.Areas.Admin.Controllers
{
    public class Tbl_BlogController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Blog
        public ActionResult Index()
        {
            var tbl_Blog = db.Tbl_Blog.Where(b => b.IsDeleted == false).Include(t => t.Tbl_Users);
            return View(tbl_Blog.OrderByDescending(c => c.CreateDate).ToList());
        }

        // GET: Admin/Tbl_Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            if (tbl_Blog == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Blog);
        }

        // GET: Admin/Tbl_Blog/Create
        public ActionResult Create()
        {
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View(new Tbl_Blog()
            {
                IsActive = true
            });
        }

        // POST: Admin/Tbl_Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Blog tbl_Blog, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                tbl_Blog.ImageName = "no-photo.jpg";
                tbl_Blog.CountVisit = 0;
                tbl_Blog.IsDeleted = false;
                tbl_Blog.CreateDate = DateTime.Now;
                tbl_Blog.CreateUser_id = 1;
                if (imgUp != null && imgUp.IsImage())
                {
                    tbl_Blog.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/BlogImages/Image/" + tbl_Blog.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BlogImages/Image/" + tbl_Blog.ImageName),
                        Server.MapPath("/Images/BlogImages/Thumb/" + tbl_Blog.ImageName));

                }
                if (tbl_Blog.TitleUrl == null)
                {
                    tbl_Blog.TitleUrl = tbl_Blog.Title.Trim().Replace(" ", "-");
                }
                else
                {
                    tbl_Blog.TitleUrl = tbl_Blog.TitleUrl.Trim().Replace(" ", "-");
                }
                db.Tbl_Blog.Add(tbl_Blog);
                db.SaveChanges();
                return RedirectToAction("Details/" + tbl_Blog.BlogID);
            }

            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_Blog.CreateUser_id);
            return View(tbl_Blog);
        }

        // GET: Admin/Tbl_Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            if (tbl_Blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_Blog.CreateUser_id);
            return View(tbl_Blog);
        }

        // POST: Admin/Tbl_Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Blog tbl_Blog, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {

                if (imgUp != null && imgUp.IsImage())
                {
                    if (tbl_Blog.ImageName != "no-photo.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/BlogImages/Image/" + tbl_Blog.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/BlogImages/Thumb/" + tbl_Blog.ImageName));
                    }
                    tbl_Blog.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/BlogImages/Image/" + tbl_Blog.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/BlogImages/Image/" + tbl_Blog.ImageName),
                        Server.MapPath("/Images/BlogImages/Thumb/" + tbl_Blog.ImageName));

                }
                if (tbl_Blog.TitleUrl == null)
                {
                    tbl_Blog.TitleUrl = tbl_Blog.Title.Trim().Replace(" ", "-");
                }
                db.Entry(tbl_Blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details/" + tbl_Blog.BlogID);
            }
            return View(tbl_Blog);
        }

        // GET: Admin/Tbl_Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            if (tbl_Blog == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Blog);
        }

        // POST: Admin/Tbl_Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Blog tbl_Blog = db.Tbl_Blog.Find(id);
            tbl_Blog.IsDeleted = true;
            db.Entry(tbl_Blog).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
