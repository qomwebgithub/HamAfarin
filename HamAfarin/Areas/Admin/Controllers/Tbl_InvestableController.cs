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
    public class Tbl_InvestableController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Investable
        public ActionResult Index()
        {
            return View(db.Tbl_Investable.ToList());
        }

        // GET: Admin/Tbl_Investable/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Investable tbl_Investable = db.Tbl_Investable.Find(id);
            if (tbl_Investable == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Investable);
        }

        // GET: Admin/Tbl_Investable/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_Investable/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Investable tbl_Investable, HttpPostedFileBase imgUpInvestable)
        {
            if (ModelState.IsValid)
            {
                tbl_Investable.ImageName = "no-photo.jpg";
                tbl_Investable.CreateDate = DateTime.Now;
                tbl_Investable.IsDelete = false;
                if (imgUpInvestable != null && imgUpInvestable.IsImage())
                {
                    tbl_Investable.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUpInvestable.FileName);
                    imgUpInvestable.SaveAs(Server.MapPath("/Images/InvestableImages/Image/" + tbl_Investable.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/InvestableImages/Image/" + tbl_Investable.ImageName),
                        Server.MapPath("/Images/InvestableImages/Thumb/" + tbl_Investable.ImageName));

                }
                db.Tbl_Investable.Add(tbl_Investable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Investable);
        }

        // GET: Admin/Tbl_Investable/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Investable tbl_Investable = db.Tbl_Investable.Find(id);
            if (tbl_Investable == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Investable);
        }

        // POST: Admin/Tbl_Investable/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Investable tbl_Investable, HttpPostedFileBase imgUpInvestable)
        {
            if (ModelState.IsValid)
            {

                if (imgUpInvestable != null && imgUpInvestable.IsImage())
                {
                    if (tbl_Investable.ImageName != "no-photo.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/InvestableImages/Image/" + tbl_Investable.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/InvestableImages/Thumb/" + tbl_Investable.ImageName));
                    }
                    tbl_Investable.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUpInvestable.FileName);
                    imgUpInvestable.SaveAs(Server.MapPath("/Images/InvestableImages/Image/" + tbl_Investable.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/InvestableImages/Image/" + tbl_Investable.ImageName),
                        Server.MapPath("/Images/InvestableImages/Thumb/" + tbl_Investable.ImageName));

                }

                db.Entry(tbl_Investable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Investable);
        }

        // GET: Admin/Tbl_Investable/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Investable tbl_Investable = db.Tbl_Investable.Find(id);
            if (tbl_Investable == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Investable);
        }

        // POST: Admin/Tbl_Investable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Investable tbl_Investable = db.Tbl_Investable.Find(id);
            tbl_Investable.IsDelete = true;
            db.Entry(tbl_Investable).State = EntityState.Modified;
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
