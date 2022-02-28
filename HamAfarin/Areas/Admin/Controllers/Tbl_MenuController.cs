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
    public class Tbl_MenuController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Menu
        public ActionResult Index()
        {
            return View(db.Tbl_Menu.Where(p=>p.IsDelete == false)
                .OrderByDescending(c => c.CreateDate).ToList());
        }

        // GET: Admin/Tbl_Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            if (tbl_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Menu);
        }

        // GET: Admin/Tbl_Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Menu tbl_Menu)
        {
            if (ModelState.IsValid)
            {
                tbl_Menu.IsDelete = false;
                tbl_Menu.CreateDate = DateTime.Now;
                db.Tbl_Menu.Add(tbl_Menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Menu);
        }

        // GET: Admin/Tbl_Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            if (tbl_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Menu);
        }

        // POST: Admin/Tbl_Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Menu tbl_Menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Menu);
        }

        // GET: Admin/Tbl_Menu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            if (tbl_Menu == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Menu);
        }

        // POST: Admin/Tbl_Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Menu tbl_Menu = db.Tbl_Menu.Find(id);
            tbl_Menu.IsDelete = true;
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
