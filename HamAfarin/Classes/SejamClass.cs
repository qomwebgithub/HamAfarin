using Common;
using DataLayer;
using HamAfarin.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ViewModels;

namespace HamAfarin
{
    public class SejamClass
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();

        public SejamClass()
        {

        }

        // در صورت غیر فعال شدن اکشن تست ای پی آی در کنترل اکانت این متد پرایوت شود
        public async Task<(bool Success, string Message)> kycOtpHttpClientAsync(string uniqueIdentifier)
        {
            // string uniqueIdentifier = "0022168842";
            bool boolToken = GetToken(out string token);
            (bool Success, string Message) tokenResult;
            if (boolToken)
            {
                //روش دوم
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.sejam.ir:8080/v1.1/");

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    StringContent json = new StringContent("{\"uniqueIdentifier\": \"" + uniqueIdentifier + "\"}", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("kycOtp", json);
                    if (response.IsSuccessStatusCode)
                    {
                        //return "Message: " + response.RequestMessage + " Header: " + response.Content.Headers + " content: " + responseContent;

                        string responseContent = await response.Content.ReadAsStringAsync();
                        tokenResult = (true, responseContent);
                        return tokenResult;
                    }
                    else
                    {
                        //return "Message: " + (int)response.StatusCode + " reason: " + response.ReasonPhrase + " content: " + response.Content.ReadAsStringAsync() + " json: " + json.ToString();
                        string responseContent = await response.Content.ReadAsStringAsync();
                        string errorMessage = SejamExceptionMessageHttpClient(responseContent);
                        tokenResult = (false, errorMessage);
                        return tokenResult;
                    }
                }
            }
            tokenResult = (false, "Token Error");
            return tokenResult;
        }

