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
    public class Tbl_DargahController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Dargah
        public ActionResult Index()
        {
            return View(db.Tbl_Dargah.ToList());
        }

        // GET: Admin/Tbl_Dargah/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Dargah tbl_Dargah = db.Tbl_Dargah.Find(id);
            if (tbl_Dargah == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Dargah);
        }

        // GET: Admin/Tbl_Dargah/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_Dargah/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IsActive,Title,Name,CreateDate,Checked")] Tbl_Dargah tbl_Dargah)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Dargah.Add(tbl_Dargah);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Dargah);
        }

        // GET: Admin/Tbl_Dargah/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Dargah tbl_Dargah = db.Tbl_Dargah.Find(id);
            if (tbl_Dargah == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Dargah);
        }

        // POST: Admin/Tbl_Dargah/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Dargah tbl_Dargah)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Dargah).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Dargah);
        }

        // GET: Admin/Tbl_Dargah/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Dargah tbl_Dargah = db.Tbl_Dargah.Find(id);
            if (tbl_Dargah == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Dargah);
        }

        // POST: Admin/Tbl_Dargah/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Dargah tbl_Dargah = db.Tbl_Dargah.Find(id);
            db.Tbl_Dargah.Remove(tbl_Dargah);
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
