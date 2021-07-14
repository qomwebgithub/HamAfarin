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
        SMS oSms = new SMS();
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
            //**************************************************///
            //**************************************************////
            //recaptcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(model);
            }
            //recaptcha
            //**************************************************///
            //**************************************************////

            if (ModelState.IsValid)
            {
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5");
                model.MobileNumber = StringExtensions.Fa2En(model.MobileNumber);
                Tbl_Users user = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == model.MobileNumber &&
                  u.Password == hashPassword);

                if (user != null)
                {
                    if (user.IsActive && user.IsDeleted == false)
                    {
                        //Request.UrlReferrer.Host()
                        // FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);
                        string strSetAuthCookie = user.UserID + "," + user.Role_id + "," + user.UserName + "," + user.MobileNumber;
                        FormsAuthentication.SetAuthCookie(strSetAuthCookie, model.RememberMe);

                        string formatedMobileNumber = "98" + user.MobileNumber.Substring(1);
                        // 1 = ورود
                        Tbl_Sms qSms = db.Tbl_Sms.Find(1);
                        (bool Success, string Message) result = oSms.AdpSendSms(formatedMobileNumber, qSms.Message);

                        if (user.Role_id == 1)
                            return Redirect(ReturnUrl);
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("MobileNumber", "حساب کاربری شما فعال نیست");
                    }
                }
                else
                {
                    ModelState.AddModelError("MobileNumber", "نام کاربری یا کلمه عبور اشتباه است یا کاربری یافت نشد");
                }
            }
            return View(model);
        }

        [Route("LogOut")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegiserViewModel regiser)
        {
            //**************************************************///
            //**************************************************////
            //recaptcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(regiser);
            }
            //recaptcha
            //**************************************************///
            //**************************************************////
            if (ModelState.IsValid)
            {
                //if (!IsUserNameExist(regiser.UserName))
                //{
                regiser.MobileNumber = StringExtensions.Fa2En(regiser.MobileNumber);

                if (!IsMobileNumberExist(regiser.MobileNumber))
                {
                    Random rndSmsCode = new Random();
                    int smsCode = rndSmsCode.Next(1000, 9999);
                    Tbl_Users oUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == regiser.MobileNumber);

                    oUser = new Tbl_Users()
                    {
                        UserName = StringExtensions.Fa2En(regiser.MobileNumber),
                        MobileNumber = StringExtensions.Fa2En(regiser.MobileNumber),
                        IsActive = false,
                        IsDeleted = false,
                        RegisterDate = DateTime.Now,
                        Role_id = 2,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(regiser.Password, "MD5"),
                        SmsCode = smsCode,
                        IsLegal = regiser.IsLegal,
                        UserToken = Guid.NewGuid().ToString(),
                        HasSejam = false,
                        ActivateDate = null,
                    };
                    db.Tbl_Users.Add(oUser);

                    if (oUser.UserToken == null)
                    {
                        oUser.UserToken = Guid.NewGuid().ToString();
                    }

                    db.SaveChanges();

                    ViewBag.IsSuccess = true;

                    oSms.SendSMS(oUser.MobileNumber, oUser.SmsCode.ToString());

                    return RedirectToAction("VerifySms", new { id = oUser.UserToken });
                }
                else
                {
                    ModelState.AddModelError("MobileNumber", "موبایل تکراری می باشد");
                }
                //}
                //else
                //{
                //    ModelState.AddModelError("UserName", "نام کاربری تکراری می باشد");
                //}
            }
            return View(regiser);
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
                    Random rndSmsCode = new Random();
                    int smsCode = rndSmsCode.Next(1000, 9999);
                    qUser.SmsCode = smsCode;
                    db.SaveChanges();
                    oSms.SendSMS(qUser.MobileNumber, qUser.SmsCode.ToString());
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
            {
                return View(new VerifySmsViewModel { UserToken = id, ReturnUrl = ReturnUrl });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifySms(VerifySmsViewModel verifySms)
        {
            //**************************************************///
            //**************************************************////
            //recaptcha
            if (!this.IsCaptchaValid("عبارت امنیتی را درست وارد کنید"))
            {
                ModelState.AddModelError("CaptchaInputText", "عبارت امنیتی را درست وارد کنید");
                return View(verifySms);
            }
            //recaptcha
            //**************************************************///
            //**************************************************////
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(verifySms.SmsCode) == false)
                {
                    verifySms.SmsCode = StringExtensions.Fa2En(verifySms.SmsCode);

                    Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verifySms.UserToken);

                    Tbl_Sms qSms;
                    string formatedMobileNumber;
                    (bool Success, string Message) result;

                    if (qUser != null)
                    {
                        if (qUser.Role_id != 1)
                        {
                            if (qUser.HasSejam)
                            {
                                oSejamClass = new SejamClass();
                                bool VerifySejam = oSejamClass.VerifyUser(verifySms, out string Message);
                                if (VerifySejam == false)
                                {
                                    ViewBag.profile = Message;
                                    ModelState.AddModelError("SmsCode", Message);

                                    // 3 = ثبت اطلاعات از سجام
                                    qSms = db.Tbl_Sms.Find(3);
                                    formatedMobileNumber = "98" + qUser.MobileNumber.Substring(1);
                                    result = oSms.AdpSendSms(formatedMobileNumber, qSms.Message);

                                    return View(verifySms);
                                }
                            }
                            else if (qUser.SmsCode != Convert.ToInt32(verifySms.SmsCode))
                            {
                                ModelState.AddModelError("SmsCode", "کد تایید اشتباه است");
                                return View(verifySms);
                            }

                            qUser.SmsCode = 0;
                            qUser.IsActive = true;
                            db.SaveChanges();

                        }

                        // 2 = ثبت نام
                        qSms = db.Tbl_Sms.Find(2);
                        formatedMobileNumber = "98" + qUser.MobileNumber.Substring(1);
                        result = oSms.AdpSendSms(formatedMobileNumber, qSms.Message);


                        string strSetAuthCookie = qUser.UserID + "," + qUser.Role_id + "," + qUser.UserName + "," + qUser.MobileNumber;
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
                }
            }

            return View(verifySms);
        }

        public ActionResult SejamLogin(string ReturnUrl = "/")
        {
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
            if (ModelState.IsValid)
            {


                oSejamClass = new SejamClass();
                SejamLogin.NationalCode = StringExtensions.Fa2En(SejamLogin.NationalCode);
                if (User.Identity.IsAuthenticated == false)
                {
                    return Redirect("/LogOut");
                }

                if (oSejamClass.CheckNationalCode(SejamLogin.NationalCode, UserSetAuthCookie.GetUserID(User.Identity.Name)) == false)
                {
                    ModelState.AddModelError("NationalCode", "کد ملی وارد شده قبلا ثبت شده است ");
                    return View(new SejamLoginViewModel { ReturnUrl = SejamLogin.ReturnUrl });
                }

                //در صورت استفاده از این متد اکشن را از حالت ای سینک خارج کنید
                //bool boolSejamLogin = oSejamClass.LoginUser(SejamLogin.NationalCode, UserSetAuthCookie.GetUserID(User.Identity.Name), out string UserToken);

                (bool Success, string UserToken) sejamLogin = await oSejamClass.LoginUserAsync(SejamLogin.NationalCode, UserSetAuthCookie.GetUserID(User.Identity.Name));

                if (sejamLogin.Success)
                {
                    return RedirectToAction("VerifySms", new { id = sejamLogin.UserToken, ReturnUrl = SejamLogin.ReturnUrl });
                }
                ViewBag.Exemption = true;
                ModelState.AddModelError("NationalCode", sejamLogin.UserToken);

                return View(new SejamLoginViewModel { ReturnUrl = SejamLogin.ReturnUrl });
            }
            return View(SejamLogin);
        }

        bool IsUserNameExist(string username)
        {
            username = StringExtensions.Fa2En(username);
            return db.Tbl_Users.Any(u => u.UserName == username.Trim().ToLower() && u.IsActive);
        }

        bool IsMobileNumberExist(string mobileNumber)
        {
            mobileNumber = StringExtensions.Fa2En(mobileNumber);
            return db.Tbl_Users.Any(u => u.MobileNumber == mobileNumber.Trim().ToLower() && u.IsActive);
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
    }
}