using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using DataLayer;
using System.Web.Security;
using Common;
using HamAfarin;
using CaptchaMvc.HtmlHelpers;
using System.Text;
using System.Threading.Tasks;

namespace Hamafarin.Controllers
{
    public class AccountController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        SmsService oSms = new SmsService();
        SejamClass oSejamClass = new SejamClass();
        CheckNationalCode checkNationalCode = new CheckNationalCode();
        CheckPassword checkPassword = new CheckPassword();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl = "/")
        {
            //captcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(model);
            }

            if (ModelState.IsValid == false)
                return View(model);

            string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5");
            model.MobileNumber = StringExtensions.Fa2En(model.MobileNumber);

            Tbl_Users user = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == model.MobileNumber && u.IsDeleted == false && u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError("MobileNumber", "نام کاربری یا کلمه عبور اشتباه است یا کاربری یافت نشد یا حساب کاربری شما فعال نیست");
                return View(model);
            }
            // چک کردن پسورد
            if (user.Password != hashPassword)
            {
                ModelState.AddModelError("MobileNumber", "نام کاربری یا کلمه عبور اشتباه است یا کاربری یافت نشد");
                return View(model);
            }
            //if (user.IsActive == false)
            //{
            //    ModelState.AddModelError("MobileNumber", "حساب کاربری شما فعال نیست");
            //    return View(model);
            //}

            //Request.UrlReferrer.Host()
            // FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);
            string strSetAuthCookie = new UserService().SetCookieString(user);
            FormsAuthentication.SetAuthCookie(strSetAuthCookie, model.RememberMe);

            // 1 = ورود
            Tbl_Sms qSms = db.Tbl_Sms.Find(1);
            oSms.SendSms(user.MobileNumber, qSms.Message);

            return Redirect(ReturnUrl);
        }

        [Route("LogOut")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Register(string id)
        {

            if (id != null)
            {
                id = id.ToLower();
                var token = db.Tbl_ApiToken.Where(u => u.Url == id).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token.TokenHash))
                {
                    HttpCookie affiliateCookie = new HttpCookie("affiliateToken");
                    affiliateCookie.Value = token.TokenHash;
                    affiliateCookie.Expires = DateTime.Now.AddDays(14);
                    Response.Cookies.Add(affiliateCookie);

                    if (id == "signal")
                        ViewBag.Name = token.Name;
                }
            }

            return View();
        }

        //[HttpPost]
        //public ActionResult RegisterSignal()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Register(RegisterViewModel register)
        {
            var transaction = db.Database.BeginTransaction();
            //recaptcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(register);
            }

            if (ModelState.IsValid == false)
                return View(register);

            register.MobileNumber = StringExtensions.Fa2En(register.MobileNumber);

            if (IsMobileNumberExist(register.MobileNumber))
            {
                ModelState.AddModelError("MobileNumber", "موبایل تکراری می باشد");
                return View(register);
            }

            Random random = new Random();
            int smsCode = random.Next(1000, 9999);
            Tbl_Users oUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == register.MobileNumber);

            oUser = new Tbl_Users()
            {
                UserName = StringExtensions.Fa2En(register.MobileNumber),
                MobileNumber = StringExtensions.Fa2En(register.MobileNumber),
                IsActive = false,
                IsDeleted = false,
                RegisterDate = DateTime.Now,
                Role_id = 2,
                Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
                SmsCode = smsCode,
                IsLegal = register.IsLegal,
                UserToken = Guid.NewGuid().ToString(),
                HasSejam = false,
                ActivateDate = null,
            };
            db.Tbl_Users.Add(oUser);

            if (oUser.UserToken == null)
                oUser.UserToken = Guid.NewGuid().ToString();

            db.SaveChanges();

            HttpCookie cookie = Request.Cookies["affiliateToken"];
            if (!string.IsNullOrWhiteSpace(cookie?.Value))
            {
                int? TokenId = db.Tbl_ApiToken.Where(u => u.TokenHash == cookie.Value)
                    .Select(u => u.ID).FirstOrDefault();

                if (TokenId != null)
                {
                    Tbl_Affiliate Tbl_Affiliate = new Tbl_Affiliate() { Token_Id = TokenId, User_Id = oUser.UserID };
                    db.Tbl_Affiliate.Add(Tbl_Affiliate);
                    db.SaveChanges();
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }

            transaction.Commit();

            ViewBag.IsSuccess = true;
            string smsMassage = "کد:" + oUser.SmsCode.ToString() + Environment.NewLine + "از سایت هم آفرین برای شما ارسال شده است";
            oSms.SendSms(oUser.MobileNumber, smsMassage);

            return RedirectToAction(nameof(VerifySms), new { id = oUser.UserToken });
        }

        public async Task<ActionResult> SendSms(string id)
        {
            string Message = "";

            try
            {

                Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == id);
                if (qUser.HasSejam)
                {
                    oSejamClass = new SejamClass();

                    //در صورت استفاده از این متد اکشن را از حالت ای سینک خارج کنید
                    //bool boolSejamLogin = oSejamClass.LoginUser(qUser.UserName, UserSetAuthCookie.GetUserID(User.Identity.Name), out string UserToken);

                    (bool Success, string UserToken) sejamLogin = await oSejamClass.LoginUserAsync(qUser.UserName, UserSetAuthCookie.GetUserID(User.Identity.Name));
                }
                else
                {
                    Random random = new Random();
                    int smsCode = random.Next(1000, 9999);
                    qUser.SmsCode = smsCode;
                    db.SaveChanges();
                    string smsMassage = "کد:" + smsCode.ToString() + Environment.NewLine + "از سایت هم آفرین برای شما ارسال شده است";
                    oSms.SendSms(qUser.MobileNumber, smsMassage);
                }

                return Json(new { success = true, Message = Message }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {

                return Json(new { success = true, Message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult VerifySms(string id, string ReturnUrl = "/")
        {
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == id);

            if (qUser != null)
                return View(new VerifySmsViewModel { UserToken = id, ReturnUrl = ReturnUrl });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifySms(VerifySmsViewModel verifySms)
        {
            //recaptcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(verifySms);
            }

            if (ModelState.IsValid == false)
                return View(verifySms);

            if (string.IsNullOrEmpty(verifySms.SmsCode))
            {
                ModelState.AddModelError("SmsCode", "لطفا کد تاییدیه را وارد کنید");
                return View(verifySms);
            }

            Tbl_Sms qSms;
            // انگلیسی سازی عدد اس ام اس
            verifySms.SmsCode = StringExtensions.Fa2En(verifySms.SmsCode);
            // پیدا کردن کابر از طریق توکن
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verifySms.UserToken);

            if (qUser == null)
            {
                ModelState.AddModelError("SmsCode", "کابر یافت نشد" + verifySms.UserToken);
                return View(verifySms);
            }
            //اگر کاربر ادمین نبود
            if (qUser.Role_id != 1)
            {
                // اگر کابر سجام داشت
                if (qUser.HasSejam)
                {
                    oSejamClass = new SejamClass();
                    bool VerifySejam = oSejamClass.VerifyUser(qUser.UserToken, verifySms.SmsCode, out string Message);

                    if (VerifySejam == false)
                    {
                        ViewBag.profile = Message;
                        ModelState.AddModelError("SmsCode", Message);

                        return View(verifySms);
                    }
                    else
                    {
                        // 3 = ثبت اطلاعات از سجام
                        qSms = db.Tbl_Sms.Find(3);
                        oSms.SendSms(qUser.MobileNumber, qSms.Message);
                    }
                }
                else if (qUser.SmsCode != Convert.ToInt32(verifySms.SmsCode))
                {
                    ModelState.AddModelError("SmsCode", "کد تایید اشتباه است");
                    return View(verifySms);
                }

                qUser.UserToken = Guid.NewGuid().ToString();
                qUser.SmsCode = 0;
                qUser.IsActive = true;
                qUser.ActivateDate = DateTime.Now;
                db.SaveChanges();

            }

            // 2 = ثبت نام
            qSms = db.Tbl_Sms.Find(2);
            oSms.SendSms(qUser.MobileNumber, qSms.Message);

            string strSetAuthCookie = new UserService().SetCookieString(qUser);
            FormsAuthentication.SetAuthCookie(strSetAuthCookie, false);
            return Redirect("/UserPanel/UserProfile");

            //if (string.IsNullOrEmpty(verifySms.ReturnUrl) == false)
            //{
            //    return Redirect(verifySms.ReturnUrl);
            //}
            //else
            //{
            //    return Redirect("/UserPanel/UserProfile");
            //}
        }

        public ActionResult SejamLogin(string ReturnUrl = "/")
        {
            if (User.Identity.IsAuthenticated == false)
                return Redirect("/LogOut");

            // اگر کاربر حقوقی بود اجازه ورود از سجام را نمیدهیم
            if (UserSetAuthCookie.GetIsLegal(User.Identity.Name))
            {
                return Redirect("/");
            }

            return View(new SejamLoginViewModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        public async Task<ActionResult> SejamLogin(SejamLoginViewModel SejamLogin)
        {
            //**************************************************///
            //**************************************************////
            //recaptcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(SejamLogin);
            }
            //recaptcha
            //**************************************************///
            //**************************************************////

            if (ModelState.IsValid == false)
                return View(SejamLogin);

            oSejamClass = new SejamClass();
            SejamLogin.NationalCode = StringExtensions.Fa2En(SejamLogin.NationalCode);

            if (User.Identity.IsAuthenticated == false)
                return Redirect("/LogOut");

            if (oSejamClass.CheckNationalCode(SejamLogin.NationalCode, UserSetAuthCookie.GetUserID(User.Identity.Name)) == false)
            {
                ModelState.AddModelError("NationalCode", "کد ملی وارد شده قبلا ثبت شده است ");
                return View(new SejamLoginViewModel { ReturnUrl = SejamLogin.ReturnUrl });
            }

            //در صورت استفاده از این متد اکشن را از حالت ای سینک خارج کنید
            //bool boolSejamLogin = oSejamClass.LoginUser(SejamLogin.NationalCode, UserSetAuthCookie.GetUserID(User.Identity.Name), out string UserToken);

            (bool Success, string UserToken) sejamLogin = await oSejamClass.LoginUserAsync(SejamLogin.NationalCode, UserSetAuthCookie.GetUserID(User.Identity.Name));

            if (sejamLogin.Success)
                return RedirectToAction(nameof(VerifySms), new { id = sejamLogin.UserToken, ReturnUrl = SejamLogin.ReturnUrl });

            ViewBag.Exemption = true;
            ModelState.AddModelError("NationalCode", sejamLogin.UserToken);

            return View(new SejamLoginViewModel { ReturnUrl = SejamLogin.ReturnUrl });
        }

        bool IsUserNameExist(string username)
        {
            username = StringExtensions.Fa2En(username);
            return db.Tbl_Users.Any(u => u.UserName == username.Trim().ToLower() && u.IsActive);
        }

        bool IsMobileNumberExist(string mobileNumber)
        {
            mobileNumber = StringExtensions.Fa2En(mobileNumber);

            int? userId = db.Tbl_Users.Where(u => u.MobileNumber == mobileNumber).Select(u => u.UserID).FirstOrDefault();

            if (userId != null)
                return db.Tbl_UserProfiles.Any(p => p.User_id == userId);

            return false;
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

        [HttpPost]
        public JsonResult CheckPassword(string Password)
        {
            if (checkPassword.check(Password, out string Message))
            {
                return Json(true, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// چک کردن کد ملی
        /// </summary>
        /// <param name="NationalCode">کد ملی</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DuplicateNationalCode(string NationalCode, string LastNationalCode = "")
        {
            if (checkNationalCode.check(NationalCode, out string Message))
            {
                if (db.Tbl_Users.Where(p => p.UserName == NationalCode).Any() && NationalCode != LastNationalCode)
                {
                    return Json("کد ملی تکراری است", JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                return Json("کد ملی نامعتبر است", JsonRequestBehavior.DenyGet);
            }


        }

        public async Task<JsonResult> TestApi(string id)
        {
            (bool Success, string Message) kycOtpResulat = await oSejamClass.kycOtpHttpClientAsync(id);
            return Json(kycOtpResulat.Message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {

            if (ModelState.IsValid == false)
                return View();

            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == forgotPassword.MobileNumber);

            if (qUser == null)
            {
                ModelState.AddModelError("MobileNumber", "کاربر یافت نشد" + forgotPassword.MobileNumber);
                return View(forgotPassword);
            }

            Random rndSmsCode = new Random();
            int smsCode = rndSmsCode.Next(1000, 9999);
            qUser.SmsCode = smsCode;
            db.SaveChanges();
            string smsMassage = "کد:" + smsCode.ToString() + Environment.NewLine + "از سایت هم آفرین برای شما ارسال شده است";
            oSms.SendSms(qUser.MobileNumber, smsMassage);
            return RedirectToAction(nameof(VerifyForgotPassword), new { id = qUser.UserToken });
        }

        public ActionResult VerifyForgotPassword(string id)
        {
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == id);

            if (qUser == null)
                return HttpNotFound();

            return View(new VerifySmsViewModel { UserToken = id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyForgotPassword(VerifySmsViewModel verifySms)
        {
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(verifySms);
            }

            if (ModelState.IsValid == false)
                return View(verifySms);

            if (string.IsNullOrEmpty(verifySms.SmsCode))
            {
                ModelState.AddModelError("SmsCode", "لطفا کد تاییدیه را وارد کنید");
                return View(verifySms);
            }

            // انگلیسی سازی عدد اس ام اس
            verifySms.SmsCode = StringExtensions.Fa2En(verifySms.SmsCode);
            // پیدا کردن کابر از طریق توکن
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verifySms.UserToken);

            if (qUser == null)
            {
                ModelState.AddModelError("SmsCode", "کابر یافت نشد" + verifySms.UserToken);
                return View(verifySms);
            }

            if (qUser.SmsCode != Convert.ToInt32(verifySms.SmsCode))
            {
                ModelState.AddModelError("SmsCode", "کد تایید اشتباه است");
                return View(verifySms);
            }

            qUser.UserToken = Guid.NewGuid().ToString();
            // صفر کردن کد اس ام اس برای امنیت است تا کاربر با گویید وارد متد بعدی نشود
            qUser.SmsCode = 0;
            db.SaveChanges();
            return RedirectToAction(nameof(ResetPassword), new { id = qUser.UserToken });
        }

        public ActionResult ResetPassword(string id)
        {
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == id && u.SmsCode == 0);

            if (qUser == null)
                return HttpNotFound();

            return View(new ResetPasswordViewModel { UserToken = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(resetPassword);
            }
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == resetPassword.UserToken && u.SmsCode == 0);

            if (ModelState.IsValid == false)
                return View(resetPassword);

            qUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(resetPassword.Password, "MD5");
            qUser.UserToken = Guid.NewGuid().ToString();
            db.SaveChanges();
            return RedirectToAction(nameof(Login));
        }


        public ActionResult Affiliate(string id, int? planId)
        {
            if (id != null)
            {
                Tbl_ApiToken token = db.Tbl_ApiToken.Where(u => u.Url == id).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token.TokenHash))
                {
                    HttpCookie affiliateCookie = new HttpCookie("affiliateToken");
                    affiliateCookie.Value = token.TokenHash;
                    affiliateCookie.Expires = DateTime.Now.AddDays(14);
                    Response.Cookies.Add(affiliateCookie);
                }
            }

            if (planId == null)
                return RedirectToAction("Index", "BusinessPlans");

            return RedirectToAction("SingleBusinessPlan", "BusinessPlans", new { id = planId });
        }


    }
}