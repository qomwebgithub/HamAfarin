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

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_FutureBusinessPlanController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_FutureBusinessPlan
        public ActionResult Index()
        {
            return View(db.Tbl_FutureBusinessPlan.ToList());
        }

        // GET: Admin/Tbl_FutureBusinessPlan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_FutureBusinessPlan tbl_FutureBusinessPlan = db.Tbl_FutureBusinessPlan.Find(id);
            if (tbl_FutureBusinessPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_FutureBusinessPlan);
        }

        // GET: Admin/Tbl_FutureBusinessPlan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_FutureBusinessPlan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_FutureBusinessPlan tbl_FutureBusinessPlan, HttpPostedFileBase imgFutureUp)
        {
            if (ModelState.IsValid)
            {
                tbl_FutureBusinessPlan.ImageName = "no-photo.jpg";
                tbl_FutureBusinessPlan.CreateDate = DateTime.Now;
                if (imgFutureUp != null && imgFutureUp.IsImage())
                {
                    tbl_FutureBusinessPlan.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgFutureUp.FileName);
                    imgFutureUp.SaveAs(Server.MapPath("/Images/FuturePlans/Image/" + tbl_FutureBusinessPlan.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/FuturePlans/Image/" + tbl_FutureBusinessPlan.ImageName),
                        Server.MapPath("/Images/FuturePlans/Thumb/" + tbl_FutureBusinessPlan.ImageName));

                }
                db.Tbl_FutureBusinessPlan.Add(tbl_FutureBusinessPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_FutureBusinessPlan);
        }

        // GET: Admin/Tbl_FutureBusinessPlan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_FutureBusinessPlan tbl_FutureBusinessPlan = db.Tbl_FutureBusinessPlan.Find(id);
            if (tbl_FutureBusinessPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_FutureBusinessPlan);
        }

        // POST: Admin/Tbl_FutureBusinessPlan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_FutureBusinessPlan tbl_FutureBusinessPlan, HttpPostedFileBase imgFutureUp)
        {
            if (ModelState.IsValid)
            {

                if (imgFutureUp != null && imgFutureUp.IsImage())
                {
                    if (tbl_FutureBusinessPlan.ImageName != "no-photo.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/FuturePlans/Image/" + tbl_FutureBusinessPlan.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/FuturePlans/Thumb/" + tbl_FutureBusinessPlan.ImageName));
                    }
                    tbl_FutureBusinessPlan.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgFutureUp.FileName);
                    imgFutureUp.SaveAs(Server.MapPath("/Images/FuturePlans/Image/" + tbl_FutureBusinessPlan.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/FuturePlans/Image/" + tbl_FutureBusinessPlan.ImageName),
                        Server.MapPath("/Images/FuturePlans/Thumb/" + tbl_FutureBusinessPlan.ImageName));

                }

                db.Entry(tbl_FutureBusinessPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_FutureBusinessPlan);
        }

        // GET: Admin/Tbl_FutureBusinessPlan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_FutureBusinessPlan tbl_FutureBusinessPlan = db.Tbl_FutureBusinessPlan.Find(id);
            if (tbl_FutureBusinessPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_FutureBusinessPlan);
        }

        // POST: Admin/Tbl_FutureBusinessPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_FutureBusinessPlan tbl_FutureBusinessPlan = db.Tbl_FutureBusinessPlan.Find(id);
            tbl_FutureBusinessPlan.IsDeleted = true;
            db.Entry(tbl_FutureBusinessPlan).State = EntityState.Modified;
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
