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
    public class Tbl_MenuTypeController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_MenuType
        public ActionResult Index()
        {
            return View(db.Tbl_MenuType.Where(t => t.IsDelete == false).ToList());
        }

        // GET: Admin/Tbl_MenuType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuType tbl_MenuType = db.Tbl_MenuType.Find(id);
            if (tbl_MenuType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuType);
        }

        // GET: Admin/Tbl_MenuType/Create
        public ActionResult Create()
        {
            return View(new Tbl_MenuType() { IsActive = true, IsDelete = false, CreateDate = DateTime.Now });
        }

        // POST: Admin/Tbl_MenuType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_MenuType tbl_MenuType)
        {
            if (ModelState.IsValid)
            {
                tbl_MenuType.Title.Trim();
                db.Tbl_MenuType.Add(tbl_MenuType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_MenuType);
        }

        // GET: Admin/Tbl_MenuType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuType tbl_MenuType = db.Tbl_MenuType.Find(id);
            if (tbl_MenuType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuType);
        }

        // POST: Admin/Tbl_MenuType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuTypeID,IsDelete,IsActive,CreateDate,Title")] Tbl_MenuType tbl_MenuType)
        {
            if (ModelState.IsValid)
            {
                tbl_MenuType.Title.Trim();
                db.Entry(tbl_MenuType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_MenuType);
        }

        // GET: Admin/Tbl_MenuType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuType tbl_MenuType = db.Tbl_MenuType.Find(id);
            if (tbl_MenuType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuType);
        }

        // POST: Admin/Tbl_MenuType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_MenuType tbl_MenuType = db.Tbl_MenuType.Find(id);
            tbl_MenuType.IsDelete = true;
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
