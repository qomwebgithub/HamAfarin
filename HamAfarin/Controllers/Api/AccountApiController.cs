using Common;
using DataLayer;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using ViewModels;
//using System.Web.Http.Cors;

namespace HamAfarin.Controllers.Api
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountApiController : ApiController
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        SMS oSms = new SMS();
        private readonly SejamClass oSejamClass = new SejamClass();
        CheckNationalCode checkNationalCode = new CheckNationalCode();
        CheckPassword checkPassword = new CheckPassword();
        private readonly string ApiKey = "ApiKey";

        [HttpPost]
        [Route("api/v1/register")]
        public async Task<IHttpActionResult> Register([FromBody] RegisterApiDto register)
        {
            var header = Request.Headers;

            if (header.Contains(ApiKey) == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "لطفا توکن خود را ارسال کنید." });

            var ApiToken = header.GetValues(ApiKey).First();

            if (db.Tbl_ApiToken.Any(a => a.Token == ApiToken) == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 404, Message = "توکن معتبر نمی باشد" });

            if (ModelState.IsValid == false)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                return Json(new { IsSuccess = false, StatusCode = 401, Message = "اطلاعات وارد شده صحیح نیست", errors = errors });
            }

            register.NationalCode = register.NationalCode.Fa2En();

            if (checkNationalCode.check(register.NationalCode, out string Message) == false)
            {
                return Json(new ApiResult { IsSuccess = false, StatusCode = 410, Message = "کد ملی وارد شده صحیح نمی باشد" });
            }

            register.MobileNumber = register.MobileNumber.Fa2En();

            if (StringExtensions.PhoneValid(register.MobileNumber) == false)
            {
                return Json(new ApiResult { IsSuccess = false, StatusCode = 420, Message = "شماره موبایل وارد شده صحیح نمی باشد" });
            }

            if (db.Tbl_Users.Any(u => u.MobileNumber == register.MobileNumber && u.IsActive))
                return Json(new ApiResult { IsSuccess = false, StatusCode = 421, Message = "این شماره قبلا ثبت شده است" });

            if (db.Tbl_Users.Any(u => u.UserName == register.NationalCode && u.IsActive))
                return Json(new ApiResult { IsSuccess = false, StatusCode = 411, Message = "این کدملی قبلا ثبت شده است" });

            Tbl_Users oUser = db.Tbl_Users.FirstOrDefault(u => u.UserName == register.NationalCode && u.IsActive == false);

            if (oUser == null)
            {
                oUser = new Tbl_Users()
                {
                    UserName = register.NationalCode.Fa2En(),
                    MobileNumber = register.MobileNumber.Fa2En(),
                    IsActive = false,
                    IsDeleted = false,
                    RegisterDate = DateTime.Now,
                    Role_id = 2,
                    Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
                    SmsCode = 0,
                    IsLegal = false,
                    UserToken = Guid.NewGuid().ToString(),
                    HasSejam = false,
                    ActivateDate = null,
                    PermanentUserToken = Guid.NewGuid().ToString(),
                };
                db.Tbl_Users.Add(oUser);
                db.SaveChanges();
            }
            else
            {
                oUser.MobileNumber = register.MobileNumber.Fa2En();
                oUser.UserName = register.NationalCode.Fa2En();
                oUser.UserToken = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(oUser.PermanentUserToken))
                {
                    oUser.PermanentUserToken = Guid.NewGuid().ToString();
                }
                db.SaveChanges();
            }



            (bool Success, string UserToken) sejamLogin =
                await oSejamClass.LoginUserAsync(register.NationalCode, oUser.UserID);

            if (sejamLogin.Success == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 430, Message = sejamLogin.UserToken });

            object data = new { UserToken = oUser.UserToken };

            return Json(new ApiResult<object> { Data = data, IsSuccess = true, StatusCode = 201, Message = "عملیات با موفقیت انجام شد" });
        }

        [HttpPost]
        [Route("api/v1/mobileverification")]
        public IHttpActionResult MobileVerification([FromBody] MobileVerificationApiDto verificationDto)
        {
            var header = Request.Headers;

            if (header.Contains(ApiKey) == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "لطفا توکن خود را ارسال کنید." });

            var apiToken = header.GetValues(ApiKey).First();

            var qApiToken = db.Tbl_ApiToken.FirstOrDefault(a => a.Token == apiToken);

            if (qApiToken == null)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 404, Message = "توکن معتبر نمی باشد" });


            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verificationDto.UserToken);
            if (qUser == null)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 405, Message = "کابر یافت نشد" });

            bool VerifySejam = oSejamClass.VerifyUser(qUser.UserToken, verificationDto.VerificationCode, out string Message);

            if (VerifySejam == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 430, Message = Message });

            // 3 = ثبت اطلاعات از سجام
            //var qSms = db.Tbl_Sms.Find(3);
            //var smsResult = oSms.SendSms(qUser.MobileNumber, qSms.Message);

            qUser.UserToken = Guid.NewGuid().ToString();
            qUser.SmsCode = 0;
            qUser.IsActive = true;
            qUser.ActivateDate = DateTime.Now;

            var affiliate = new Tbl_Affiliate()
            {
                Token_Id = qApiToken.ID,
                User_Id = qUser.UserID,
            };

            db.Tbl_Affiliate.Add(affiliate);
            db.SaveChanges();

            // 2 = ثبت نام
            //var qSms2 = db.Tbl_Sms.Find(2);
            //var smsResult2 = oSms.SendSms(qUser.MobileNumber, qSms.Message);

            object data = new { UserAffiliateToken = qUser.PermanentUserToken };

            return Json(new ApiResult<object> { Data = data, IsSuccess = true, StatusCode = 201, Message = "عملیات با موفقیت انجام شد" });
        }

        [HttpPost]
        [Route("api/v1/resendSms")]
        public async Task<IHttpActionResult> ResendSms([FromBody] MobileVerificationApiDto verificationDto)
        {
            var header = Request.Headers;

            if (header.Contains(ApiKey) == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "لطفا توکن خود را ارسال کنید." });

            var apiToken = header.GetValues(ApiKey).First();

            var qApiToken = db.Tbl_ApiToken.FirstOrDefault(a => a.Token == apiToken);

            if (qApiToken == null)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 404, Message = "توکن معتبر نمی باشد" });


            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verificationDto.UserToken);
            if (qUser == null)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 405, Message = "کابر یافت نشد" });

            (bool Success, string UserToken) sejamLogin = await oSejamClass.LoginUserAsync(qUser.UserName, qUser.UserID);

            if (sejamLogin.Success == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 430, Message = sejamLogin.UserToken });

            object data = new { UserToken = qUser.UserToken };

            return Json(new ApiResult<object> { Data = data, IsSuccess = true, StatusCode = 201, Message = "عملیات با موفقیت انجام شد" });
        }

    }
}