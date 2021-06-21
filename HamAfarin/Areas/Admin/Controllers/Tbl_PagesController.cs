using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using InsertShowImage;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_PagesController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Pages
        public ActionResult Index()
        {
            return View(db.Tbl_Pages.Where(p=>p.IsDelete == false).OrderByDescending(c => c.CreateDate).ToList());
        }

        // GET: Admin/Tbl_Pages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Pages tbl_Pages = db.Tbl_Pages.Find(id);
            if (tbl_Pages == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Pages);
        }

        // GET: Admin/Tbl_Pages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Pages tbl_Pages, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {

                    tbl_Pages.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    ///// عکس را در محل مورد نظر ذخیره میکنیم
                    Image.SaveAs(Server.MapPath("/Images/Pages/Image/" + tbl_Pages.ImageName));
                    ///عکس را ریسایز میکنیم و دخیره میکنیم
                    ImageResizer imageresizer = new ImageResizer(500);
                    imageresizer.Resize(Server.MapPath("/Images/Pages/Image/" + tbl_Pages.ImageName),
                        Server.MapPath("/Images/Pages/Thumb/" + tbl_Pages.ImageName)
                        );
                }
                if (string.IsNullOrEmpty(tbl_Pages.UrlTitle) == false)
                {
                    tbl_Pages.UrlTitle = tbl_Pages.UrlTitle.Replace(" ", "-");
                }

                tbl_Pages.IsDelete = false;
                tbl_Pages.CreateDate = DateTime.Now;
                db.Tbl_Pages.Add(tbl_Pages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Pages);
        }

        // GET: Admin/Tbl_Pages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Pages tbl_Pages = db.Tbl_Pages.Find(id);
            if (tbl_Pages == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Pages);
        }

        // POST: Admin/Tbl_Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Pages tbl_Pages, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Images/Pages/Image/" + tbl_Pages.ImageName)) &&
         System.IO.File.Exists(Server.MapPath("/Images/Pages/Thumb/" + tbl_Pages.ImageName)))
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath("/Images/Pages/Image/" + tbl_Pages.ImageName));
                            System.IO.File.Delete(Server.MapPath("/Images/Pages/Thumb/" + tbl_Pages.ImageName));
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    tbl_Pages.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    ///// عکس را در محل مورد نظر ذخیره میکنیم
                    Image.SaveAs(Server.MapPath("/Images/Pages/Image/" + tbl_Pages.ImageName));
                    ///عکس را ریسایز میکنیم و دخیره میکنیم
                    ImageResizer imageresizer = new ImageResizer(500);
                    imageresizer.Resize(Server.MapPath("/Images/Pages/Image/" + tbl_Pages.ImageName),
                        Server.MapPath("/Images/Pages/Thumb/" + tbl_Pages.ImageName)
                        );
                }

                db.Entry(tbl_Pages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Pages);
        }

        // GET: Admin/Tbl_Pages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Pages tbl_Pages = db.Tbl_Pages.Find(id);
            if (tbl_Pages == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Pages);
        }

        // POST: Admin/Tbl_Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Pages tbl_Pages = db.Tbl_Pages.Find(id);
            db.Tbl_Pages.Remove(tbl_Pages);
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
