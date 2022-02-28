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
    public class Tbl_InvestorController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Investor
        public ActionResult Index()
        {
            return View(db.Tbl_Investor.OrderByDescending(i => i.CreateDate).ToList());
        }

        // GET: Admin/Tbl_Investor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Investor tbl_Investor = db.Tbl_Investor.Find(id);
            if (tbl_Investor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Investor);
        }

        // GET: Admin/Tbl_Investor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_Investor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Investor tbl_Investor, HttpPostedFileBase imgUpInvestor)
        {
            if (ModelState.IsValid)
            {
                tbl_Investor.ImageName = "no-photo.jpg";
                tbl_Investor.CreateDate = DateTime.Now;
                tbl_Investor.IsDelete = false;
                if (imgUpInvestor != null && imgUpInvestor.IsImage())
                {
                    tbl_Investor.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUpInvestor.FileName);
                    imgUpInvestor.SaveAs(Server.MapPath("/Images/InvestorImages/Image/" + tbl_Investor.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/InvestorImages/Image/" + tbl_Investor.ImageName),
                        Server.MapPath("/Images/InvestorImages/Thumb/" + tbl_Investor.ImageName));

                }
                db.Tbl_Investor.Add(tbl_Investor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Investor);
        }

        // GET: Admin/Tbl_Investor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Investor tbl_Investor = db.Tbl_Investor.Find(id);
            if (tbl_Investor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Investor);
        }

        // POST: Admin/Tbl_Investor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Investor tbl_Investor, HttpPostedFileBase imgUpInvestor)
        {
            if (ModelState.IsValid)
            {

                if (imgUpInvestor != null && imgUpInvestor.IsImage())
                {
                    if (tbl_Investor.ImageName != "no-photo.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/InvestorImages/Image/" + tbl_Investor.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/InvestorImages/Thumb/" + tbl_Investor.ImageName));
                    }
                    tbl_Investor.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUpInvestor.FileName);
                    imgUpInvestor.SaveAs(Server.MapPath("/Images/InvestorImages/Image/" + tbl_Investor.ImageName));

                    ImageResizer img = new ImageResizer(500);
                    img.Resize(Server.MapPath("/Images/InvestorImages/Image/" + tbl_Investor.ImageName),
                        Server.MapPath("/Images/InvestorImages/Thumb/" + tbl_Investor.ImageName));

                }

                db.Entry(tbl_Investor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Investor);
        }

        // GET: Admin/Tbl_Investor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Investor tbl_Investor = db.Tbl_Investor.Find(id);
            if (tbl_Investor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Investor);
        }

        // POST: Admin/Tbl_Investor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Investor tbl_Investor = db.Tbl_Investor.Find(id);
            tbl_Investor.IsDelete = true;
            db.Entry(tbl_Investor).State = EntityState.Modified;
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
