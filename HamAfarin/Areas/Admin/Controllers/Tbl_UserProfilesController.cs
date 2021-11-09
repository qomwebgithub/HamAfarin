using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Common;
using DataLayer;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_UserProfilesController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_UserProfiles
        public ActionResult Index()
        {
            var tbl_UserProfiles = db.Tbl_UserProfiles.Where(p => p.IsDeleted == false).Include(t => t.Tbl_Users);
            return View(tbl_UserProfiles.ToList());
        }

        public FileResult ExcelReport(string startDate, string endDate)
        {
            var userProfileList = db.Tbl_UserProfiles
                .Where(p => p.IsDeleted == false);

            if (startDate.HasValue())
            {
                var date = StringExtensions.StringToDate(startDate);
                userProfileList = userProfileList.Where(p => p.CreateDate >= date);
            }
            if (endDate.HasValue())
            {
                var date = StringExtensions.StringToDate(endDate);
                userProfileList = userProfileList.Where(p => p.CreateDate <= date);
            }

            userProfileList = userProfileList.OrderBy(p => p.CreateDate);

            List<string> lstColumnsName = new List<string>
            {
                "تاریخ ایجاد",
                "موبایل",
                "نام",
                "نام خانوادگی",
                "کد ملی",
                "نام پدر",
                "کد سجام",
                "شماره کاربری",
                "ش حساب شبا",
                "ایمیل",
                "تاریخ تولد",
                "جنسیت",
            };

            DataTable dt = new DataTable("Grid");
            foreach (var item in lstColumnsName)
            {
                dt.Columns.Add(item);
            }

            foreach (var item in userProfileList.ToList())
            {
                dt.Rows.Add(
                    item.CreateDate,
                    item.MobileNumber,
                    item.FirstName,
                    item.LastName,
                    item.NationalCode,
                    item.FatherName,
                    item.SejamCode,
                    item.AccountNumber,
                    item.AccountSheba,
                    item.Email,
                    item.BirthDate,
                    item.Gender
                );
            }

            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook  
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"فهرست پروفایل کاربران ({DateTime.Now.ToString("yyyy-MM-dd")}).xlsx");
                }
            }
        }

        // GET: Admin/Tbl_UserProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == id);
            if (tbl_UserProfiles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_UserProfiles);
        }

        // GET: Admin/Tbl_UserProfiles/Create
        public ActionResult Create()
        {
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName");
            return View();
        }

        // POST: Admin/Tbl_UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_UserProfiles tbl_UserProfiles)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_UserProfiles.Add(tbl_UserProfiles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_UserProfiles.User_id);
            return View(tbl_UserProfiles);
        }

        // GET: Admin/Tbl_UserProfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.Find(id);
            if (tbl_UserProfiles == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_UserProfiles.User_id);
            return View(tbl_UserProfiles);
        }

        // POST: Admin/Tbl_UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_UserProfiles tbl_UserProfiles)
        {
            if (ModelState.IsValid)
            {
                ////////////****************/////////////////////////////
                // تبدیل تاریخ تولد از 
                // string
                // به
                // datetime
                if (string.IsNullOrEmpty(tbl_UserProfiles.strBirthDate) == false)
                {
                    ModelState.Remove("BirthDate");
                    tbl_UserProfiles.BirthDate = StringExtensions.StringToDate(tbl_UserProfiles.strBirthDate);
                }
                db.Entry(tbl_UserProfiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_id = new SelectList(db.Tbl_Users, "UserID", "UserName", tbl_UserProfiles.User_id);
            return View(tbl_UserProfiles);
        }

        // GET: Admin/Tbl_UserProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.Find(id);
            if (tbl_UserProfiles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_UserProfiles);
        }

        // POST: Admin/Tbl_UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.FirstOrDefault(p => p.ProfileID == id);
            //  db.Tbl_UserProfiles.Remove(tbl_UserProfiles);
            tbl_UserProfiles.IsDeleted = true;
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
