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
    public class Tbl_MenuPageController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_MenuPage
        public ActionResult Index(int id)
        {
            ViewBag.MenuTypeTitle = db.Tbl_MenuType.Find(id).Title;
            ViewBag.MenuTypeID = id;
            var tbl_MenuPage = db.Tbl_MenuPage.Where(m => m.MenuType_id == id && m.IsDelete == false).Include(t => t.Tbl_MenuType);
            return View(tbl_MenuPage.ToList());
        }

        // GET: Admin/Tbl_MenuPage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuPage tbl_MenuPage = db.Tbl_MenuPage.Find(id);
            if (tbl_MenuPage == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuPage);
        }

        // GET: Admin/Tbl_MenuPage/Create
        /// <summary>
        /// ایجاد منوی صفحه
        /// </summary>
        /// <param name="id">شناسه نوع منو</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            ViewBag.Page_id = new SelectList(db.Tbl_Pages, "PageID", "UrlTitle");
            return View(new Tbl_MenuPage() { IsActive = true, MenuType_id = id });
        }

        // POST: Admin/Tbl_MenuPage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_MenuPage tbl_MenuPage)
        {
            if (ModelState.IsValid)
            {
                tbl_MenuPage.CreateDate = DateTime.Now;
                tbl_MenuPage.IsDelete = false;
                tbl_MenuPage.Title.Trim();
                db.Tbl_MenuPage.Add(tbl_MenuPage);
                db.SaveChanges();
                return RedirectToAction("Index",new { id=tbl_MenuPage.MenuType_id});
            }

            ViewBag.Page_id = new SelectList(db.Tbl_Pages, "PageID", "UrlTitle");
            return View(tbl_MenuPage);
        }

        // GET: Admin/Tbl_MenuPage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuPage tbl_MenuPage = db.Tbl_MenuPage.Find(id);
            if (tbl_MenuPage == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuType_id = new SelectList(db.Tbl_MenuType, "MenuTypeID", "Title", tbl_MenuPage.MenuType_id);
            ViewBag.MenuPageID = new SelectList(db.Tbl_Pages, "PageID", "ImageName", tbl_MenuPage.MenuPageID);
            return View(tbl_MenuPage);
        }

        // POST: Admin/Tbl_MenuPage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_MenuPage tbl_MenuPage)
        {
            if (ModelState.IsValid)
            {
                tbl_MenuPage.Title.Trim();
                db.Entry(tbl_MenuPage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = tbl_MenuPage.MenuType_id });
            }
            ViewBag.MenuType_id = new SelectList(db.Tbl_MenuType, "MenuTypeID", "Title", tbl_MenuPage.MenuType_id);
            ViewBag.MenuPageID = new SelectList(db.Tbl_Pages, "PageID", "ImageName", tbl_MenuPage.MenuPageID);
            return View(tbl_MenuPage);
        }

        // GET: Admin/Tbl_MenuPage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_MenuPage tbl_MenuPage = db.Tbl_MenuPage.Find(id);
            if (tbl_MenuPage == null)
            {
                return HttpNotFound();
            }
            return View(tbl_MenuPage);
        }

        // POST: Admin/Tbl_MenuPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_MenuPage tbl_MenuPage = db.Tbl_MenuPage.Find(id);
            tbl_MenuPage.IsDelete = true;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = tbl_MenuPage.MenuType_id });
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
