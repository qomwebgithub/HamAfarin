using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace HamAfarin.Areas.UserPanel.Controllers
{
    public class Tbl_BusinessPlanPaymentController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: UserPanel/Tbl_BusinessPlanPayment
        /// <summary>
        /// دریافت لیست پرداختی های یک طرح
        /// </summary>
        /// <param name="id">شناسه طرح تجاری</param>
        /// <returns>لیست سرمایه گذری های یک طرح</returns>
        [Route("BusinessPlan/{id}/Index")]
        public ActionResult Index(int id)
        {
            ViewBag.Title = db.Tbl_BussinessPlans.FirstOrDefault(b => id == b.BussinessPlanID).Title;
            var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Include(t => t.Tbl_BussinessPlans).Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Where(p => p.BusinessPlan_id == id);
            return View(tbl_BusinessPlanPayment.ToList());
        }

        // GET: UserPanel/Tbl_BusinessPlanPayment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            if (tbl_BusinessPlanPayment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanPayment);
        }

        // GET: UserPanel/Tbl_BusinessPlanPayment/Create
        public ActionResult Create()
        {
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle");
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: UserPanel/Tbl_BusinessPlanPayment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_BusinessPlanPayment tbl_BusinessPlanPayment)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanPayment.BusinessPlan_id);
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle", tbl_BusinessPlanPayment.PaymentType_id);
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.CreateUser_id);
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.PaymentUser_id);
            return View(tbl_BusinessPlanPayment);
        }

        // GET: UserPanel/Tbl_BusinessPlanPayment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            if (tbl_BusinessPlanPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanPayment.BusinessPlan_id);
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle", tbl_BusinessPlanPayment.PaymentType_id);
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.CreateUser_id);
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.PaymentUser_id);
            return View(tbl_BusinessPlanPayment);
        }

        // POST: UserPanel/Tbl_BusinessPlanPayment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_BusinessPlanPayment tbl_BusinessPlanPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_BusinessPlanPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanPayment.BusinessPlan_id);
            ViewBag.PaymentType_id = new SelectList(db.Tbl_PaymentType, "PaymentTypeID", "PaymentTitle", tbl_BusinessPlanPayment.PaymentType_id);
            ViewBag.CreateUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.CreateUser_id);
            ViewBag.PaymentUser_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanPayment.PaymentUser_id);
            return View(tbl_BusinessPlanPayment);
        }

        // GET: UserPanel/Tbl_BusinessPlanPayment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            if (tbl_BusinessPlanPayment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanPayment);
        }

        // POST: UserPanel/Tbl_BusinessPlanPayment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Find(id);
            db.Tbl_BusinessPlanPayment.Remove(tbl_BusinessPlanPayment);
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
