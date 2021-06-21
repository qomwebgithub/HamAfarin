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
    public class Tbl_BusinessPlanQuestionController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_BusinessPlanQuestion
        public ActionResult Index()
        {
            var tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Include(t => t.Tbl_BusinessPlanQuestion2).Include(t => t.Tbl_BussinessPlans).Include(t => t.Tbl_Users);
            return View(tbl_BusinessPlanQuestion.ToList());
        }

        // GET: Admin/Tbl_BusinessPlanQuestion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (tbl_BusinessPlanQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanQuestion);
        }

        // GET: Admin/Tbl_BusinessPlanQuestion/Create
        public ActionResult Create()
        {
            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText");
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: Admin/Tbl_BusinessPlanQuestion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,QuestionText")] Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_BusinessPlanQuestion.Add(tbl_BusinessPlanQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText", tbl_BusinessPlanQuestion.Parent_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanQuestion.BusinessPlan_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanQuestion.User_id);
            return View(tbl_BusinessPlanQuestion);
        }

        // GET: Admin/Tbl_BusinessPlanQuestion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (tbl_BusinessPlanQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText", tbl_BusinessPlanQuestion.Parent_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanQuestion.BusinessPlan_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanQuestion.User_id);
            return View(tbl_BusinessPlanQuestion);
        }

        // POST: Admin/Tbl_BusinessPlanQuestion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,IsActive,IsDeleted,Parent_id,BusinessPlan_id,User_id,CreateDate,QuestionText")] Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_BusinessPlanQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Parent_id = new SelectList(db.Tbl_BusinessPlanQuestion, "QuestionID", "QuestionText", tbl_BusinessPlanQuestion.Parent_id);
            ViewBag.BusinessPlan_id = new SelectList(db.Tbl_BussinessPlans, "BussinessPlanID", "Title", tbl_BusinessPlanQuestion.BusinessPlan_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_BusinessPlanQuestion.User_id);
            return View(tbl_BusinessPlanQuestion);
        }

        // GET: Admin/Tbl_BusinessPlanQuestion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            if (tbl_BusinessPlanQuestion == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BusinessPlanQuestion);
        }

        // POST: Admin/Tbl_BusinessPlanQuestion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_BusinessPlanQuestion tbl_BusinessPlanQuestion = db.Tbl_BusinessPlanQuestion.Find(id);
            db.Tbl_BusinessPlanQuestion.Remove(tbl_BusinessPlanQuestion);
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