        /// <summary>
        /// چک کردن تکراری نبودن کد ملی
        /// یا کد ملی متعلق به کاربر جاری باشد
        /// </summary>
        /// <param name="uniqueIdentifier">کد ملی</param>
        /// <param name="UserID">ایدی کاربر</param>
        /// <returns></returns>
        public bool CheckNationalCode(string uniqueIdentifier, int UserID)
        {
            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserName == uniqueIdentifier);
            if (qUser != null)
            {
                if (qUser.UserID != UserID)
                {
                    return false;
                }
            }
            return true;
        }

        public bool LoginUser(string uniqueIdentifier, int UserID, out string UserToken)
        {
            UserToken = "";
            try
            {
                if (string.IsNullOrEmpty(uniqueIdentifier))
                {
                    return false;
                }
                Tbl_Users oUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == UserID);
                if (oUser != null)
                {
                    UserToken = oUser.UserToken;
                    if (kycOtp(uniqueIdentifier, out string Message))
                    {
                        oUser.HasSejam = true;
                        db.SaveChanges();

                        // کدملی کاربر را در تیبل دیگری ذخیره میکنیم
                        // اگر ابتدا در خوده تیبل کاربر ذخیره کنیم ممکن است اشتباه وارد کرده باشد و کد ملی شخص دیگری برای این کاربر ثبت شود
                        // اگر اس ام اس را درست وارد کرد ان موقع کد ملی را در تیبل کاربر ذخیره میکنیم
                        Tbl_SejamTempNationalCode qTempNationalCode = db.Tbl_SejamTempNationalCode.FirstOrDefault(s => s.NationalCode == uniqueIdentifier);
                        if (qTempNationalCode != null)
                        {
                            qTempNationalCode.IsActive = false;
                            db.SaveChanges();
                        }

                        Tbl_SejamTempNationalCode tempNationalCode = new Tbl_SejamTempNationalCode()
                        {
                            CreteDate = DateTime.Now,
                            ID = Guid.NewGuid().ToString(),
                            MobileNumber = oUser.MobileNumber,
                            NationalCode = uniqueIdentifier,
                            IsActive = true,
                            User_id = oUser.UserID
                        };
                        db.Tbl_SejamTempNationalCode.Add(tempNationalCode);
                        db.SaveChanges();
                        return true;
                    }

                    UserToken = Message;
                    return false;
                }
                UserToken = "کاربر یافت نشد";
            }
            catch (Exception e)
            {
                Tbl_SejamException oSejamException = new Tbl_SejamException()
                {
                    CreateDate = DateTime.Now,
                    Description = "uniqueIdentifier: " + uniqueIdentifier + " ",
                    Exception = e.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Method = "VerifyUser"
                };
                db.Tbl_SejamException.Add(oSejamException);
                db.SaveChanges();
                UserToken = e.ToString();
                return false;
            }
            return false;
        }

        public async Task<(bool Success, string UserToken)> LoginUserAsync(string uniqueIdentifier, int UserID)
        {
            (bool Success, string UserToken) loginResult;
            try
            {
                if (string.IsNullOrEmpty(uniqueIdentifier))
                {
                    loginResult = (false, "Empty");
                    return loginResult;
                }
                Tbl_Users oUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == UserID);
                if (oUser != null)
                {
                    loginResult = (true, oUser.UserToken);

                    (bool Success, string Message) kycOtpResult = await kycOtpHttpClientAsync(uniqueIdentifier);
                    if (kycOtpResult.Success)
                    {
                        oUser.HasSejam = true;
                        db.SaveChanges();

                        // کدملی کاربر را در تیبل دیگری ذخیره میکنیم
                        // اگر ابتدا در خوده تیبل کاربر ذخیره کنیم ممکن است اشتباه وارد کرده باشد و کد ملی شخص دیگری برای این کاربر ثبت شود
                        // اگر اس ام اس را درست وارد کرد ان موقع کد ملی را در تیبل کاربر ذخیره میکنیم
                        Tbl_SejamTempNationalCode qTempNationalCode = db.Tbl_SejamTempNationalCode.FirstOrDefault(s => s.NationalCode == uniqueIdentifier);
                        if (qTempNationalCode != null)
                        {
                            qTempNationalCode.IsActive = false;
                            db.SaveChanges();
                        }

                        Tbl_SejamTempNationalCode tempNationalCode = new Tbl_SejamTempNationalCode()
                        {
                            CreteDate = DateTime.Now,
                            ID = Guid.NewGuid().ToString(),
                            MobileNumber = oUser.MobileNumber,
                            NationalCode = uniqueIdentifier,
                            IsActive = true,
                            User_id = oUser.UserID
                        };
                        db.Tbl_SejamTempNationalCode.Add(tempNationalCode);
                        db.SaveChanges();
                        return loginResult;
                    }
                    loginResult = (false, kycOtpResult.Message);
                    return loginResult;
                }
                loginResult = (false, "کاربر یافت نشد");
                return loginResult;
            }
            catch (Exception e)
            {
                Tbl_SejamException oSejamException = new Tbl_SejamException()
                {
                    CreateDate = DateTime.Now,
                    Description = "uniqueIdentifier: " + uniqueIdentifier + " ",
                    Exception = e.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Method = "VerifyUser"
                };
                db.Tbl_SejamException.Add(oSejamException);
                db.SaveChanges();
                loginResult = (false, e.ToString());
                return loginResult;
            }

        }



        /// <summary>
        /// تایید کد ملی و ارسال اس ام اس
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        private bool kycOtp(string uniqueIdentifier, out string Message)
        {
            Message = "";
            bool boolToken = GetToken(out string token);
            if (boolToken)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.sejam.ir:8080/v1.1/kycOtp");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"uniqueIdentifier\": \"" + uniqueIdentifier + "\"}";
                    streamWriter.Write(json);
                }

                try
                {
                    // Get the response.
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                        }

                    }
                }
                catch (Exception e)
                {
                    Tbl_SejamException oSejamException = new Tbl_SejamException()
                    {
                        CreateDate = DateTime.Now,
                        Description = "uniqueIdentifier: " + uniqueIdentifier + " ",
                        Exception = e.ToString(),
                        ID = Guid.NewGuid().ToString(),
                        Method = "kycOtp"
                    };
                    db.Tbl_SejamException.Add(oSejamException);
                    db.SaveChanges();

                    Message = SejamExceptionMessage(e.ToString());
                    return false;
                }

                return true;
            }

            return false;
        }

        public bool VerifyUser(VerifySmsViewModel verifySms, out string Message)
        {
            Message = "";
            bool boolToken = GetToken(out string token);
            if (boolToken)
            {
                verifySms.SmsCode = StringExtensions.Fa2En(verifySms.SmsCode);
                Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserToken == verifySms.UserToken);
                if (qUser != null)
                {
                    // کدملی کاربر را در تیبل دیگری ذخیره میکنیم
                    // اگر ابتدا در خوده تیبل کاربر ذخیره کنیم ممکن است اشتباه وارد کرده باشد و کد ملی شخص دیگری برای این کاربر ثبت شود
                    // اگر اس ام اس را درست وارد کرد ان موقع کد ملی را در تیبل کاربر ذخیره میکنیم
                    Tbl_SejamTempNationalCode qSejamTemp = db.Tbl_SejamTempNationalCode.FirstOrDefault(s => s.User_id == qUser.UserID && s.IsActive);
                    if (qSejamTemp == null)
                    {
                        Message = "کد ملی یافت نشد";
                        return false;
                    }
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.sejam.ir:8080/v1.1/servicesWithOtp/profiles/" + qSejamTemp.NationalCode + "?otp=" + verifySms.SmsCode);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "Get";
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

                    try
                    {
                        // Get the response.
                        using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                        {
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                                bool getProfile = GetProfile(qUser, result, out string NationalCode);
                                if (getProfile)
                                {
                                    qUser.UserName = NationalCode;
                                    db.SaveChanges();
                                    return true;
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Tbl_SejamException oSejamException = new Tbl_SejamException()
                        {
                            CreateDate = DateTime.Now,
                            Description = "uniqueIdentifier: " + qUser.UserName + " ",
                            Exception = e.ToString(),
                            ID = Guid.NewGuid().ToString(),
                            Method = "VerifyUser"
                        };
                        db.Tbl_SejamException.Add(oSejamException);
                        db.SaveChanges();
                        Message = SejamExceptionMessage(e.ToString());
                        return false;
                    }
                }
            }
            return false;
        }


        private bool GetProfile(Tbl_Users oUser, string Profile, out string NationalCode)
        {
            NationalCode = "";
            try
            {
                BasePerson oBasePerson = new BasePerson(Profile.ToString());
                if (string.IsNullOrEmpty(oUser.MobileNumber))
                {
                    oUser.MobileNumber = oBasePerson.mobile;
                }
                oUser.UserStatus = oBasePerson.status;
                oUser.UserType = oBasePerson.type;
                Tbl_UserProfiles oUserProfiles = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == oUser.UserID);
                // DateTime DateObject = DateTime.Parse("11/22/1994 00:00:00");

                if (oUserProfiles != null)
                {
                    oUserProfiles.NationalCode = oBasePerson.uniqueIdentifier;
                    oUserProfiles.FirstName = oBasePerson.privatePerson.firstName;
                    oUserProfiles.LastName = oBasePerson.privatePerson.lastName;
                    oUserProfiles.FatherName = oBasePerson.privatePerson.fatherName;
                    oUserProfiles.BirthDate = StringToDate(oBasePerson.privatePerson.birthDate);
                    oUserProfiles.Email = oBasePerson.email;
                    oUserProfiles.Gender = oBasePerson.privatePerson.gender;
                    oUserProfiles.User_id = oUser.UserID;
                    oUserProfiles.strSejamResponse = Profile;
                    oUserProfiles.IsActive = true;
                    oUserProfiles.IsDeleted = false;
                    //  oUserProfiles.strBirthDate = oBasePerson.privatePerson.birthDate;
                    oUserProfiles.MobileNumber = oBasePerson.mobile;
                    oUserProfiles.ProfileNationalId = oBasePerson.privatePerson.shNumber;
                    oUserProfiles.AccountSheba = oBasePerson.accounts.sheba;
                    oUserProfiles.SejamCode = oBasePerson.tradingCodes.code;
                }
                else
                {
                    oUserProfiles = new Tbl_UserProfiles()
                    {
                        NationalCode = oBasePerson.uniqueIdentifier,
                        FirstName = oBasePerson.privatePerson.firstName,
                        LastName = oBasePerson.privatePerson.lastName,
                        FatherName = oBasePerson.privatePerson.fatherName,
                        BirthDate = StringToDate(oBasePerson.privatePerson.birthDate),
                        Email = oBasePerson.email,
                        Gender = oBasePerson.privatePerson.gender,
                        User_id = oUser.UserID,
                        strSejamResponse = Profile,
                        CreateDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        // strBirthDate = oBasePerson.privatePerson.birthDate,
                        MobileNumber = oBasePerson.mobile,
                        ProfileNationalId = oBasePerson.privatePerson.shNumber,
                        AccountSheba = oBasePerson.accounts.sheba,
                        SejamCode = oBasePerson.tradingCodes.code
                    };
                    db.Tbl_UserProfiles.Add(oUserProfiles);
                }

                db.SaveChanges();
                NationalCode = oBasePerson.uniqueIdentifier;
                return true;
            }
            catch (Exception e)
            {

                Tbl_SejamException oSejamException = new Tbl_SejamException()
                {
                    CreateDate = DateTime.Now,
                    Description = "uniqueIdentifier: " + oUser.UserName + " ",
                    Exception = e.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Method = "VerifyUser"
                };
                db.Tbl_SejamException.Add(oSejamException);
                db.SaveChanges();
                return false;
            }
            return false;
        }

        private DateTime StringToDate(string strBirthDate)
        {
            strBirthDate = strBirthDate.Split(' ')[0];
            strBirthDate = strBirthDate.Split('/')[2] + "/" + strBirthDate.Split('/')[0] + "/" + strBirthDate.Split('/')[1];

            //int year = int.Parse(strBirthDate.Substring(0, 4));
            //int month = int.Parse(strBirthDate.Substring(5, 2));
            //int day = int.Parse(strBirthDate.Substring(8, 2));
            //System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            //DateTime dt = p.ToDateTime(year, month, day, 0, 0, 0, 0);
            //strBirthDate = dt.ToShortDateString();
            //DateTime date = DateTime.Parse(strBirthDate);
            //return date;

            string[] DigitArray = new string[3];
            DigitArray = strBirthDate.Split('/');
            DateTime ObjDateTime = new DateTime(int.Parse(DigitArray[0]), int.Parse(DigitArray[1])
                , int.Parse(DigitArray[2]));
            return ObjDateTime;
        }

        private bool GetToken(out string token)
        {
            token = "";

            try
            {


                //از این روش به علت ارسال دو درخواست تا زمان رفع باگ استفاده نمی شود
                //Tbl_SajamToken qSajamToken = db.Tbl_SajamToken.FirstOrDefault(t => t.IsActive && t.IsDelete == false);

                //if (qSajamToken != null)
                //{
                //    if (qSajamToken.DateExpire > DateTime.Now)
                //    {
                //        token = qSajamToken.Token;
                //        return true;
                //    }

                //    qSajamToken.IsActive = false;
                //    db.Entry(qSajamToken).State = EntityState.Modified;
                //    db.SaveChanges();
                //}

                //روش دوم: پاک کردن تمام اکتیو ها
                List<Tbl_SajamToken> qSajamTokens = db.Tbl_SajamToken.Where(t => t.IsActive && t.IsDelete == false).ToList();
                if (qSajamTokens != null && qSajamTokens.Count() > 0)
                {

                    if (qSajamTokens[0].DateExpire > DateTime.Now)
                    {
                        token = qSajamTokens[0].Token;
                        return true;
                    }
                    foreach (var qSajamToken in qSajamTokens)
                    {
                        qSajamToken.IsActive = false;
                    }
                    db.SaveChanges();
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.sejam.ir:8080/v1.1/accessToken");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"username\":\"920\"," +
                                  "\"password\":\"A@5x3hp4y\"}";

                    streamWriter.Write(json);
                }


                try
                {
                    // Get the response.
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        GetToken oGetToken = new GetToken(result.ToString());
                        token = oGetToken.accessToken.ToString();
                        using (HamAfarinDBEntities hdb = new HamAfarinDBEntities())
                        {
                            Tbl_SajamToken oSajamToken = new Tbl_SajamToken()
                            {
                                DateExpire = DateTime.Now.AddMinutes(SejamTtlStringConvertToInt(oGetToken.ttl)),
                                DateRequest = DateTime.Now,
                                ErrorMessage = "",
                                IsActive = true,
                                IsDelete = false,
                                Token = token,
                                TTL = oGetToken.ttl
                            };
                            hdb.Tbl_SajamToken.Add(oSajamToken);
                            hdb.SaveChanges();
                        }

                    }
                    httpResponse.Close();
                }
                catch (Exception e)
                {
                    token = e.Message;

                    Tbl_SajamToken oExSajamToken = new Tbl_SajamToken()
                    {
                        DateExpire = DateTime.Now,
                        DateRequest = DateTime.Now,
                        ErrorMessage = e.Message,
                        IsActive = false,
                        IsDelete = false,
                        Token = "",
                        TTL = ""
                    };
                    db.Tbl_SajamToken.Add(oExSajamToken);
                    db.SaveChanges();

                    return false;
                }
                return true;

            }
            catch (Exception e)
            {
                token = e.ToString();
                Tbl_SejamException oSejamException = new Tbl_SejamException()
                {
                    CreateDate = DateTime.Now,
                    Description = "",
                    Exception = e.ToString(),
                    ID = Guid.NewGuid().ToString(),
                    Method = "GetToken"
                };
                db.Tbl_SejamException.Add(oSejamException);
                db.SaveChanges();
                return false;
            }
        }

        private int SejamTtlStringConvertToInt(string ttl)
        {
            string[] lstTtl = ttl.Split(':');

            int ttlInMinutes = (int.Parse(lstTtl[0]) * 60) + (int.Parse(lstTtl[1])) - 2;

            return ttlInMinutes;

        }
        public string SejamExceptionMessage(string exception)
        {
            if (exception.Contains("error: (400)"))
            {
                return "خطا در ارتباط با سرور سجام";
            }
            else if (exception.Contains("error: (401)"))
            {
                return "کاربر اعتبارسنجی نشد";
            }
            else if (exception.Contains("error: (403)"))
            {
                return "خطا عدم دسترسی";
            }
            else if (exception.Contains("error: (404)"))
            {
                return "کاربر موردنظر یافت نشد";
            }
            else if (exception.Contains("error: (429)"))
            {
                return "تعداد درخواست بیش از حد مجاز";
            }
            else
            {
                return "خطای نامشخص";
            }
        }
        public string SejamExceptionMessageHttpClient(string exception)
        {
            if (exception.Contains("\"errorCode\":4001"))
            {
                return "شماره موبایل اشتباه است";
            }
            else if (exception.Contains("\"errorCode\":4002"))
            {
                return "زمان اعتبار کد تأیید به پایان رسیده است";
            }
            else if (exception.Contains("\"errorCode\":4004"))
            {
                return "ثبت نام تکمیل نشده است";
            }
            else if (exception.Contains("\"errorCode\":4005"))
            {
                return "کدپیگیری اشتباه است";
            }
            else if (exception.Contains("\"errorCode\":4008"))
            {
                return "حجم تصویر زیاد است";
            }
            else if (exception.Contains("\"errorCode\":4010"))
            {
                return "کاربر اعتبارسنجی نشد";
            }
            else if (exception.Contains("\"errorCode\":4011"))
            {
                return "LockedUser";
            }
            else if (exception.Contains("\"errorCode\":4031"))
            {
                return "کد تأیید نامعتبر است";
            }
            else if (exception.Contains("\"errorCode\":4041"))
            {
                return "کاربر مورد نظر یافت نشد";
            }
            else if (exception.Contains("\"errorCode\":4042"))
            {
                return "سرویس تعریف نشده است";
            }
            else if (exception.Contains("\"errorCode\":4043"))
            {
                return "ThirdPartyServiceNotFound";
            }
            else if (exception.Contains("\"errorCode\":4090"))
            {
                return "Conflicts";
            }
            else if (exception.Contains("\"errorCode\":4150"))
            {
                return "نوع فایل پشتیبانی نمی شود";
            }
            else if (exception.Contains("\"errorCode\":4290"))
            {
                return "تعداد درخواست سرویس بیش از حد مجاز";
            }
            else if (exception.Contains("\"errorCode\":400"))
            {
                return "BadRequest";
            }
            else if (exception.Contains("\"errorCode\":403"))
            {
                return "Forbidden";
            }
            else if (exception.Contains("\"errorCode\":500"))
            {
                return "خطای سرور";
            }
            else if (exception.Contains("\"errorCode\":600"))
            {
                return "عدم ارسال اس ام اس";
            }
            else if (exception.Contains("\"errorCode\":601"))
            {
                return "پرداخت ناموفق";
            }
            else
            {
                return "خطای نامشخص";
            }
        }

    }
}