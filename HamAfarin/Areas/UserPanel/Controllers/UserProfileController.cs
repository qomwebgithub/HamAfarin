using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common;
using DataLayer;
using Hamafarin;
using ViewModels;

namespace HamAfarin.Areas.UserPanel
{
    public class UserProfileController : Controller
    {
        private HamAfarinDBEntities db = new HamAfarinDBEntities();
        UserService userService = new UserService();
                CheckNationalCode checkNationalCode = new CheckNationalCode();

        // GET: UserPanel/Tbl_UserProfiles
        // GET: UserPanel/Tbl_UserProfiles
        public ActionResult Index()
        {
            UserProfileViewModel userProfileViewModel = GetUserProfile();
            if (userProfileViewModel == null)
            {
                return RedirectToAction("Create");
            }
            if (TempData.ContainsKey("EditProfileSuccess"))
                ViewBag.EditProfileSuccess = TempData["EditProfileSuccess"];
            return View(userProfileViewModel);
        }

        private UserProfileViewModel GetUserProfile()
        {
            // Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles qProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == UserID);
            if (qProfile == null)
            {
                return null;
            }

            ProfileViewModel profileViewModel = null;
            if (qProfile != null)
                profileViewModel = userService.ConvertTblProfileToProfileModel(qProfile);
            Tbl_PersonLegal qLegal = db.Tbl_PersonLegal.FirstOrDefault(p => p.User_id == UserID);
            PersonLegalViewModel personLegalViewModel = null;
            if (qLegal != null)
                personLegalViewModel = userService.ConvertTblLegalToLegalModel(qLegal);
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
            {
                IsActive = qProfile.IsActive,
                UserName = UserSetAuthCookie.GetUserName(User.Identity.Name),
                Profile = profileViewModel,
                IsLegal = false,
                strGender = qProfile.Gender,
                PersonLegal = personLegalViewModel,
                NationalCode = qProfile.NationalCode,
                Profile_id = qProfile.ProfileID
            };
            return userProfileViewModel;
        }



        // GET: UserPanel/Tbl_UserProfiles/Create
        public ActionResult Create()
        {
            if (GetUserProfile() != null)
            {
                return RedirectToAction("Index");
            }
            List<DropDownViewModel> genderList = new List<DropDownViewModel>()
            {
                new DropDownViewModel(){key = 1,value= "مرد"},
                new DropDownViewModel(){key = 2,value= "زن"}
            };
            ViewBag.Gender = new SelectList(genderList, "key", "value", 1);
            UserProfileViewModel userProfile = new UserProfileViewModel()
            {
                Profile = new ProfileViewModel()
                {
                    MobileNumber = UserSetAuthCookie.GetMobileNumber(User.Identity.Name)
                }
            };
            return View(userProfile);
        }

