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
using KooyWebApp_MVC.Classes;
using ViewModels;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_ShareHoldersCompanyController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_ShareHoldersCompany
        public ActionResult Index()
        {
            return View(db.Tbl_ShareHoldersCompany.Where(s => s.IsDelete == false)
                .OrderByDescending(s => s.CreateDate).ToList());
        }

        // GET: Admin/Tbl_ShareHoldersCompany/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ShareHoldersCompany tbl_ShareHoldersCompany = db.Tbl_ShareHoldersCompany.Find(id);
            if (tbl_ShareHoldersCompany == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ShareHoldersCompany);
        }

        // GET: Admin/Tbl_ShareHoldersCompany/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tbl_ShareHoldersCompany/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddEditShareHolderViewModel addEditShare, HttpPostedFileBase imgLogoIcon, HttpPostedFileBase imgImageName)
        {
            if (ModelState.IsValid)
            {
                Tbl_ShareHoldersCompany tbl_ShareHolders = new Tbl_ShareHoldersCompany()
                {
                    ShareHolderTitle = addEditShare.ShareHolderTitle,
                    Description = addEditShare.Description,
                    FullText = addEditShare.FullText,
                    CreateDate = DateTime.Now,
                    IsDelete = false,
                    IsActive = true
                };
                tbl_ShareHolders.LogoIcon = "no-photo.jpg";
                if (imgLogoIcon != null && imgLogoIcon.IsImage())
                {
                    tbl_ShareHolders.LogoIcon = Guid.NewGuid().ToString() + Path.GetExtension(imgLogoIcon.FileName);
                    imgLogoIcon.SaveAs(Server.MapPath("/Images/ShareHolders/Logo/" + tbl_ShareHolders.LogoIcon));
                }
                if (imgImageName != null && imgImageName.IsImage())
                {
                    tbl_ShareHolders.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgLogoIcon.FileName);
                    imgImageName.SaveAs(Server.MapPath("/Images/ShareHolders/Image/" + tbl_ShareHolders.ImageName));
                }

                db.Tbl_ShareHoldersCompany.Add(tbl_ShareHolders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(addEditShare);
        }

        // GET: Admin/Tbl_ShareHoldersCompany/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ShareHoldersCompany tbl_ShareHoldersCompany = db.Tbl_ShareHoldersCompany.Find(id);
            if (tbl_ShareHoldersCompany == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ShareHoldersCompany);
        }

        // POST: Admin/Tbl_ShareHoldersCompany/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_ShareHoldersCompany tbl_ShareHoldersCompany, HttpPostedFileBase imgLogoIcon, HttpPostedFileBase imgImageName)
        {
            if (ModelState.IsValid)
            {
                if (imgLogoIcon != null && imgLogoIcon.IsImage())
                {
                    if (tbl_ShareHoldersCompany.LogoIcon != "no-photo.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ShareHolders/Logo/" + tbl_ShareHoldersCompany.LogoIcon));
                    }
                    tbl_ShareHoldersCompany.LogoIcon = Guid.NewGuid().ToString() + Path.GetExtension(imgLogoIcon.FileName);
                    imgLogoIcon.SaveAs(Server.MapPath("/Images/ShareHolders/Logo/" + tbl_ShareHoldersCompany.LogoIcon));

                }
                if (imgImageName != null && imgImageName.IsImage())
                {
                    if (tbl_ShareHoldersCompany.ImageName != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ShareHolders/Image/" + tbl_ShareHoldersCompany.ImageName));
                    }
                    tbl_ShareHoldersCompany.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgImageName.FileName);
                    imgImageName.SaveAs(Server.MapPath("/Images/ShareHolders/Image/" + tbl_ShareHoldersCompany.ImageName));

                }
                db.Entry(tbl_ShareHoldersCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_ShareHoldersCompany);
        }

        /// <summary>
        /// حذف تصویر لوگوی سهامدار
        /// </summary>
        /// <param name="id">شناسه تصویر در گالری</param>
        public void DeleteLogoIntroductionCompany(int id)
        {
            Tbl_ShareHoldersCompany qShareHolder = db.Tbl_ShareHoldersCompany.Find(id);
            if (qShareHolder.LogoIcon != "no-photo.jpg")
            {
                System.IO.File.Delete(Server.MapPath("/Images/ShareHolders/Logo/" + qShareHolder.LogoIcon));
                qShareHolder.LogoIcon = "no-photo.jpg";
                db.Entry(qShareHolder).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// حذف تصویر سهامدار
        /// </summary>
        /// <param name="id">شناسه تصویر در گالری</param>
        public void DeleteImageIntroductionCompany(int id)
        {
            Tbl_ShareHoldersCompany qShareHolder = db.Tbl_ShareHoldersCompany.Find(id);
            if (qShareHolder.LogoIcon != null)
            {
                System.IO.File.Delete(Server.MapPath("/Images/ShareHolders/Image/" + qShareHolder.ImageName));
                qShareHolder.ImageName = null;
                db.Entry(qShareHolder).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // GET: Admin/Tbl_ShareHoldersCompany/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_ShareHoldersCompany tbl_ShareHoldersCompany = db.Tbl_ShareHoldersCompany.Find(id);
            if (tbl_ShareHoldersCompany == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ShareHoldersCompany);
        }

        // POST: Admin/Tbl_ShareHoldersCompany/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_ShareHoldersCompany tbl_ShareHoldersCompany = db.Tbl_ShareHoldersCompany.Find(id);
            tbl_ShareHoldersCompany.IsDelete = true;
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
