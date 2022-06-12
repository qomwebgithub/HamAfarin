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
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "توکن معتبر نمی باشد" });

            register.MobileNumber = register.MobileNumber.Fa2En();

            if (ModelState.IsValid == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "خطا" });


            if (db.Tbl_Users.Any(u => u.MobileNumber == register.MobileNumber))
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "این شماره قبلا ثبت شده است" });

            if (db.Tbl_UserProfiles.Any(u => u.NationalCode == register.NationalCode))
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "این کدملی قبلا ثبت شده است" });

            Tbl_Users oUser = db.Tbl_Users.FirstOrDefault(u => u.MobileNumber == register.MobileNumber);
            oUser = new Tbl_Users()
            {
                UserName = register.MobileNumber.Fa2En(),
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
            };
            db.Tbl_Users.Add(oUser);
            db.SaveChanges();

            (bool Success, string UserToken) sejamLogin =
                await oSejamClass.LoginUserAsync(register.NationalCode, oUser.UserID);

            if (sejamLogin.Success == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = sejamLogin.UserToken });

            return Json(new ApiResult<string> { Data = oUser.UserToken, IsSuccess = true, StatusCode = 201, Message = "عملیات با موفقیت انجام شد" });
        }

        [HttpPost]
        [Route("api/v1/mobileverification")]
        public IHttpActionResult MobileVerification([FromBody] MobileVerificationApiDto verificationDto)
        {
            var header = Request.Headers;

            if (header.Contains(ApiKey) == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "لطفا توکن خود را ارسال کنید." });

            var apiToken = header.GetValues(ApiKey).First();

            if (db.Tbl_ApiToken.Any(a => a.Token == apiToken) == false)
                return Json(new ApiResult { IsSuccess = false, StatusCode = 400, Message = "توکن معتبر نمی باشد" });


            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verificationDto.UserToken);
            if (qUser == null)
                return Json(new ApiResult { IsSuccess = true, StatusCode = 400, Message = "کابر یافت نشد" });

            bool VerifySejam = oSejamClass.VerifyUser(qUser.UserToken,verificationDto.VerificationCode, out string Message);

            if (VerifySejam == false)
                return Json(new ApiResult { IsSuccess = true, StatusCode = 400, Message = Message });
                
            // 3 = ثبت اطلاعات از سجام
            var qSms = db.Tbl_Sms.Find(3);
            var smsResult = oSms.SendSms(qUser.MobileNumber, qSms.Message);

            qUser.UserToken = Guid.NewGuid().ToString();
            qUser.SmsCode = 0;
            qUser.IsActive = true;

            var qApiToken = db.Tbl_ApiToken.FirstOrDefault( a=>a.Token == apiToken);

            var affiliate = new Tbl_Affiliate()
            {
                Token_Id = qApiToken.ID,
                User_Id = qUser.UserID,
            };

            db.Tbl_Affiliate.Add(affiliate);
            db.SaveChanges();

            // 2 = ثبت نام
            var qSms2 = db.Tbl_Sms.Find(2);
            var smsResult2 = oSms.SendSms(qUser.MobileNumber, qSms.Message);

            return Json(new ApiResult { IsSuccess = true, StatusCode = 201, Message = "عملیات با موفقیت انجام شد" });
        }
    }
}