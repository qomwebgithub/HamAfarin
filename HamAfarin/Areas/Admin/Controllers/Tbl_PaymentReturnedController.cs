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
    public class Tbl_PaymentReturnedController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_PaymentReturned
        public ActionResult Index()
        {
            var tbl_PaymentReturned = db.Tbl_PaymentReturned.Include(t => t.Tbl_BusinessPlanPayment)
                .Include(t => t.Tbl_BussinessPlans).Include(t => t.Tbl_Users)
                .OrderByDescending(b => b.CreateDate);
            return View(tbl_PaymentReturned.ToList());
        }

        // GET: Admin/Tbl_PaymentReturned/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_PaymentReturned tbl_PaymentReturned = db.Tbl_PaymentReturned.Find(id);
            if (tbl_PaymentReturned == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PaymentReturned);
        }

        // GET: Admin/Tbl_PaymentReturned/Create
        public ActionResult Create()
        {
            ViewBag.Payment_id = new SelectList(db.Tbl_BusinessPlanPayment, "PaymentID", "TransactionPaymentCode");
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: Admin/Tbl_PaymentReturned/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReturnedID,IsDelete,IsActive,CreateDate,IsConfirm,ReasonText,Payment_id,BusinessPlan_id,User_id,ConfirmDate")] Tbl_PaymentReturned tbl_PaymentReturned)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_PaymentReturned.Add(tbl_PaymentReturned);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Payment_id = new SelectList(db.Tbl_BusinessPlanPayment, "PaymentID", "TransactionPaymentCode", tbl_PaymentReturned.Payment_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_PaymentReturned.BusinessPlan_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_PaymentReturned.User_id);
            return View(tbl_PaymentReturned);
        }

        // GET: Admin/Tbl_PaymentReturned/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_PaymentReturned tbl_PaymentReturned = db.Tbl_PaymentReturned.Find(id);
            if (tbl_PaymentReturned == null)
            {
                return HttpNotFound();
            }
            ViewBag.Payment_id = new SelectList(db.Tbl_BusinessPlanPayment, "PaymentID", "TransactionPaymentCode", tbl_PaymentReturned.Payment_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_PaymentReturned.BusinessPlan_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_PaymentReturned.User_id);
            return View(tbl_PaymentReturned);
        }

        // POST: Admin/Tbl_PaymentReturned/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReturnedID,IsDelete,IsActive,CreateDate,IsConfirm,ReasonText,Payment_id,BusinessPlan_id,User_id,ConfirmDate")] Tbl_PaymentReturned tbl_PaymentReturned)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PaymentReturned).State = EntityState.Modified;
                tbl_PaymentReturned.ConfirmDate = DateTime.Now;
                tbl_PaymentReturned.IsConfirm = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Payment_id = new SelectList(db.Tbl_BusinessPlanPayment, "PaymentID", "TransactionPaymentCode", tbl_PaymentReturned.Payment_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_PaymentReturned.BusinessPlan_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_PaymentReturned.User_id);
            return View(tbl_PaymentReturned);
        }

        // GET: Admin/Tbl_PaymentReturned/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_PaymentReturned tbl_PaymentReturned = db.Tbl_PaymentReturned.Find(id);
            if (tbl_PaymentReturned == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PaymentReturned);
        }

        // POST: Admin/Tbl_PaymentReturned/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_PaymentReturned tbl_PaymentReturned = db.Tbl_PaymentReturned.Find(id);
            tbl_PaymentReturned.IsDelete = true;
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
