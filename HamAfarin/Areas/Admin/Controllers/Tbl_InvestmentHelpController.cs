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
    public class Tbl_InvestmentHelpController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();


        // GET: Admin/Tbl_InvestmentHelp/Details/5
        public ActionResult Index()
        {
            return View(db.Tbl_InvestmentHelp.OrderByDescending(i => i.CreateDate).FirstOrDefault());
        }


        // GET: Admin/Tbl_InvestmentHelp/Edit/5
        public ActionResult Edit()
        {
            return View(db.Tbl_InvestmentHelp.FirstOrDefault());
        }

        // POST: Admin/Tbl_InvestmentHelp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_InvestmentHelp tbl_InvestmentHelp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_InvestmentHelp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_InvestmentHelp);
        }

        // GET: Admin/Tbl_InvestmentHelp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_InvestmentHelp tbl_InvestmentHelp = db.Tbl_InvestmentHelp.Find(id);
            if (tbl_InvestmentHelp == null)
            {
                return HttpNotFound();
            }
            return View(tbl_InvestmentHelp);
        }

        // POST: Admin/Tbl_InvestmentHelp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_InvestmentHelp tbl_InvestmentHelp = db.Tbl_InvestmentHelp.Find(id);
            db.Tbl_InvestmentHelp.Remove(tbl_InvestmentHelp);
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
