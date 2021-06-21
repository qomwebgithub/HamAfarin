using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_GalleriesController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Galleries
        public ActionResult Index()
        {
            return View(db.Tbl_Galleries.OrderByDescending(c => c.CreateDate).ToList());
        }

        // GET: Admin/Tbl_Galleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Galleries tbl_Galleries = db.Tbl_Galleries.Find(id);
            if (tbl_Galleries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Galleries);
        }

        // GET: Admin/Tbl_Galleries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Galleries tbl_Galleries, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    ViewBag.ImageNull = true;
                    ModelState.AddModelError("ImageName", "تصویر اجباری میباشد");
                    return View(tbl_Galleries);
                }

                tbl_Galleries.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                ///// عکس را در محل مورد نظر ذخیره میکنیم
                Image.SaveAs(Server.MapPath("/Images/Galleries/image/" + tbl_Galleries.ImageName));
                ///عکس را ریسایز میکنیم و دخیره میکنیم
                if (Image.IsImage())
                {
                    ImageResizer imageresizer = new ImageResizer(500);
                    imageresizer.Resize(Server.MapPath("/Images/Galleries/image/" + tbl_Galleries.ImageName),
                        Server.MapPath("/Images/Galleries/thumb/" + tbl_Galleries.ImageName)
                        );
                }

                tbl_Galleries.ImageUrl = ConfigurationManager.AppSettings["ThisDomain"] + "Images/Galleries/Image/" + tbl_Galleries.ImageName;
                tbl_Galleries.CreateDate = DateTime.Now;
                db.Tbl_Galleries.Add(tbl_Galleries);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Galleries);
        }

        // GET: Admin/Tbl_Galleries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Galleries tbl_Galleries = db.Tbl_Galleries.Find(id);
            if (tbl_Galleries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Galleries);
        }

        // POST: Admin/Tbl_Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Galleries tbl_Galleries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Galleries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Galleries);
        }

        // GET: Admin/Tbl_Galleries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Galleries tbl_Galleries = db.Tbl_Galleries.Find(id);
            if (tbl_Galleries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Galleries);
        }

        // POST: Admin/Tbl_Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Galleries tbl_Galleries = db.Tbl_Galleries.Find(id);
            /// اگر تصویر داشته باشد آن را پاک میکنیم
            if (tbl_Galleries.ImageName != null)
            {
                if (System.IO.File.Exists(Server.MapPath("/Images/Galleries/Image/" + tbl_Galleries.ImageName)) &&
                         System.IO.File.Exists(Server.MapPath("/Images/Galleries/Thumb/" + tbl_Galleries.ImageName)))
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/Galleries/Image/" + tbl_Galleries.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/Galleries/Thumb/" + tbl_Galleries.ImageName));
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            db.Tbl_Galleries.Remove(tbl_Galleries);
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
