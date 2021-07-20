using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using Hamafarin;
using ViewModels;

namespace HamAfarin.Areas.Admin.Controllers
{
    public class Tbl_UsersController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();

        // GET: Admin/Tbl_Users
        public ActionResult Index()
        {
            var tbl_Users = db.Tbl_Users.Include(t => t.Tbl_Roles);
            return View(tbl_Users.OrderByDescending(c => c.RegisterDate).ToList());
        }

        // GET: Admin/Tbl_Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Users tbl_Users = db.Tbl_Users.Find(id);
            if (tbl_Users == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Users);
        }

        // GET: Admin/Tbl_Users/Create
        public ActionResult Create()
        {
            ViewBag.Role_id = new SelectList(db.Tbl_Roles, "RoleID", "RoleSystemName");
            return View();
        }

        // POST: Admin/Tbl_Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tbl_Users tbl_Users)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Users.Add(tbl_Users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Role_id = new SelectList(db.Tbl_Roles, "RoleID", "RoleSystemName", tbl_Users.Role_id);
            return View(tbl_Users);
        }

        // GET: Admin/Tbl_Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Users tbl_Users = db.Tbl_Users.Find(id);
            if (tbl_Users == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role_id = new SelectList(db.Tbl_Roles, "RoleID", "RoleSystemName", tbl_Users.Role_id);
            return View(tbl_Users);
        }

        // POST: Admin/Tbl_Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tbl_Users tbl_Users)
        {



            bool blnPreviosIsActive;
            using (HamAfarinDBEntities dbTest = new HamAfarinDBEntities())
            {
                blnPreviosIsActive = dbTest.Tbl_Users.Find(tbl_Users.UserID).IsActive;
            }

            if (blnPreviosIsActive == false && tbl_Users.IsActive)
            {
                tbl_Users.ActivateDate = DateTime.Now;
            }
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Role_id = new SelectList(db.Tbl_Roles, "RoleID", "RoleSystemName", tbl_Users.Role_id);
            return View(tbl_Users);
        }

        // GET: Admin/Tbl_Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Users tbl_Users = db.Tbl_Users.Find(id);
            if (tbl_Users == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Users);
        }

        // POST: Admin/Tbl_Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Users tbl_Users = db.Tbl_Users.Find(id);
            db.Tbl_Users.Remove(tbl_Users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// دریافت لیست پروفایلهای تایید نشده
        /// </summary>
        /// <returns>لیست پروفایلهای تایید نشده</returns>
        public ActionResult UnSubmittedProfile()
        {
            List<Tbl_UserProfiles> qListProfile = db.Tbl_UserProfiles.Where(p => p.IsActive == false && p.IsDeleted == false).ToList();
            List<ProfileItemViewModel> listProfileItems = new List<ProfileItemViewModel>();
            foreach (var item in qListProfile)
            {
                listProfileItems.Add(new ProfileItemViewModel()
                {
                    ProfileID = item.ProfileID,
                    MobileNumber = item.MobileNumber,
                    Name = item.FirstName + " " + item.LastName,
                    Gender = item.Gender,
                    IsLegal = item.Tbl_Users.IsLegal == true ? "حقوقی" : "حقیقی",
                    UserName = item.Tbl_Users.UserName,
                });
            }
            return View(listProfileItems);
        }
        /// <summary>
        /// دریافت اطلاعات پروفایل
        /// </summary>
        /// <param name="id">شناسه پروفایل</param>
        /// <returns>آیتم پروفایل</returns>
        public ActionResult EditProfile(int id)
        {
            Tbl_UserProfiles qProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.ProfileID == id);
            UserService userService = new UserService();
            ProfileViewModel profileViewModel = userService.ConvertTblProfileToProfileModel(qProfile);
            PersonLegalViewModel legalViewModel = null;
            if (qProfile.Tbl_Users.IsLegal)
            {
                Tbl_PersonLegal qPersonLegal = db.Tbl_PersonLegal.FirstOrDefault(p => p.User_id == qProfile.User_id);
                legalViewModel = userService.ConvertTblLegalToLegalModel(qPersonLegal);
            }
            //DateTime dt = DateTime.ParseExact(qProfile.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
            {
                UserName = qProfile.Tbl_Users.UserName,
                IsActive = qProfile.IsActive,
                IsLegal = qProfile.Tbl_Users.IsLegal,
                strGender = qProfile.Gender,
                Profile = profileViewModel,
                Profile_id = profileViewModel.ProfileID,
                PersonLegal = legalViewModel,
                NationalCode = qProfile.NationalCode
            };
            return View(userProfileViewModel);
        }

        // POST: Admin/Tbl_Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel userProfileView)
        {
            Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.Find(userProfileView.Profile_id);
            tbl_UserProfiles.IsActive = userProfileView.IsActive;
            db.Entry(tbl_UserProfiles).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UnSubmittedProfile");
        }

        public ActionResult PaymentList(int id)
        {
            var tbl_BusinessPlanPayment = db.Tbl_BusinessPlanPayment.Where(p => p.PaymentUser_id == id).Include(t => t.Tbl_PaymentType).Include(t => t.Tbl_Users).Include(t => t.Tbl_Users1).Include(t => t.Tbl_BussinessPlans);
            return View(tbl_BusinessPlanPayment.OrderByDescending(c => c.PaidDateTime).ToList());
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
