using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_CapitalApplicantHelpController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_CapitalApplicantHelp
        public ActionResult Index()
        {
            return View(db.Tbl_CapitalApplicantHelp.FirstOrDefault());
        }

        // GET: Admin/Tbl_CapitalApplicantHelp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CapitalApplicantHelp tbl_CapitalApplicantHelp = db.Tbl_CapitalApplicantHelp.Find(id);
            if (tbl_CapitalApplicantHelp == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CapitalApplicantHelp);
        }

        // GET: Admin/Tbl_CapitalApplicantHelp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_CapitalApplicantHelp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Tbl_CapitalApplicantHelp tbl_CapitalApplicantHelp)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_CapitalApplicantHelp.Add(tbl_CapitalApplicantHelp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_CapitalApplicantHelp);
        }

        // GET: Admin/Tbl_CapitalApplicantHelp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CapitalApplicantHelp tbl_CapitalApplicantHelp = db.Tbl_CapitalApplicantHelp.Find(id);
            if (tbl_CapitalApplicantHelp == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CapitalApplicantHelp);
        }

        // POST: Admin/Tbl_CapitalApplicantHelp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_CapitalApplicantHelp tbl_CapitalApplicantHelp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_CapitalApplicantHelp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_CapitalApplicantHelp);
        }

        // GET: Admin/Tbl_CapitalApplicantHelp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CapitalApplicantHelp tbl_CapitalApplicantHelp = db.Tbl_CapitalApplicantHelp.Find(id);
            if (tbl_CapitalApplicantHelp == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CapitalApplicantHelp);
        }

        // POST: Admin/Tbl_CapitalApplicantHelp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_CapitalApplicantHelp tbl_CapitalApplicantHelp = db.Tbl_CapitalApplicantHelp.Find(id);
            db.Tbl_CapitalApplicantHelp.Remove(tbl_CapitalApplicantHelp);
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
