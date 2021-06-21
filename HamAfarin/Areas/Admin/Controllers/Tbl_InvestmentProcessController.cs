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
using KooyWebApp_MVC.Classes;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_InvestmentProcessController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_InvestmentProcess
        public ActionResult Index()
        {
            return View(db.Tbl_InvestmentProcess.ToList());
        }

        // GET: Admin/Tbl_InvestmentProcess/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_InvestmentProcess tbl_InvestmentProcess = db.Tbl_InvestmentProcess.Find(id);
            if (tbl_InvestmentProcess == null)
            {
                return HttpNotFound();
            }
            return View(tbl_InvestmentProcess);
        }

        // GET: Admin/Tbl_InvestmentProcess/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_InvestmentProcess/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_InvestmentProcess tbl_Investment, HttpPostedFileBase imgImageName)
        {
            if (ModelState.IsValid)
            {
               
                tbl_Investment.IsDeleted = false;
                tbl_Investment.CreateDate = DateTime.Now;
                tbl_Investment.ImageName = "no-photo.jpg";
                if (imgImageName != null && imgImageName.IsImage())
                {
                    tbl_Investment.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgImageName.FileName);
                    imgImageName.SaveAs(Server.MapPath("/Images/InvestmentProcess/Image/" + tbl_Investment.ImageName));
                }

                db.Tbl_InvestmentProcess.Add(tbl_Investment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Investment);
        }

        // GET: Admin/Tbl_InvestmentProcess/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_InvestmentProcess tbl_InvestmentProcess = db.Tbl_InvestmentProcess.Find(id);
            if (tbl_InvestmentProcess == null)
            {
                return HttpNotFound();
            }
            return View(tbl_InvestmentProcess);
        }

        // POST: Admin/Tbl_InvestmentProcess/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_InvestmentProcess tbl_Investment, HttpPostedFileBase imgImageName)
        {
            if (ModelState.IsValid)
            {
                if (imgImageName != null && imgImageName.IsImage())
                {
                    if (tbl_Investment.ImageName != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/InvestmentProcess/Image/" + tbl_Investment.ImageName));
                    }
                    tbl_Investment.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgImageName.FileName);
                    imgImageName.SaveAs(Server.MapPath("/Images/InvestmentProcess/Image/" + tbl_Investment.ImageName));

                }
                db.Entry(tbl_Investment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Investment);
        }

        // GET: Admin/Tbl_InvestmentProcess/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_InvestmentProcess tbl_InvestmentProcess = db.Tbl_InvestmentProcess.Find(id);
            if (tbl_InvestmentProcess == null)
            {
                return HttpNotFound();
            }
            return View(tbl_InvestmentProcess);
        }

        // POST: Admin/Tbl_InvestmentProcess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_InvestmentProcess tbl_InvestmentProcess = db.Tbl_InvestmentProcess.Find(id);
            tbl_InvestmentProcess.IsDeleted = true;
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
