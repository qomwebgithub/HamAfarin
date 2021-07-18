using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using HamAfarin;
using KooyWebApp_MVC.Classes;
using ViewModels;
using System.Text;
using System.Net;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;

namespace Hamafarin.Controllers
{
    public class PaymentController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        PlanService planService = new PlanService();
        UserService userService = new UserService();
        ShaparakMessageEncoding ShaparakMessage = new ShaparakMessageEncoding();
        SMS oSms = new SMS();


        // GET: Payment
        [Authorize]
        public ActionResult SelectPaymentType(int id)
        {
            //  int qMyUserId = db.Tbl_Users.Find(userService.GetUserIdByUserName(User.Identity.Name)).UserID;
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles qUserProfiles = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == UserID);
            if (qUserProfiles != null && qUserProfiles.IsActive)
            {
                Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == id);

                ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                ViewBag.UserProfileIsActive = true;

                SelectPaymentTypeViewModel selectPayment = new SelectPaymentTypeViewModel();
                selectPayment.BusinessPlanID = qBussinessPlans.BussinessPlanID;
                selectPayment.CompanyName = qBussinessPlans.CompanyName;
                selectPayment.BussinessName = qBussinessPlans.Title;
                selectPayment.FinancialDuration_id = qBussinessPlans.Tbl_BussinessPlan_FinancialDuration.FinancialDurationTitle;
                selectPayment.CodeOTC = qBussinessPlans.CodeOTC;
                selectPayment.ImageNameWarranty = qBussinessPlans.ImageNameWarranty;
                selectPayment.PercentageReturnInvestment = qBussinessPlans.PercentageReturnInvestment;
                selectPayment.PercentageReturnInvestment = qBussinessPlans.PercentageReturnInvestment;
                selectPayment.AmountRequiredRoRaiseCapital = qBussinessPlans.AmountRequiredRoRaiseCapital;
                int myUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                Tbl_UserProfiles tbl_UserProfiles = db.Tbl_UserProfiles.FirstOrDefault(u => u.User_id == myUserId);
                selectPayment.InvestorFullName = tbl_UserProfiles.FirstName + " " + tbl_UserProfiles.LastName;
                selectPayment.InvestorSejamId = "";
                selectPayment.IsOnline = true;
                selectPayment.SiteTermsConditions = db.Tbl_Settings.FirstOrDefault().SiteTermsConditions;
                selectPayment.Privacy = db.Tbl_Settings.FirstOrDefault().Privacy;
                selectPayment.InvestorMobile = tbl_UserProfiles.MobileNumber;
                selectPayment.InvestorNationalCode = tbl_UserProfiles.NationalCode;
                // کل سرمایه گذاری شما
                selectPayment.TotalInvestment = planService.GetInvsetmentUserOfPlan(db, id, myUserId);
                // حداکثر امکان سرمایه گذاری شما
                selectPayment.MinimumAmountInvest = Convert.ToInt64(qBussinessPlans.MinimumAmountInvest);
                // حداقل مبلغ برای سرمایه گذاری
                selectPayment.MinimumAmountInvest = Convert.ToInt64(qBussinessPlans.MinimumAmountInvest);
                // حداکثر مبلغ برای سرمایه گذاری
                selectPayment.MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                // حداکثر امکان سرمایه گذاری شما
                selectPayment.CanInvestment = selectPayment.MaximumInvestment - selectPayment.TotalInvestment;
                return View(selectPayment);

            }
            else
            {
                if (qUserProfiles == null)
                    ViewBag.UserProfileIsActive = null;
                else
                    ViewBag.UserProfileIsActive = false;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SelectPaymentType(SelectPaymentTypeViewModel selectPaymentTypeViewModel, HttpPostedFileBase imgPaymentImageNameUploaded)
        {

            ViewBag.UserProfileIsActive = true;
            if (selectPaymentTypeViewModel.IsOnline)
            {
                ModelState.Remove("OfflinePaymentPrice");
                ModelState.Remove("PaymentImageName");
                ModelState.Remove("TransactionPaymentCode");

                if (ModelState.IsValid)
                {
                    Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID);

                    // حداقل مبلغ برای سرمایه گذاری
                    ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                    int MaximumInvestment = Convert.ToInt32((Convert.ToInt32(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                    // حداکثر مبلغ برای سرمایه گذاری
                    ViewBag.MaximumInvestment = MaximumInvestment;

                    PaymentPriceValidation paymentPriceValidation = planService.ValidationPaymentPrice(db, selectPaymentTypeViewModel.BusinessPlanID,
                        selectPaymentTypeViewModel.OnlinePaymentPrice, UserSetAuthCookie.GetUserID(User.Identity.Name));
                    if (paymentPriceValidation.Validation)
                    {
                        UserService userService = new UserService();
                        int intCurrentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                        Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == intCurrentUserId);

                        Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                        {
                            BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
                            IsPaid = false,
                            IsConfirmedFromAdmin = false,
                            PaidDateTime = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUser_id = intCurrentUserId,
                            PaymentUser_id = intCurrentUserId,
                            PaymentPrice = selectPaymentTypeViewModel.OnlinePaymentPrice,
                            PaymentType_id = 2,
                            AdminCheckDate = null,
                            IsDelete = false,
                            PaymentStatus = 2,
                            IsReturned = false,
                            PaymentImageName = null,
                            InvoiceNumber = Generator.InvoiceNumber()
                        };
                        db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                        db.SaveChanges();

                        Tbl_PaymentOnlineDetils tbl_PaymentOnline = new Tbl_PaymentOnlineDetils()
                        {
                            CreateDate = tbl_BusinessPlanPayment.CreateDate,
                            FinallyDate = null,
                            IsDelete = false,
                            IsFinally = false,
                            PaymentDetilsID = Guid.NewGuid().ToString(),
                            Payment_id = tbl_BusinessPlanPayment.PaymentID,
                        };
                        db.Tbl_PaymentOnlineDetils.Add(tbl_PaymentOnline);
                        decimal dclAmount = Convert.ToDecimal(tbl_BusinessPlanPayment.PaymentPrice) * 10;
                        // //int merchentCode = 4650168;  //کد پذیرنده
                        // //int terminalCode = 1837060; //کد ترمینال  
                        // string strInvoiceNumber = "\"" + tbl_BusinessPlanPayment.InvoiceNumber + "\"";
                        // string strTimestamp = "\"" + tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") + "\"";
                        // string strUserMobileNumber = "\"" + qUser.MobileNumber + "\"";
                        // //string strTimestamp = "\"2021/03/08 23:35:13\"";
                        // // { "InvoiceNumber": 123, "InvoiceDate":"1349/04/04","TerminalCode": "1837060", "MerchantCode": "4650168", "Amount":"1000","RedirectAddress":" https://www.sample.com/PaymentResult","Timestamp":2021/03/08 23:35:13, "Action":"1003","Mobile":"09122222222", "Email":"BuyerName@Sample.ir" }

                        // // var sendingData = "{ \"InvoiceNumber\": \"4650168\", \"InvoiceDate\":\"4650168\",\"TerminalCode\": \"1837060\", \"MerchantCode\": \"4650168\", \"Amount\":\"10000\",\"RedirectAddress\":\" https://www.hamafarin.ir/PaymentResult\",\"Timestamp\":\"2021/03/08 23:10:13\", \"Action\":\"1003\",\"Mobile\":\"09197246889\", \"Email\":\"ahmad@gmail.com\" }";
                        // var sendingData = "{ \"InvoiceNumber\": " + strInvoiceNumber + ", \"InvoiceDate\":" + strTimestamp + ",\"TerminalCode\": \"1837060\", \"MerchantCode\": \"4650168\", \"Amount\":" + dclAmount + ",\"RedirectAddress\":\" http://localhost/Payment/VerifyPayment\",\"Timestamp\":" + strTimestamp + ", \"Action\":\"1003\",\"Mobile\":" + strUserMobileNumber + ", \"Email\":\"hamafarin@Sample.ir\" }";

                        // var content = new StringContent(sendingData, Encoding.UTF8, "application/json");
                        // var request = new HttpRequestMessage
                        // {
                        //     RequestUri = new Uri("https://pep.shaparak.ir/Api/v1/Payment/GetToken"),
                        //     Method = HttpMethod.Post,
                        //     Content = content
                        // };
                        // request.Headers.Add("Sign", GetSign(sendingData));
                        // var client = new HttpClient();
                        // client.DefaultRequestHeaders.Accept.Add(new
                        //MediaTypeWithQualityHeaderValue("application/json"));
                        // var response = client.SendAsync(request).Result;
                        // string strTestResault = Encoding.UTF8.GetString(response.Content.ReadAsByteArrayAsync().Result);
                        // //  {"Token":"AhVuYaudIyef/TBPTOXNve+6SIQ0gn9CKVl8r7lg4IY=","IsSuccess":true,"Message":"عمليات با موفقيت انجام شد"} 
                        // ViewBag.RedirectShaparak = strTestResault;

                        // string strToken = "";
                        // string strMessage = "";
                        // tbl_BusinessPlanPayment.ShaparakIsSuccess = ShaparakMessage.Encode(strTestResault, out strToken, out strMessage);
                        // tbl_BusinessPlanPayment.ShaparakToken = strToken;
                        // tbl_BusinessPlanPayment.ShaparakMessage = strMessage;
                        // db.SaveChanges();
                        // Response.Redirect("https://pep.shaparak.ir/payment.aspx?n=" + strToken);
                        // return View(selectPaymentTypeViewModel);

                        string token = "";
                        string timeStamp = tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss");
                        string amount = dclAmount.ToString();
                        string invoiceNumber = tbl_BusinessPlanPayment.InvoiceNumber;
                        string ActionResult = "1003";
                        string invoiceDate = tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd");
                        AppSettingsReader appSetting = new AppSettingsReader();
                        string merchantCode = "4650168";  //کد پذیرنده
                        string terminalCode = "1837060"; //کد ترمینال  
                                                         //  string redirectAddress = appSetting.GetValue("RedirectAddress", typeof(string)).ToString();
                        string redirectAddress = "https://www.hamafarin.ir/Payment/VerifyPayment/" + tbl_PaymentOnline.PaymentDetilsID;

                        DataPost dp = new DataPost();
                        dp.InvoiceNumber = invoiceNumber;
                        dp.InvoiceDate = invoiceDate;
                        dp.MerchantCode = merchantCode;
                        dp.TerminalCode = terminalCode;
                        dp.Amount = amount;
                        dp.RedirectAddress = redirectAddress;
                        dp.Action = ActionResult;
                        dp.TimeStamp = timeStamp;
                        string output = JsonConvert.SerializeObject(dp);
                        string sign = GetSign(output);

                        byte[] textArray = Encoding.UTF8.GetBytes(output);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/GetToken");
                        request.Method = "POST";
                        request.ContentType = "Application/Json";
                        request.ContentLength = textArray.Length;
                        request.Headers.Add("Sign", sign);

                        request.GetRequestStream().Write(textArray, 0, textArray.Length);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string result = reader.ReadToEnd();
                        tbl_PaymentOnline.ShaparakMessageGetToken = result;

                        if (result.Contains("Token"))
                        {
                            var res = result.Split(':', ',');

                            // Session.Add("Token", res[1]);
                            token = res[1];

                            token = token.Replace("\"", "");

                            tbl_PaymentOnline.ShaparakToken = token;
                            db.SaveChanges();

                            Response.Redirect("https://pep.shaparak.ir/payment.aspx?n=" + token);
                            return View();
                        }

                        db.SaveChanges();


                        return View(selectPaymentTypeViewModel);

                        // return RedirectToAction("Shaparak", new { strTestResault = token });

                        //     return RedirectToAction("Shaparak", new { strTestResault = strTestResault });

                        //System.Net.ServicePointManager.Expect100Continue = false;
                        //HamAfarin.ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp =
                        //    new HamAfarin.ZarinPal.PaymentGatewayImplementationServicePortTypeClient();

                        //string Authority;
                        //int status = zp.PaymentRequest("code", selectPaymentTypeViewModel.OnlinePaymentPrice.Value, "test portal",
                        //    "email", "mobile", ConfigurationManager.AppSettings["ThisDomain"] + "/Payment/VerifyPayment/" + tbl_BusinessPlanPayment.PaymentID, out Authority);
                        //if (status == 100)
                        //{
                        //    Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                        //}
                        //else
                        //{
                        //    ViewBag.Error = "Error : " + status;
                        //}
                        //ViewBag.IsPaymentSuccess = true;
                        //return View("OnlinePayment");
                    }
                    else
                    {
                        // سرمایه گذاری من در این طرح
                        ViewBag.TotalMyInvestmentInPlan = planService.GetInvsetmentUserOfPlan(db, selectPaymentTypeViewModel.BusinessPlanID
                            , UserSetAuthCookie.GetUserID(User.Identity.Name));
                        ModelState.AddModelError("OnlinePaymentPrice", paymentPriceValidation.Error.ToString());
                        return View(selectPaymentTypeViewModel);
                    }
                }
            }
            else
            {
                ModelState.Remove("OnlinePaymentPrice");
                if (ModelState.IsValid)
                {
                    Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID);

                    // حداقل مبلغ برای سرمایه گذاری
                    ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                    int MaximumInvestment = Convert.ToInt32((Convert.ToInt32(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                    // حداکثر مبلغ برای سرمایه گذاری
                    ViewBag.MaximumInvestment = MaximumInvestment;

                    PaymentPriceValidation paymentPriceValidation = planService.ValidationPaymentPrice(db, selectPaymentTypeViewModel.BusinessPlanID, selectPaymentTypeViewModel.OfflinePaymentPrice
                        , UserSetAuthCookie.GetUserID(User.Identity.Name));

                    if (paymentPriceValidation.Validation)
                    {
                        if (imgPaymentImageNameUploaded != null && imgPaymentImageNameUploaded.IsImage())
                        {
                            selectPaymentTypeViewModel.PaymentImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgPaymentImageNameUploaded.FileName);
                            imgPaymentImageNameUploaded.SaveAs(Server.MapPath("/Images/PaymentImages/" + selectPaymentTypeViewModel.PaymentImageName));

                        }
                        else
                        {
                            ModelState.AddModelError("PaymentImageName", "رسید بانکی را انتخاب کنید");
                            return View(selectPaymentTypeViewModel);
                        }
                        UserService userService = new UserService();
                        int intCurrentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);

                        Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                        {
                            BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
                            IsPaid = false,
                            IsConfirmedFromAdmin = false,
                            PaidDateTime = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUser_id = intCurrentUserId,
                            PaymentUser_id = intCurrentUserId,
                            TransactionPaymentCode = selectPaymentTypeViewModel.TransactionPaymentCode,
                            PaymentPrice = selectPaymentTypeViewModel.OfflinePaymentPrice,
                            PaymentType_id = 3,
                            PaymentImageName = selectPaymentTypeViewModel.PaymentImageName,
                            AdminCheckDate = null,
                            IsDelete = false,
                            PaymentStatus = 2
                        };
                        db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                        db.SaveChanges();
                        ViewBag.IsPaymentSuccess = true;
                        return View("OfflinePayment");
                    }
                    else
                    {
                        // سرمایه گذاری من در این طرح
                        ViewBag.TotalMyInvestmentInPlan = planService.GetInvsetmentUserOfPlan(db, selectPaymentTypeViewModel.BusinessPlanID
                            , UserSetAuthCookie.GetUserID(User.Identity.Name));
                        ModelState.AddModelError("OfflinePaymentPrice", paymentPriceValidation.Error.ToString());
                    }
                    if (imgPaymentImageNameUploaded != null) selectPaymentTypeViewModel.PaymentImageName = imgPaymentImageNameUploaded.FileName;
                    return View(selectPaymentTypeViewModel);
                }
            }
            return View("SelectPaymentType/id=" + selectPaymentTypeViewModel);
        }

        public ActionResult Shaparak(string strTestResault)
        {
            //ViewBag.RedirectShaparak = strTestResault;
            //string strToken = "";
            //string strMessage = "";
            //bool IsSuccess = ShaparakMessage.Encode(strTestResault, out strToken, out strMessage);
            ViewBag.Token = strTestResault;
            ViewBag.Message = strTestResault;
            return View();
        }

        public string GetSign(string data)
        {
            var cs = new CspParameters { KeyContainerName = "PaymentTest" };
            var rsa = new RSACryptoServiceProvider(cs) { PersistKeyInCsp = false };
            rsa.Clear();
            rsa = new RSACryptoServiceProvider();
            //rsa.FromXmlString("<RSAKeyValue><Modulus>2vW5l4pF6e2xBe4L0pq197o2/Xm6A4ktgYme+S/7SAQglFYb9swyrXzeeZGM0FDziUuxPJpCmG85As35L3PBHc5D/cVWt7HyrgoygXguzIVFLQPcSkGlPhiu/9wXFl+wt579JloapjYIM99vzwsw1X8W5HMWO02snNR4Gh+KujE=</Modulus><Exponent>AQAB</Exponent><P>7TpcqQxQSnxQuGXg1gAExDjkLZRuWXMsMZ1HcGs9B659ZCtyx0K9PZeBb+rSbsDiQ53ME7gfr1qez5OJs8V3Kw==</P><Q>7ElMHADjw9SjcIoA9ZiEtdZNlesfuxImTJ3ZSgVJLHm1DzpddZizOiPMcoJB30p9DUybV0yA92Ks9vPON1emEw==</Q><DP>M5IXaKyDm92wkpWbLgps/tc7S2lUH9/4wIRnblnxt4TIP4lud0Db8NLJ0bGjs239AiQApFzHjpBNq+Rv8APCZw==</DP><DQ>t1v15PFHzorvPgDJ18xAh/9Ce+lW2UuvTttbaFaLvyi/OY74Qa56vs5S9DVius1KF4zwKyGiHteDE6yExWuN4w==</DQ><InverseQ>ps0bUjxFx2RxXgvA8nU+C+RSGVI3UgoUMD9L+asg2YrCSj11S1RQqu4Yh+bkIY3sb58/5VpzRZ1/DufdZ1/LFw==</InverseQ><D>Ohw2yioPTfsgHRPFHXqjyoAoMfNJU8DnS9arUzTRupYl01hbCY+9718RaDAhr/Ob/pcRDaVfATebaMwjUt2kpXe6R5TQ7dfShxNqbBgQtPBXvjuhoOtU6mFOWSyEGQUEsVsVSexvJyM/I1aXj9m9BHVSp0gp6+Jb5yYM/G2PnbU=</D></RSAKeyValue>");
            rsa.FromXmlString("<RSAKeyValue><Modulus>oPmmPFmRrBmLkwah/zqUpyjU3yv+lZtZ+ucJQsJkrrnZhZdvrNFknByXp8BTWbHy5oRAGbHZ7EjofxqV3ELYIkinlRhprvqrPgl1qkoIBOjY0VSjepCZc8g8UYNcyI+RwMWNdQrNLhDgnid5DMzt5Jd5TCmM47NZ7SiZw4cfr9k=</Modulus><Exponent>AQAB</Exponent><P>2YchjDV4ZgRAMX7UehtuLr6q7fysh0ayyy7tfzVzbGjYwfScvXrn+Dyv5QcfV27XS3nnSinEulZbxD7Y7D3M6Q==</P><Q>vXIFDT+2vRh37lXXCHz/zdMLTL81XDeXHayzq89ZGtHdRDbLEgS586BSBsv2rHu3faQkRUVpozNaQilYHYw1cQ==</Q><DP>aHNfo4ClYG8YF+wrwLpdLOr3DrGucoJ7Jd9RwSx8UQkDVBbOEJnsuyEA4PuLjCM4FBDkA5OM2GDRTalT5hRCyQ==</DP><DQ>HXhmRshZkhMDXSUnXV+sn4UsJdU85MqVDH24OYr47o4lIJ5xeq0XjEmtWPdTuUNJqRbmuYvxwfyUFuS9m2MbUQ==</DQ><InverseQ>U28ACYa+rqQ04rhKJUXuqyScEQbDrKaDKt9YsrHtWtAD3kiSwhx+qIWVRCNQYpbVwabVn0KbwvlyNW7jyuJprQ==</InverseQ><D>aN/c7sqH/nCyHzqeBhNbT8BpV3BpL30D1VP6E5VkVPVeAwwTRKsy+gYiw7aoVvJ5Rrd4r4TQboFdf5mJcvPVrQ81jmWF01hTUWlRa0vxpokPFlf9K9nzrYcLjjgwMlIYqzbeobI2Jozi2kF+MdbqJlgH2YHOwxpCSfksBL1FOYE=</D></RSAKeyValue>");
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            return sign;
        }



        [Authorize]
        public ActionResult OfflinePayment(int id = 0)
        {
            Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == id);

            ViewBag.BusinessPlanId = id;
            // حداقل مبلغ برای سرمایه گذاری
            ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
            int MaximumInvestment = Convert.ToInt32((Convert.ToInt32(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
            // حداکثر مبلغ برای سرمایه گذاری
            ViewBag.MaximumInvestment = MaximumInvestment;
            // سرمایه گذاری من در این طرح
            ViewBag.TotalMyInvestmentInPlan = planService.GetInvsetmentUserOfPlan(db, id, UserSetAuthCookie.GetUserID(User.Identity.Name));

            return View(new PaymentOfflineViewModel() { BusinessPlan_id = id });
        }
        [HttpPost]
        [Authorize]
        public ActionResult OfflinePayment(PaymentOfflineViewModel paymentOfflineViewModel, HttpPostedFileBase imgPaymentImageNameUploaded)
        {
            if (ModelState.IsValid)
            {
                Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == paymentOfflineViewModel.BusinessPlan_id);

                // حداقل مبلغ برای سرمایه گذاری
                ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                int MaximumInvestment = Convert.ToInt32((Convert.ToInt32(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                // حداکثر مبلغ برای سرمایه گذاری
                ViewBag.MaximumInvestment = MaximumInvestment;

                PaymentPriceValidation paymentPriceValidation = planService.ValidationPaymentPrice(db, paymentOfflineViewModel.BusinessPlan_id, paymentOfflineViewModel.PaymentPrice
                    , UserSetAuthCookie.GetUserID(User.Identity.Name));

                if (paymentPriceValidation.Validation)
                {
                    if (imgPaymentImageNameUploaded != null && imgPaymentImageNameUploaded.IsImage())
                    {
                        paymentOfflineViewModel.PaymentImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgPaymentImageNameUploaded.FileName);
                        imgPaymentImageNameUploaded.SaveAs(Server.MapPath("/Images/PaymentImages/" + paymentOfflineViewModel.PaymentImageName));

                    }
                    else
                    {
                        ModelState.AddModelError("PaymentImageName", "رسید بانکی را انتخاب کنید");
                        return View(paymentOfflineViewModel);
                    }
                    UserService userService = new UserService();
                    int intCurrentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);

                    Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                    {
                        BusinessPlan_id = paymentOfflineViewModel.BusinessPlan_id,
                        IsPaid = false,
                        IsConfirmedFromAdmin = false,
                        PaidDateTime = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser_id = intCurrentUserId,
                        PaymentUser_id = intCurrentUserId,
                        TransactionPaymentCode = paymentOfflineViewModel.TransactionPaymentCode,
                        PaymentPrice = paymentOfflineViewModel.PaymentPrice,
                        PaymentType_id = 3,
                        PaymentImageName = paymentOfflineViewModel.PaymentImageName,
                        IsDelete = false,
                        PaymentStatus = 2,
                        AdminCheckDate = null
                    };
                    db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                    db.SaveChanges();
                    ViewBag.IsPaymentSuccess = true;
                    return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { Area = "UserPanel", id = tbl_BusinessPlanPayment.PaymentID });
                }
                else
                {
                    // سرمایه گذاری من در این طرح
                    ViewBag.TotalMyInvestmentInPlan = planService.GetInvsetmentUserOfPlan(db, paymentOfflineViewModel.BusinessPlan_id.Value
                        , UserSetAuthCookie.GetUserID(User.Identity.Name));
                    ModelState.AddModelError("PaymentPrice", paymentPriceValidation.Error.ToString());
                }
                if (imgPaymentImageNameUploaded != null) paymentOfflineViewModel.PaymentImageName = imgPaymentImageNameUploaded.FileName;
            }
            return View(paymentOfflineViewModel);
        }

        [Authorize]
        public ActionResult OnlinePayment(int id = 0)
        {
            Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == id);

            ViewBag.BusinessPlanId = id;
            // حداقل مبلغ برای سرمایه گذاری
            ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
            int MaximumInvestment = Convert.ToInt32((Convert.ToInt32(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
            // حداکثر مبلغ برای سرمایه گذاری
            ViewBag.MaximumInvestment = MaximumInvestment;
            // سرمایه گذاری من در این طرح
            ViewBag.TotalMyInvestmentInPlan = planService.GetInvsetmentUserOfPlan(db, id, UserSetAuthCookie.GetUserID(User.Identity.Name));
            return View(new PaymentOnlineViewModel() { BusinessPlan_id = id });
        }

        [Authorize]
        [HttpPost]
        public ActionResult OnlinePayment(PaymentOnlineViewModel paymentOnlineViewModel)
        {
            if (ModelState.IsValid)
            {
                Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == paymentOnlineViewModel.BusinessPlan_id);

                // حداقل مبلغ برای سرمایه گذاری
                ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                int MaximumInvestment = Convert.ToInt32((Convert.ToInt32(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                // حداکثر مبلغ برای سرمایه گذاری
                ViewBag.MaximumInvestment = MaximumInvestment;

                PaymentPriceValidation paymentPriceValidation = planService.ValidationPaymentPrice(db, paymentOnlineViewModel.BusinessPlan_id,
                    paymentOnlineViewModel.PaymentPrice, UserSetAuthCookie.GetUserID(User.Identity.Name));
                if (paymentPriceValidation.Validation)
                {
                    UserService userService = new UserService();
                    int intCurrentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);

                    Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                    {
                        BusinessPlan_id = paymentOnlineViewModel.BusinessPlan_id,
                        IsPaid = false,
                        IsConfirmedFromAdmin = false,
                        PaidDateTime = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser_id = intCurrentUserId,
                        PaymentUser_id = intCurrentUserId,
                        PaymentPrice = paymentOnlineViewModel.PaymentPrice,
                        PaymentType_id = 2,
                        IsDelete = false,
                        PaymentStatus = 2,
                        AdminCheckDate = null
                    };
                    db.Tbl_BusinessPlanPayment.Add(tbl_BusinessPlanPayment);
                    db.SaveChanges();
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HamAfarin.ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp =
                        new HamAfarin.ZarinPal.PaymentGatewayImplementationServicePortTypeClient();

                    string Authority;
                    int status = zp.PaymentRequest("code", paymentOnlineViewModel.PaymentPrice.Value, "test portal",
                        "email", "mobile", ConfigurationManager.AppSettings["ThisDomain"] + "/Payment/VerifyPayment/" + tbl_BusinessPlanPayment.PaymentID, out Authority);
                    if (status == 100)
                    {
                        Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                    }
                    else
                    {
                        ViewBag.Error = "Error : " + status;
                    }
                    //ViewBag.IsPaymentSuccess = true;
                    return View();
                }
                else
                {
                    // سرمایه گذاری من در این طرح
                    ViewBag.TotalMyInvestmentInPlan = planService.GetInvsetmentUserOfPlan(db, paymentOnlineViewModel.BusinessPlan_id.Value
                        , UserSetAuthCookie.GetUserID(User.Identity.Name));
                    ModelState.AddModelError("PaymentPrice", paymentPriceValidation.Error.ToString());
                }
            }
            return View(paymentOnlineViewModel);
        }



        public ActionResult VerifyPayment(string id)
        {

            try
            {
                Tbl_PaymentOnlineDetils qPaymentOnline = db.Tbl_PaymentOnlineDetils.FirstOrDefault(p => p.PaymentDetilsID == id);
                if (qPaymentOnline != null)
                {

                    string invoiceNumber = Request.QueryString["iN"];
                    string invoiceDate = Request.QueryString["iD"];
                    string TransactionReferenceID = Request.QueryString["tref"];
                    qPaymentOnline.TransactionReferenceID = TransactionReferenceID;
                    string strResult = ReadPaymentResult(TransactionReferenceID);

                    qPaymentOnline.ShaparakCheckTransactionResult = strResult;
                    //lblInvoiceNumber.Text = invoiceNumber;
                    //lblInvoiceDate.Text = invoiceDate;
                    //lblTransactionReferenceID.Text = TransactionReferenceID;
                    db.SaveChanges();

                    if (!strResult.Contains("ReferenceNumber"))
                    {
                        // Response.Write("تراکنش  انجام نشد ");
                        ViewBag.Result = "عملیات ناموفق ";
                    }
                    else
                    {
                        var res = strResult.Split(':', ',');
                        string[] pay = res[21].Split('.');
                        string ShaparakRefNumber = ConfirmPayment(pay[0], invoiceNumber, invoiceDate);

                        var shaparakbum = ShaparakRefNumber.Split(':', ',');
                        if (ShaparakRefNumber.Contains("ShaparakRefNumber"))
                        {
                            qPaymentOnline.ShaparakVerifyPayment = ShaparakRefNumber;
                            qPaymentOnline.IsFinally = true;
                            qPaymentOnline.FinallyDate = DateTime.Now;
                            db.SaveChanges();
                            ViewBag.IsSuccess = true;
                            ViewBag.TransactionReferenceID = TransactionReferenceID;

                            // 4 = سرمایه گذاری
                            Tbl_Sms qSms = db.Tbl_Sms.Find(4); 
                            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                            string formatedMobileNumber = "98" + qUser.MobileNumber.Substring(1);
                            (bool Success, string Message) result = oSms.AdpSendSms(formatedMobileNumber, qSms.Message);

                            return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                        }

                        // lblResult.ForeColor = System.Drawing.Color.Green;

                        //lblTraceNumber.Text = res[1];
                        //lblReferenceNumber.Text = res[3];
                        //lblTransactionDate.Text = res[5];
                        //lblAction.Text = res[9];
                        //lblTraceNumber.Text = res[13];
                        //lblInvoiceNumber.Text = res[15];
                        //lblInvoiceDate.Text = res[15];
                        //lblMerchantCode.Text = res[19];
                        //lblTerminalCode.Text = res[21];
                        //lblAmount.Text = res[23];
                        //lblMessage.Text = res[31];
                        //lblResult.Text = strResult;
                        //lblShaparak.Text = ShaparakRefNumber;

                        //lblshMessage.Text = shaparakbum[9];
                        //lblMaskedCardNumber.Text = res[1];
                        //lblHashedCardNumber.Text = res[3];
                        //lblShaparakRefNumber.Text = res[3];

                        //lblResult.Text = strResult;
                        //lblShaparak.Text = ShaparakRefNumber;

                    }
                }
            }
            catch
            {
            }


            //Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(p => p.PaymentID == id);


            //    var payment = db.Tbl_BusinessPlanPayment.Find(id);


            //    if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            //    {
            //        if (Request.QueryString["Status"].ToString().Equals("OK"))
            //        {
            //            int Amount = Convert.ToInt32(payment.PaymentPrice.Value);
            //            long RefID;
            //            System.Net.ServicePointManager.Expect100Continue = false;
            //            HamAfarin.ZarinPal.PaymentGatewayImplementationServicePortTypeClient zp = new HamAfarin.ZarinPal.PaymentGatewayImplementationServicePortTypeClient();

            //            int Status = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Request.QueryString["Authority"].ToString(), Amount, out RefID);

            //            if (Status == 100)
            //            {
            //                payment.IsPaid = true;
            //                payment.TransactionPaymentCode = RefID.ToString();
            //                db.SaveChanges();
            //                ViewBag.IsSuccess = true;
            //                ViewBag.RefId = RefID;
            //                // Response.Write("Success!! RefId: " + RefID);
            //                return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { Area = "UserPanel", id = payment.PaymentID });

            //            }
            //            else
            //            {
            //                ViewBag.Status = Status;
            //            }

            //        }
            //        else
            //        {
            //            Response.Write("Error! Authority: " + Request.QueryString["Authority"].ToString() + " Status: " + Request.QueryString["Status"].ToString());
            //        }
            //    }
            //    else
            //    {
            //        Response.Write("Invalid Input");
            //    }
            return View();
        }


        private string ReadPaymentResult(string TransactionReferenceID)
        {
            DataPost dp = new DataPost();
            dp.TransactionReferenceID = TransactionReferenceID;
            string output = JsonConvert.SerializeObject(dp);
            string sign = GetSign(output);

            byte[] textArray = Encoding.UTF8.GetBytes(output);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/CheckTransactionResult");
            request.Method = "POST";
            request.ContentType = "Application/Json";
            request.ContentLength = textArray.Length;
            request.Headers.Add("Sign", sign);
            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
        }

        private string ConfirmPayment(string amount, string invoiceNumber, string invoiceDate)
        {

            // //int merchentCode = 4650168;  //کد پذیرنده
            // //int terminalCode = 1837060; //کد ترمینال  
            string merchantCode = "4650168";
            string terminalCode = "1837060";
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            DataPost dp = new DataPost();
            dp.InvoiceNumber = invoiceNumber;
            dp.InvoiceDate = invoiceDate;
            dp.MerchantCode = merchantCode;
            dp.TerminalCode = terminalCode;
            dp.Amount = amount.ToString();
            dp.TimeStamp = timeStamp;
            string output = JsonConvert.SerializeObject(dp);
            string sign = GetSign(output);

            byte[] textArray = Encoding.UTF8.GetBytes(output);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/VerifyPayment");
            request.Method = "POST";
            request.ContentType = "Application/Json";
            request.ContentLength = textArray.Length;
            request.Headers.Add("Sign", sign);

            request.GetRequestStream().Write(textArray, 0, textArray.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string result = reader.ReadToEnd();
            return result;
        }

        public ActionResult InvestmentSummary()
        {
            List<Tbl_BusinessPlanPayment> qPayments = db.Tbl_BusinessPlanPayment.Where(p => p.IsConfirmedFromAdmin && p.IsPaid).ToList();
            int qInvestmentablePlanCount = db.Tbl_BussinessPlans.Where(p => p.IsActive && p.IsDeleted == false).Count();
            long qAmountCapitalRaised = qPayments.Sum(p => p.PaymentPrice).Value;
            int qInvestmentCountPerson = qPayments.Select(p => p.PaymentUser_id).Distinct().Count();
            int qInvestmentSuccessPlanCount = db.Tbl_BussinessPlans.Where(p => p.IsSuccessBussinessPlan).Count();
            InvestmentSummaryeViewModel investmentSummaryeViewModel = new InvestmentSummaryeViewModel()
            {
                AmountCapitalRaised = qAmountCapitalRaised,
                InvestmentablePlanCount = qInvestmentablePlanCount,
                InvestmentCountPerson = qInvestmentCountPerson,
                InvestmentSuccessPlanCount = qInvestmentSuccessPlanCount

            };
            return PartialView(investmentSummaryeViewModel);
        }
    }
}