        // POST: UserPanel/Tbl_UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfileViewModel userProfile, HttpPostedFileBase legalFile)
        {
            if (!userProfile.IsLegal)
            {
                ModelState.Remove("PersonLegal.CompanyName");
                ModelState.Remove("PersonLegal.EconomicCode");
                ModelState.Remove("PersonLegal.NationalId");
                ModelState.Remove("PersonLegal.RegistratioNumber");
                ModelState.Remove("PersonLegal.Address");
            }

            ////////////****************/////////////////////////////
            // تبدیل تاریخ تولد از 
            // string
            // به
            // datetime
            if (string.IsNullOrEmpty(userProfile.strBirthDate) == false)
            {
                ModelState.Remove("Profile.BirthDate");
                userProfile.Profile.BirthDate = StringExtensions.StringToDate(userProfile.strBirthDate);
            }
            else
            {
                ModelState.AddModelError("strBirthDate", "تاریخ تولد را انتخاب کنید");
                List<DropDownViewModel> genderList1 = new List<DropDownViewModel>()
            {
                new DropDownViewModel(){key = 1,value= "مرد"},
                new DropDownViewModel(){key = 2,value= "زن"}
            };
                ViewBag.Gender = new SelectList(genderList1, "key", "value", 1);
                return View(userProfile);
            }
            if (userProfile.Gender == 1)
            {
                userProfile.strGender = "Male";
            }
            else
            {
                userProfile.strGender = "FeMale";
            }
            if (ModelState.IsValid)
            {
                if (legalFile != null)
                {
                    userProfile.PersonLegal.LegalFile = Guid.NewGuid().ToString() + Path.GetExtension(legalFile.FileName);
                    legalFile.SaveAs(Server.MapPath("/UploadFiles/LegalFiles/" + userProfile.PersonLegal.LegalFile));

                }
                int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                Tbl_UserProfiles Tbl_UserProfiles = new Tbl_UserProfiles()
                {
                    IsActive = false,
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
                    User_id = userId,
                    MobileNumber = userProfile.Profile.MobileNumber,
                    FirstName = userProfile.Profile.FirstName,
                    LastName = userProfile.Profile.LastName,
                    Bio = userProfile.Profile.Bio,
                    NationalCode = userProfile.NationalCode,
                    FatherName = userProfile.Profile.FatherName,
                    ProfileNationalId = userProfile.Profile.ProfileNationalId,
                    SejamCode = userProfile.Profile.SejamCode,
                    AccountNumber = userProfile.Profile.AccountNumber,
                    AccountSheba = userProfile.Profile.AccountSheba,
                    Email = userProfile.Profile.Email,
                    BirthDate = userProfile.Profile.BirthDate,
                    Gender = userProfile.strGender,
                };

                db.Tbl_UserProfiles.Add(Tbl_UserProfiles);
                if (userProfile.IsLegal)
                {
                    Tbl_PersonLegal personLegal = new Tbl_PersonLegal()
                    {
                        IsActive = false,
                        IsDelete = false,
                        CreateDate = DateTime.Now,
                        User_id = userId,
                        CompanyName = userProfile.PersonLegal.CompanyName,
                        NationalId = userProfile.PersonLegal.NationalId,
                        RegistratioNumber = userProfile.PersonLegal.RegistratioNumber,
                        Address = userProfile.PersonLegal.Address,
                        LegalFile = userProfile.PersonLegal.LegalFile
                    };
                    db.Tbl_PersonLegal.Add(personLegal);
                }
                db.SaveChanges();
                //به اکشن ایندکس ارسال میشود برای اعلام موفقیت آمیز بودن پروفایل
                TempData["EditProfileSuccess"] = true;
                return RedirectToAction("Index");
            }
            List<DropDownViewModel> genderList = new List<DropDownViewModel>()
            {
                new DropDownViewModel(){key = 1,value= "مرد"},
                new DropDownViewModel(){key = 2,value= "زن"}
            };
            ViewBag.Gender = new SelectList(genderList, "key", "value", 1);
            return View(userProfile);
        }

        // GET: UserPanel/Tbl_UserProfiles/Edit/5
        public ActionResult Edit()
        {
            UserService userService = new UserService();
            // Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles qProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == UserID);
            ProfileViewModel profileViewModel = null;
            Nullable<int> qGender = null;
            if (qProfile != null)
            {
                profileViewModel = userService.ConvertTblProfileToProfileModel(qProfile);
                // qGender = qProfile.Gender.Value;
            }

            Tbl_PersonLegal qLegal = db.Tbl_PersonLegal.FirstOrDefault(p => p.User_id == UserID);
            PersonLegalViewModel personLegalViewModel = null;
            if (qLegal != null)
                personLegalViewModel = userService.ConvertTblLegalToLegalModel(qLegal);
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
            {
                IsActive = true,
                UserName = UserSetAuthCookie.GetUserName(User.Identity.Name),
                Profile = profileViewModel,
                IsLegal = false,
                PersonLegal = personLegalViewModel,
                Gender = qGender.Value,
            };
            List<DropDownViewModel> genderList = new List<DropDownViewModel>()
            {
                new DropDownViewModel(){key = 1,value= "مرد"},
                new DropDownViewModel(){key = 2,value= "زن"}
            };
            if (qGender == null)
                ViewBag.Gender = new SelectList(genderList, "key", "value", 1);
            else ViewBag.Gender = new SelectList(genderList, "key", "value", qGender);

            return View(userProfileViewModel);
        }

