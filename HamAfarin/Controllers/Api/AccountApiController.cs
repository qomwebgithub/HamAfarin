using Common;
using DataLayer;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
        SejamClass oSejamClass = new SejamClass();
        CheckNationalCode checkNationalCode = new CheckNationalCode();
        CheckPassword checkPassword = new CheckPassword();
        private readonly string ApiKey = "ApiKey";

        [HttpPost]
        [Route("api/v1/register")]
        public IHttpActionResult Register([FromBody] ApiRegisterDto register)
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

            Random rndSmsCode = new Random();
            int smsCode = rndSmsCode.Next(1000, 9999);
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
                SmsCode = smsCode,
                IsLegal = false,
                UserToken = Guid.NewGuid().ToString(),
                HasSejam = false,
                ActivateDate = null,
            };
            db.Tbl_Users.Add(oUser);

            db.SaveChanges();

            oSms.SendSms(oUser.MobileNumber, oUser.SmsCode.ToString());
            return Json(new ApiResult<string> { Data= oUser.UserToken, IsSuccess = true, StatusCode = 201, Message = "عملیات با موفقیت انجام شد" });
        }

    }
}