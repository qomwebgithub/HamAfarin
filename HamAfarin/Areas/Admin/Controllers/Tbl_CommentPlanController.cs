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
    public class Tbl_CommentPlanController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_CommentPlan
        public ActionResult Index()
        {
            var tbl_CommentPlan = db.Tbl_CommentPlan.Include(t => t.Tbl_BussinessPlans)
                .Include(t => t.Tbl_CommentPlan2).Include(t => t.Tbl_Users)
                .OrderByDescending(t => t.CreateDate);
            return View(tbl_CommentPlan.ToList());
        }

        // GET: Admin/Tbl_CommentPlan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            if (tbl_CommentPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CommentPlan);
        }

        // GET: Admin/Tbl_CommentPlan/Create
        public ActionResult Create()
        {
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: Admin/Tbl_CommentPlan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,CommentText")] Tbl_CommentPlan tbl_CommentPlan)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_CommentPlan.Add(tbl_CommentPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_CommentPlan.BusinessPlan_id);
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText", tbl_CommentPlan.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_CommentPlan.User_id);
            return View(tbl_CommentPlan);
        }

        // GET: Admin/Tbl_CommentPlan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            if (tbl_CommentPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_CommentPlan.BusinessPlan_id);
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText", tbl_CommentPlan.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_CommentPlan.User_id);
            return View(tbl_CommentPlan);
        }

        // POST: Admin/Tbl_CommentPlan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,CommentText")] Tbl_CommentPlan tbl_CommentPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_CommentPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_CommentPlan.BusinessPlan_id);
            ViewBag.Parent_id = new SelectList(db.Tbl_CommentPlan, "CommentID", "CommentText", tbl_CommentPlan.Parent_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_CommentPlan.User_id);
            return View(tbl_CommentPlan);
        }

        // GET: Admin/Tbl_CommentPlan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            if (tbl_CommentPlan == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CommentPlan);
        }

        // POST: Admin/Tbl_CommentPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_CommentPlan tbl_CommentPlan = db.Tbl_CommentPlan.Find(id);
            db.Tbl_CommentPlan.Remove(tbl_CommentPlan);
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
