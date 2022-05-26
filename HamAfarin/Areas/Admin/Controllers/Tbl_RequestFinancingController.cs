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
    public class Tbl_RequestFinancingController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_RequestFinancing
        public ActionResult Index()
        {
            var tbl_RequestFinancing = db.Tbl_RequestFinancing.Include(t => t.Tbl_RequestFinancingStatus).Include(t => t.Tbl_Users);
            return View(tbl_RequestFinancing.ToList());
        }

        public bool ChangeStatusRequestFinancing(int status, int Id,string DescriptionAdmin, string DescriptionAdminHidden)
        {
            try
            {
                Tbl_RequestFinancing qRequestFinancing = db.Tbl_RequestFinancing.FirstOrDefault(f => f.ID == Id);
                if (qRequestFinancing != null)
                {
                    qRequestFinancing.Status_id = status;
                    qRequestFinancing.DescriptionAdmin = DescriptionAdmin;
                    qRequestFinancing.DescriptionAdminHidden = DescriptionAdminHidden;
                    qRequestFinancing.CheckDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
 
                return false;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        // GET: Admin/Tbl_RequestFinancing/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_RequestFinancing tbl_RequestFinancing = db.Tbl_RequestFinancing.Find(id);
            if (tbl_RequestFinancing == null)
            {
                return HttpNotFound();
            }
            return View(tbl_RequestFinancing);
        }

        // GET: Admin/Tbl_RequestFinancing/Create
        public ActionResult Create()
        {
            ViewBag.Status_id = new SelectList(db.Tbl_RequestFinancingStatus, "ID", "Title");
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: Admin/Tbl_RequestFinancing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IsDelete,User_id,Status_id,Title,Description,DocumentFile,CreateDate,CheckDate,DescriptionAdmin,DescriptionAdminHidden")] Tbl_RequestFinancing tbl_RequestFinancing)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_RequestFinancing.Add(tbl_RequestFinancing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status_id = new SelectList(db.Tbl_RequestFinancingStatus, "ID", "Title", tbl_RequestFinancing.Status_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_RequestFinancing.User_id);
            return View(tbl_RequestFinancing);
        }

        // GET: Admin/Tbl_RequestFinancing/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_RequestFinancing tbl_RequestFinancing = db.Tbl_RequestFinancing.Find(id);
            if (tbl_RequestFinancing == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status_id = new SelectList(db.Tbl_RequestFinancingStatus, "ID", "Title", tbl_RequestFinancing.Status_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_RequestFinancing.User_id);
            return View(tbl_RequestFinancing);
        }

        // POST: Admin/Tbl_RequestFinancing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IsDelete,User_id,Status_id,Title,Description,DocumentFile,CreateDate,CheckDate,DescriptionAdmin,DescriptionAdminHidden")] Tbl_RequestFinancing tbl_RequestFinancing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_RequestFinancing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status_id = new SelectList(db.Tbl_RequestFinancingStatus, "ID", "Title", tbl_RequestFinancing.Status_id);
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_RequestFinancing.User_id);
            return View(tbl_RequestFinancing);
        }

        // GET: Admin/Tbl_RequestFinancing/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_RequestFinancing tbl_RequestFinancing = db.Tbl_RequestFinancing.Find(id);
            if (tbl_RequestFinancing == null)
            {
                return HttpNotFound();
            }
            return View(tbl_RequestFinancing);
        }

        // POST: Admin/Tbl_RequestFinancing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_RequestFinancing tbl_RequestFinancing = db.Tbl_RequestFinancing.Find(id);
            db.Tbl_RequestFinancing.Remove(tbl_RequestFinancing);
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