        // POST: UserPanel/Tbl_UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfileViewModel userProfile)
        {
            if (!userProfile.IsLegal)
            {
                ModelState.Remove("PersonLegal.CompanyName");
                ModelState.Remove("PersonLegal.EconomicCode");
                ModelState.Remove("PersonLegal.NationalId");
                ModelState.Remove("PersonLegal.RegistratioNumber");
                ModelState.Remove("PersonLegal.Address");
            }
            //////////////****************/////////////////////////////
            //// تبدیل تاریخ تولد از 
            //// string
            //// به
            //// datetime
            if (string.IsNullOrEmpty(userProfile.strBirthDate) == false)
            {
                userProfile.Profile.BirthDate = StringExtensions.StringToDate(userProfile.strBirthDate);
            }
            if (ModelState.IsValid)
            {
                int userId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                Tbl_UserProfiles UserProfile = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == userId);

                UserProfile.IsActive = false;
                UserProfile.MobileNumber = userProfile.Profile.MobileNumber;
                UserProfile.FirstName = userProfile.Profile.FirstName;
                UserProfile.LastName = userProfile.Profile.LastName;
                UserProfile.Bio = userProfile.Profile.Bio;
                UserProfile.NationalCode = userProfile.NationalCode;
                UserProfile.FatherName = userProfile.Profile.FatherName;
                UserProfile.ProfileNationalId = userProfile.Profile.ProfileNationalId;
                UserProfile.SejamCode = userProfile.Profile.SejamCode;
                UserProfile.AccountNumber = userProfile.Profile.AccountNumber;
                UserProfile.AccountSheba = userProfile.Profile.AccountSheba;
                UserProfile.Email = userProfile.Profile.Email;
                UserProfile.BirthDate = userProfile.Profile.BirthDate;
                UserProfile.Gender = userProfile.strGender;
                db.SaveChanges();


                Tbl_Users tbl_Users = db.Tbl_Users.FirstOrDefault(p => p.UserID == userId);
                tbl_Users.IsLegal = userProfile.IsLegal;
                db.SaveChanges();
                if (userProfile.IsLegal)
                {
                    Tbl_PersonLegal personLegal = db.Tbl_PersonLegal.FirstOrDefault(p => p.User_id == userId);

                    personLegal.CompanyName = userProfile.PersonLegal.CompanyName;
                    personLegal.NationalId = userProfile.PersonLegal.NationalId;
                    personLegal.RegistratioNumber = userProfile.PersonLegal.RegistratioNumber;
                    personLegal.Address = userProfile.PersonLegal.Address;
                    db.SaveChanges();
                }
                //به اکشن ایندکس ارسال میشود برای اعلام موفقیت آمیز بودن پروفایل
                TempData["EditProfileSuccess"] = true;
                return RedirectToAction("Index");
            }
            List<DropDownViewModel> genderList = new List<DropDownViewModel>()
            {
                new DropDownViewModel(){key = 1,value= "مرد"},
                new DropDownViewModel(){key = 2,value= "زن"}
            };
            ViewBag.Gender = new SelectList(genderList, "key", "value", userProfile.Gender);
            return View(userProfile);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(changePassword.OldPassword, "MD5");
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == UserID && u.Password == oldPassword);
            if (qUser == null)
            {
                ModelState.AddModelError("OldPassword", "رمز صحیح نیست");
                return View(changePassword);
            }
            else
            {
                string newPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(changePassword.NewPassword, "MD5");
                qUser.Password = newPassword;
                db.SaveChanges();
                return View();
            }
        }

        // GET: UserPanel/Tbl_UserProfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_UserProfiles Tbl_UserProfiles = db.Tbl_UserProfiles.Find(id);
            if (Tbl_UserProfiles == null)
            {
                return HttpNotFound();
            }
            return View(Tbl_UserProfiles);
        }

        // POST: UserPanel/Tbl_UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_UserProfiles Tbl_UserProfiles = db.Tbl_UserProfiles.Find(id);
            db.Tbl_UserProfiles.Remove(Tbl_UserProfiles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CheckNationalCode(string NationalCode)
        {
            if (checkNationalCode.check(NationalCode, out string Message))
            {
                return Json(true, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }

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
