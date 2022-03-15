using DataLayer;
using HamAfarin;
using HamAfarin.ZarinPal;
using KooyWebApp_MVC.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Hamafarin.Controllers
{
    public class PaymentController : Controller
    {
        HamAfarinDBEntities db = new HamAfarinDBEntities();
        PlanService planService = new PlanService();
        UserService userService = new UserService();
        ShaparakMessageEncoding ShaparakMessage = new ShaparakMessageEncoding();
        SMS oSms = new SMS();
        string zpSecret = "bfb039ca-b62b-434a-a7fc-ad2b53c981b0";

        // GET: Payment
        [Authorize]
        public ActionResult SelectPaymentType(int id)
        {
            int UserID = UserSetAuthCookie.GetUserID(User.Identity.Name);
            Tbl_UserProfiles qUserProfiles = db.Tbl_UserProfiles.FirstOrDefault(p => p.User_id == UserID);
            if (qUserProfiles != null && qUserProfiles.IsActive)
            {
                Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == id && p.IsActive && p.IsDeleted == false);

                if (qBussinessPlans == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                if (qBussinessPlans.IsActive == false ||
                    qBussinessPlans.InvestmentStartDate > DateTime.Now ||
                    qBussinessPlans.InvestmentExpireDate < DateTime.Now)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

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
                selectPayment.ContractFileName = qBussinessPlans.ContractFileName;
                // کل سرمایه گذاری شما
                selectPayment.TotalInvestment = planService.TotalUserInvestmentForPlan(db, id, myUserId);
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
                    Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID && p.IsActive && p.IsDeleted == false);

                    // حداقل مبلغ برای سرمایه گذاری
                    ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                    long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                    // حداکثر مبلغ برای سرمایه گذاری
                    ViewBag.MaximumInvestment = MaximumInvestment;

                    int currentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                    bool userIslegal = db.Tbl_Users.Where(x => x.UserID == currentUserId).Select(x => x.IsLegal).FirstOrDefault();

                    PaymentPriceValidation paymentPriceValidation = planService.PaymentPriceValidation(db, selectPaymentTypeViewModel.BusinessPlanID,
                        selectPaymentTypeViewModel.OnlinePaymentPrice, currentUserId, userIslegal);
                    if (paymentPriceValidation.Validation)
                    {
                        Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                        {
                            BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
                            IsPaid = false,
                            IsConfirmedFromAdmin = false,
                            PaidDateTime = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUser_id = currentUserId,
                            PaymentUser_id = currentUserId,
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

                        string redirectAddress = "https://www.hamafarin.ir/Payment/VerifyPayment/" + tbl_PaymentOnline.PaymentDetilsID;

                        #region Pasargad
                        //string timeStamp = tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss");
                        //string amount = dclAmount.ToString();
                        //string invoiceNumber = tbl_BusinessPlanPayment.InvoiceNumber;
                        //string ActionResult = "1003";
                        //string invoiceDate = tbl_BusinessPlanPayment.CreateDate.Value.ToString("yyyy/MM/dd");
                        //AppSettingsReader appSetting = new AppSettingsReader();
                        //string merchantCode = "4650168";  //کد پذیرنده
                        //string terminalCode = "1837060"; //کد ترمینال  

                        //DataPost dp = new DataPost();
                        //dp.InvoiceNumber = invoiceNumber;
                        //dp.InvoiceDate = invoiceDate;
                        //dp.MerchantCode = merchantCode;
                        //dp.TerminalCode = terminalCode;
                        //dp.Amount = amount;
                        //dp.RedirectAddress = redirectAddress;
                        //dp.Action = ActionResult;
                        //dp.TimeStamp = timeStamp;
                        //string output = JsonConvert.SerializeObject(dp);
                        //string sign = GetSign(output);

                        //byte[] textArray = Encoding.UTF8.GetBytes(output);
                        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/GetToken");
                        //request.Method = "POST";
                        //request.ContentType = "Application/Json";
                        //request.ContentLength = textArray.Length;
                        //request.Headers.Add("Sign", sign);

                        //request.GetRequestStream().Write(textArray, 0, textArray.Length);
                        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        //StreamReader reader = new StreamReader(response.GetResponseStream());
                        //string result = reader.ReadToEnd();
                        //tbl_PaymentOnline.ShaparakMessageGetToken = result;

                        //if (result.Contains("Token"))
                        //{
                        //    var res = result.Split(':', ',');
                        //    string token = res[1];
                        //    token = token.Replace("\"", "");

                        //    tbl_PaymentOnline.ShaparakToken = token;
                        //    db.SaveChanges();

                        //    Response.Redirect("https://pep.shaparak.ir/payment.aspx?n=" + token);
                        //    return View();
                        //}
                        #endregion

                        // زرین پال
                        int amount = (int)tbl_BusinessPlanPayment.PaymentPrice * 10;
                        ServicePointManager.Expect100Continue = false;
                        var zp = new PaymentGatewayImplementationServicePortTypeClient();
                        string authority;
                        int Status = zp.PaymentRequest(zpSecret, amount,
                            $"هم آفرین سرمایه گذاری در طرح {selectPaymentTypeViewModel.BussinessName}",
                            "irfintech.co@gmail.com", "09131880434", redirectAddress, out authority);

                        if (Status == 100)
                        {
                            tbl_PaymentOnline.ShaparakMessageGetToken = authority;
                            Response.Redirect("https://www.zarinpal.com/pg/StartPay/" + authority);
                        }
                        else
                            ViewBag.Massage = "فعلا ارتباط با درگاه مقدور نمی باشد";

                        db.SaveChanges();

                        return View(selectPaymentTypeViewModel);
                    }
                    else
                    {
                        // سرمایه گذاری من در این طرح
                        ViewBag.TotalMyInvestmentInPlan = planService.TotalUserInvestmentForPlan(db, selectPaymentTypeViewModel.BusinessPlanID
                            , currentUserId);
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
                    Tbl_BussinessPlans qBussinessPlans = db.Tbl_BussinessPlans.FirstOrDefault(p => p.BussinessPlanID == selectPaymentTypeViewModel.BusinessPlanID && p.IsActive && p.IsDeleted == false);

                    // حداقل مبلغ برای سرمایه گذاری
                    ViewBag.MinimumAmountInvest = qBussinessPlans.MinimumAmountInvest;
                    long MaximumInvestment = Convert.ToInt64((Convert.ToInt64(qBussinessPlans.AmountRequiredRoRaiseCapital) / 100) * qBussinessPlans.MaximumInvestmentPercentage);
                    // حداکثر مبلغ برای سرمایه گذاری
                    ViewBag.MaximumInvestment = MaximumInvestment;

                    int currentUserId = UserSetAuthCookie.GetUserID(User.Identity.Name);
                    bool userIslegal = db.Tbl_Users.Where(x => x.UserID == currentUserId).Select(x => x.IsLegal).FirstOrDefault();

                    PaymentPriceValidation paymentPriceValidation = planService.PaymentPriceValidation(db, selectPaymentTypeViewModel.BusinessPlanID, selectPaymentTypeViewModel.OfflinePaymentPrice
                        , currentUserId, userIslegal);

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

                        Tbl_BusinessPlanPayment tbl_BusinessPlanPayment = new Tbl_BusinessPlanPayment()
                        {
                            BusinessPlan_id = selectPaymentTypeViewModel.BusinessPlanID,
                            IsPaid = false,
                            IsConfirmedFromAdmin = false,
                            PaidDateTime = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUser_id = currentUserId,
                            PaymentUser_id = currentUserId,
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
                        ViewBag.TotalMyInvestmentInPlan = planService.TotalUserInvestmentForPlan(db, selectPaymentTypeViewModel.BusinessPlanID
                            , currentUserId);
                        ModelState.AddModelError("OfflinePaymentPrice", paymentPriceValidation.Error.ToString());
                    }
                    if (imgPaymentImageNameUploaded != null) selectPaymentTypeViewModel.PaymentImageName = imgPaymentImageNameUploaded.FileName;
                    return View(selectPaymentTypeViewModel);
                }
            }
            return View("SelectPaymentType/" + selectPaymentTypeViewModel.BusinessPlanID);
        }

        public ActionResult VerifyPayment(string id)
        {
            try
            {
                Tbl_PaymentOnlineDetils qPaymentOnline = db.Tbl_PaymentOnlineDetils.FirstOrDefault(p => p.PaymentDetilsID == id);
                Tbl_BusinessPlanPayment qBusinessPlanPayment = db.Tbl_BusinessPlanPayment.FirstOrDefault(p => p.PaymentID == qPaymentOnline.Payment_id);
                
                // زرین پال
                if (qPaymentOnline != null)
                {
                    var status = Request.QueryString["Status"];
                    var authority = Request.QueryString["Authority"];
                    if (string.IsNullOrEmpty(status) == false && status.Equals("OK") && string.IsNullOrEmpty(authority) == false)
                    {
                        int amount = (int)qBusinessPlanPayment.PaymentPrice * 10;
                        long RefID;
                        ServicePointManager.Expect100Continue = false;
                        var zp = new PaymentGatewayImplementationServicePortTypeClient();

                        int Status = zp.PaymentVerification(zpSecret, authority, amount, out RefID);

                        if (Status == 100)
                        {
                            qPaymentOnline.ShaparakVerifyPayment = authority;
                            qPaymentOnline.IsFinally = true;
                            qBusinessPlanPayment.TransactionPaymentCode = RefID.ToString();
                            qBusinessPlanPayment.IsPaid = true;
                            qPaymentOnline.FinallyDate = DateTime.Now;
                            db.SaveChanges();
                            ViewBag.IsSuccess = true;
                            ViewBag.TransactionReferenceID = RefID;

                            // 4 = سرمایه گذاری
                            Tbl_Sms qSms = db.Tbl_Sms.Find(4);
                            Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                            (bool Success, string Message) result = oSms.SendSms(qUser.MobileNumber, qSms.Message);

                            return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                        }
                        else
                        {
                            ViewBag.IsSuccess = false;
                            ViewBag.Result = "عملیات ناموفق";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.IsSuccess = false;
                        ViewBag.Result = "عملیات ناموفق";
                        return View();
                    }

                    //درگاه پاسارگاد
                    //string invoiceNumber = Request.QueryString["iN"];
                    //string invoiceDate = Request.QueryString["iD"];
                    //string TransactionReferenceID = Request.QueryString["tref"];
                    //qPaymentOnline.TransactionReferenceID = TransactionReferenceID;

                    //// دریافت اطلاعات از درگاه
                    //string strResult = ReadPaymentResult(TransactionReferenceID);

                    //qPaymentOnline.ShaparakCheckTransactionResult = strResult;

                    //if (!strResult.Contains("ReferenceNumber"))
                    //{
                    //    ViewBag.IsSuccess = false;
                    //    ViewBag.Result = "عملیات ناموفق";
                    //    return View();
                    //}

                    //var res = strResult.Split(':', ',');
                    //string[] pay = res[21].Split('.');

                    //long raisedPrice = planService.GetRaisedPrice(db, qBusinessPlanPayment.Tbl_BussinessPlans.BussinessPlanID) + qBusinessPlanPayment.PaymentPrice.Value;
                    //long totalPrice = long.Parse(qBusinessPlanPayment.Tbl_BussinessPlans.AmountRequiredRoRaiseCapital);

                    //if (qBusinessPlanPayment.Tbl_BussinessPlans.IsOverflowInvestment == false && totalPrice < raisedPrice)
                    //{
                    //    // برگشت وجه
                    //    string refundResult = RefundPayment(pay[0], invoiceNumber, invoiceDate);
                    //    ViewBag.Result = "برگشت وجه";
                    //    return View();
                    //}

                    //// تایید پرداخت به درگاه
                    //string ShaparakRefNumber = ConfirmPayment(pay[0], invoiceNumber, invoiceDate);

                    //var shaparakbum = ShaparakRefNumber.Split(':', ',');
                    //if (ShaparakRefNumber.Contains("ShaparakRefNumber"))
                    //{
                    //    qPaymentOnline.ShaparakVerifyPayment = ShaparakRefNumber;
                    //    qPaymentOnline.IsFinally = true;
                    //    qBusinessPlanPayment.TransactionPaymentCode = TransactionReferenceID;
                    //    qBusinessPlanPayment.IsPaid = true;
                    //    qPaymentOnline.FinallyDate = DateTime.Now;
                    //    db.SaveChanges();
                    //    ViewBag.IsSuccess = true;
                    //    ViewBag.TransactionReferenceID = TransactionReferenceID;

                    //    // 4 = سرمایه گذاری
                    //    Tbl_Sms qSms = db.Tbl_Sms.Find(4);
                    //    Tbl_Users qUser = db.Tbl_Users.FirstOrDefault(u => u.UserID == qPaymentOnline.Tbl_BusinessPlanPayment.Tbl_BussinessPlans.User_id);
                    //    (bool Success, string Message) result = oSms.SendSms(qUser.MobileNumber, qSms.Message);

                    //    return RedirectToAction("SinglePaymentBusinessPlan", "UserPaymentBusinessPlan", new { area = "UserPanel", id = qPaymentOnline.Payment_id, notify = true });
                    //}
                }
            }
            catch
            {
            }
            return View();
        }

        public ActionResult InvestmentSummary()
        {
            List<Tbl_BusinessPlanPayment> qPayments = db.Tbl_BusinessPlanPayment.Where(p => p.IsConfirmedFromAdmin && p.IsPaid).ToList();
            int qActiveUsers = db.Tbl_Users.Where(p => p.IsActive && p.IsDeleted == false).Count();
            long qAmountCapitalRaised = qPayments.Sum(p => p.PaymentPrice).Value;
            int qInvestmentCountPerson = qPayments.Select(p => p.PaymentUser_id).Distinct().Count();
            int qInvestmentSuccessPlanCount = db.Tbl_BussinessPlans.Where(p => p.IsSuccessBussinessPlan).Count();
            InvestmentSummaryViewModel investmentSummaryViewModel = new InvestmentSummaryViewModel()
            {
                AmountCapitalRaised = qAmountCapitalRaised,
                ActiveUsers = qActiveUsers,
                InvestmentCountPerson = qInvestmentCountPerson,
                InvestmentSuccessPlanCount = qInvestmentSuccessPlanCount
            };
            return PartialView(investmentSummaryViewModel);
        }

        private string GetSign(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString("<RSAKeyValue><Modulus>tgPgJTn9M7pKUeIxZOkw9ZaKASSGmMtKzsG0Oe4kl4UHOaAfn/TwWrTgvyiNgAvxFQCw1A7YiMqPiDe/t9KbZoZz1uNBt3SCYDVsSVewpzdPhgmaDnUrylCSkujzTzlBqAKJ8alwW8hYKOOlbPPtY/0aWQ3bbpT49+DaNheyMwM=</Modulus><Exponent>AQAB</Exponent><P>9C4fmxKemcbQd58hm4KP0yR7SPXH/mSui0MA4BHv4NKlj7fP7DA+Uc47MKRrVbuSqe0eKo8K8u5J6ztZoO6nIQ==</P><Q>vtNnctPiGb+Bdy+jlAI4NPlePnLTo8D+4819cwKTD4NGEU0Qwhza1O6szrKemWyTGWpXPyVf/gbk06lLC0mpow==</Q><DP>Lmi1yRt42XFYHeQ41v2xqEe+xtcv88HfCsjpWa0PEoP2w6ID+rgQoCu6RDx7ygekkHdozF3zjsiLdBILrvKtAQ==</DP><DQ>Dksti4ddf0o9+1yBJzwHU8h+C7V0Lubs8MlapTvDIj1WCUO5hqC8r4h1P0JX6OweFKBHir5U82U2zLf4nA7Xew==</DQ><InverseQ>rovdYQjQoQePVxTuO4WijIJkb030RUSuSoiy3nlyH+v2eM1Q7jnTFHN0cWSNqMW486PCkUiDBH7pq2CIJFcnbg==</InverseQ><D>tKx4LLu5SUWcTFe5LDAFt2JtLuEw8i6p3T6ORgrMK9OS7nKxsbgTdhaiGV6JxxcTggOjg3wRGQfpHhAosLHQKm9xwylIV7uM+CbEaNDva6WPK2w45X/vS2/WDdUOyb2AswgibE6+G4ALeoil/shXeoMfn5PsXkLKtzaMaoNDA4E=</D></RSAKeyValue>");
            byte[] signMain = rsa.SignData(Encoding.UTF8.GetBytes(data), new
            SHA1CryptoServiceProvider());
            string sign = Convert.ToBase64String(signMain);
            return sign;
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
            string merchantCode = "4650168"; //کد پذیرنده
            string terminalCode = "1837060"; //کد ترمینال  
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

        private string RefundPayment(string amount, string invoiceNumber, string invoiceDate)
        {
            string merchantCode = "4650168"; //کد پذیرنده
            string terminalCode = "1837060"; //کد ترمینال  
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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/RefundPayment");
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

        #region Test Code
        //public ActionResult TestReturn1()
        //{
        //    DateTime now = DateTime.Now;

        //    string token = "";
        //    string timeStamp = now.ToString("yyyy/MM/dd HH:mm:ss");
        //    string amount = "10000";
        //    string invoiceNumber = "HAfarin-000904-2";
        //    string actionResult = "1003";
        //    string invoiceDate = now.ToString("yyyy/MM/dd");
        //    string merchantCode = "4650168";  //کد پذیرنده
        //    string terminalCode = "1837060"; //کد ترمینال  
        //    string redirectAddress = "https://www.hamafarin.ir/Payment/TestReturn2";

        //    DataPost dp = new DataPost();
        //    dp.InvoiceNumber = invoiceNumber;
        //    dp.InvoiceDate = invoiceDate;
        //    dp.MerchantCode = merchantCode;
        //    dp.TerminalCode = terminalCode;
        //    dp.Amount = amount;
        //    dp.RedirectAddress = redirectAddress;
        //    dp.Action = actionResult;
        //    dp.TimeStamp = timeStamp;
        //    string output = JsonConvert.SerializeObject(dp);
        //    string sign = GetSign(output);

        //    byte[] textArray = Encoding.UTF8.GetBytes(output);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pep.shaparak.ir/Api/v1/Payment/GetToken");
        //    request.Method = "POST";
        //    request.ContentType = "Application/Json";
        //    request.ContentLength = textArray.Length;
        //    request.Headers.Add("Sign", sign);

        //    request.GetRequestStream().Write(textArray, 0, textArray.Length);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string result = reader.ReadToEnd();

        //    //Tbl_PaymentOnlineDetils tbl_PaymentOnline = new Tbl_PaymentOnlineDetils()
        //    //{
        //    //    CreateDate = now,
        //    //    FinallyDate = null,
        //    //    IsDelete = false,
        //    //    IsFinally = false,
        //    //    PaymentDetilsID = Guid.NewGuid().ToString(),
        //    //    Payment_id = null,
        //    //};
        //    //db.Tbl_PaymentOnlineDetils.Add(tbl_PaymentOnline);

        //    //tbl_PaymentOnline.ShaparakMessageGetToken = result;

        //    if (result.Contains("Token"))
        //    {
        //        var res = result.Split(':', ',');

        //        token = res[1];

        //        token = token.Replace("\"", "");

        //        //tbl_PaymentOnline.ShaparakToken = token;
        //        //db.SaveChanges();

        //        Response.Redirect("https://pep.shaparak.ir/payment.aspx?n=" + token);
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    //return View();
        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}

        //public ActionResult TestReturn2()
        //{
        //    string invoiceNumber = Request.QueryString["iN"];
        //    string invoiceDate = Request.QueryString["iD"];
        //    string TransactionReferenceID = Request.QueryString["tref"];

        //    // دریافت اطلاعات از درگاه
        //    string strResult = ReadPaymentResult(TransactionReferenceID);

        //    var res = strResult.Split(':', ',');
        //    string[] pay = res[21].Split('.');

        //    // برگشت وجه
        //    string refundResult = RefundPayment(pay[0], invoiceNumber, invoiceDate);
        //    ViewBag.Result = "برگشت وجه";

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //    //return View();
        //}

        #endregion
    }
}